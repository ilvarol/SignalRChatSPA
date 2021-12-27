using Microsoft.EntityFrameworkCore;
using SignalRChatSPA.Data.Models;

namespace SignalRChatSPA.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<MessageLog> MessageLogs { get; set; }
    }
}