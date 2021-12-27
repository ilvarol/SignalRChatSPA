using System;

namespace SignalRChatSPA.Data.Models
{
    public class MessageLog
    {
        public int Id { get; set; }
        public string Nickname { get; set; }
        public string GroupName { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}