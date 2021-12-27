using SignalRChatSPA.Data.Abstract;
using SignalRChatSPA.Data.Models;

namespace SignalRChatSPA.Data.Concrete
{
    public class MessageLogRepository : Repository<MessageLog>, IMessageLogRepository
    {
        private AppDbContext AppDbContext { get => _context as AppDbContext; }

        public MessageLogRepository(AppDbContext context) : base(context)
        {
        }
    }
}