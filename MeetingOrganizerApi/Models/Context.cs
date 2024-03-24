using Microsoft.EntityFrameworkCore;

namespace MeetingOrganizerApi.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions options) : base(options) { }
        DbSet<Meeting> Meetings { get; set; }
        DbSet<Participant> Participants { get; set; }

    }
}
