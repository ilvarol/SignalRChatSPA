using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRChatSPA.Models
{
    public class Client
    {
        public string ConnectionId { get; set; }
        public string Nickname { get; set; }
    }
}