using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetingOrganizerApi.Models
{
    public class Participant
    {
        [Key]
        public int Id { get; set; }
        public int MeetingId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
