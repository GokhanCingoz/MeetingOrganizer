using MeetingOrganizerApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace MeetingOrganizerApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class MeetingController : ControllerBase
    {
        private Context _context;

        public MeetingController(Context context)
        {
            _context = context;
        }

        [HttpGet(Name = "Get")]
        public IEnumerable<Meeting> Get()
        {
            var meetings = _context.Set<Meeting>().Include(x => x.Participants);
            return meetings;
        }

        [HttpGet("{id:int}")]
        public Meeting GetById(int id)
        {
            var meeting = _context.Set<Meeting>().Include(x => x.Participants).FirstOrDefault(x => x.Id == id);
            
            return meeting;
        }

        [HttpPost]
        public IActionResult Add(Meeting meeting)
        {
            try
            {
                _context.Add(meeting);
                _context.SaveChanges();
                return Ok("Toplantı başarılı bir şekilde eklendi");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var participants = _context.Set<Participant>().Where(x => x.MeetingId == id).ToList();

                _context.RemoveRange(participants);

                var meeting = _context.Set<Meeting>().FirstOrDefault(x => x.Id == id);

                if (meeting == null)
                    return NotFound("Belirtilen toplantı bulunamadı.");

                _context.Remove(meeting);
                _context.SaveChanges();

                return Ok("Toplantı başarılı bir şekilde silindi");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Update(Meeting meeting)
        {
            try
            {
                var participants = _context.Set<Participant>().Where(x => x.MeetingId == meeting.Id).ToList();

                _context.RemoveRange(participants);

                foreach (var participant in meeting.Participants)
                {
                    participant.MeetingId = meeting.Id;
                }

                _context.AddRange(meeting.Participants);

                _context.Update(meeting);
                _context.SaveChanges();

                return Ok("Toplantı başarılı bir şekilde güncellendi");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
