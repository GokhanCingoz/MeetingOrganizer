namespace MeetingOrganizer.Models
{
    public class MeetingViewModel
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public List<ParticipantViewModel> Participants { get; set; }
    }
}