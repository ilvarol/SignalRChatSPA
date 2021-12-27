using Microsoft.AspNetCore.SignalR;
using SignalRChatSPA.Abstract;
using SignalRChatSPA.Data.Abstract;
using SignalRChatSPA.Data.Models;
using SignalRChatSPA.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRChatSPA.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ICacheService _cache;
        private readonly IMessageLogRepository _messageLogRepository;

        public ChatHub(ICacheService cache, IMessageLogRepository messageLogRepository)
        {
            _cache = cache;
            _messageLogRepository = messageLogRepository;
        }

        public void Login(string nickname)
        {
            var client = new Client();
            client.ConnectionId = Context.ConnectionId;
            client.Nickname = nickname;

            var clientSource = _cache.Get<ClientSource>("clients");
            clientSource.Clients.Add(client);
            _cache.Add("clients", clientSource);
        }

        public async Task AddClientToGroup(string groupName)
        {
            var clientSource = _cache.Get<ClientSource>("clients");
            var client = clientSource.Clients.FirstOrDefault(c => c.ConnectionId == Context.ConnectionId);

            var groupSource = _cache.Get<GroupSource>("groups");
            var groupIndex = groupSource.Groups.FindIndex(x => x.GroupName == groupName);

            groupSource.Groups[groupIndex].Clients.Add(client);
            _cache.Add("groups", groupSource);

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            var messageLogs = await _messageLogRepository
                .Where(x => x.GroupName == groupName && x.CreatedAt >= DateTime.Now.AddMinutes(-5));

            StringBuilder messages = new StringBuilder();

            messageLogs.OrderByDescending(x => x.CreatedAt)
                .ToList()
                .ForEach(x => 
                {
                    messages.Append($"{x.Nickname} ({x.CreatedAt.ToString("HH:mm")}): {x.Message} <br />");
                });

            await Clients.Caller.SendAsync("history", messages.ToString());
        }

        public async Task SendMessageToGroupAsync(string groupName, string message)
        {
            var clientSource = _cache.Get<ClientSource>("clients");
            var sender = clientSource.Clients.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);

            await Clients.Group(groupName).SendAsync("receiveMessage", sender.Nickname, DateTime.Now.ToString("HH:mm"), message);

            var messageLog = new MessageLog()
            {
                Nickname = sender.Nickname,
                GroupName = groupName,
                Message = message,
                CreatedAt = DateTime.Now
            };

            await _messageLogRepository.AddAsync(messageLog);
        }
    }
}