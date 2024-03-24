using System.ComponentModel.DataAnnotations;

namespace MeetingOrganizerApi.Models
{
    public class Meeting
    {
        [Key]
        public int Id { get; set; }
        public string Subject { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public List<Participant> Participants { get; set; }
    }
}