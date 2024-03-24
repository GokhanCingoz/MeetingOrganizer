namespace MeetingOrganizer.Models
{
    public class ParticipantViewModel
    {
        public int Id { get; set; }
        public int MettingId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public Guid Guid { get; set; }
    }
}
