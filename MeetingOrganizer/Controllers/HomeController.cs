using MeetingOrganizer.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Text;

namespace MeetingOrganizer.Controllers
{
    public class HomeController : Controller
    {
        Uri apiAddresse = new Uri("https://localhost:7234/");
        private readonly HttpClient _client;


        public HomeController()
        {
            _client = new HttpClient();
            _client.BaseAddress = apiAddresse;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<MeetingViewModel> meetings = new List<MeetingViewModel>();
            var response = _client.GetAsync(apiAddresse + "Meeting/Get").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                meetings = JsonConvert.DeserializeObject<List<MeetingViewModel>>(data);
            }
            return View(meetings);
        }

        [HttpPost]
        public IActionResult SaveMeeting(MeetingViewModel meeting)
        {
            List<ParticipantViewModel> participants = string.IsNullOrEmpty(HttpContext.Session.GetString("Participants")) ? new List<ParticipantViewModel>() : JsonConvert.DeserializeObject<List<ParticipantViewModel>>(HttpContext.Session.GetString("Participants"));

            meeting.Participants = participants;
            HttpContext.Session.SetString("Participants", JsonConvert.SerializeObject(new List<ParticipantViewModel>()));

            var jsonMeeting = JsonConvert.SerializeObject(meeting);
            var content = new StringContent(jsonMeeting, Encoding.UTF8, "application/json");

            HttpResponseMessage response;

            if (meeting.Id == 0)
            {
                response = _client.PostAsync("Meeting/Add", content).Result;
            }
            else
            {
                response = _client.PutAsync("Meeting/Update", content).Result;
            }


            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                return Json(new { message = "Toplantı başarılı bir şekilde kaydedildi" });
            }
            else
            {
                return Json(new { message = "İstek gönderilirken bir hata oluştu" });
            }
        }


        public IActionResult DeleteMeeting(int id)
        {

            HttpResponseMessage response = _client.DeleteAsync("Meeting/Delete/" + id.ToString()).Result;

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                return Json(new { result = true, message = "Toplantı başarılı bir şekilde silindi" });
            }
            else
            {
                return Json(new { result = false, message = "İstek gönderilirken bir hata oluştu" });
            }
        }


        public PartialViewResult GetParticipantPartial()
        {
            List<ParticipantViewModel> participants = string.IsNullOrEmpty(HttpContext.Session.GetString("Participants")) ? new List<ParticipantViewModel>() : JsonConvert.DeserializeObject<List<ParticipantViewModel>>(HttpContext.Session.GetString("Participants"));

            return PartialView("_ParticipantPartialView", participants);
        }

        public IActionResult AddParticipant(ParticipantViewModel participant)
        {
            List<ParticipantViewModel> participants = string.IsNullOrEmpty(HttpContext.Session.GetString("Participants")) ? new List<ParticipantViewModel>() : JsonConvert.DeserializeObject<List<ParticipantViewModel>>(HttpContext.Session.GetString("Participants"));

            participant.Guid = Guid.NewGuid();
            participants.Add(participant);

            HttpContext.Session.SetString("Participants", JsonConvert.SerializeObject(participants));

            return Json(new { result = true });

        }

        public IActionResult DeleteParticipant(Guid guid)
        {
            List<ParticipantViewModel> participants = string.IsNullOrEmpty(HttpContext.Session.GetString("Participants")) ? new List<ParticipantViewModel>() : JsonConvert.DeserializeObject<List<ParticipantViewModel>>(HttpContext.Session.GetString("Participants"));

            var participant = participants.FirstOrDefault(x => x.Guid == guid);

            if (participant != null)
            {
                participants.Remove(participant);
            }

            HttpContext.Session.SetString("Participants", JsonConvert.SerializeObject(participants));

            return Json(new { result = true });
        }

        public IActionResult GetMeeting(int id)
        {
            HttpResponseMessage response = _client.GetAsync("Meeting/GetById/" + id.ToString()).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var meeting = JsonConvert.DeserializeObject<MeetingViewModel>(data);

                foreach (var participant in meeting.Participants)
                {
                    participant.Guid = Guid.NewGuid();
                }

                HttpContext.Session.SetString("Participants", JsonConvert.SerializeObject(meeting.Participants));

                return Json(new
                {
                    id= meeting.Id,
                    subject = meeting.Subject,
                    date = meeting.Date.ToString("yyyy-MM-dd"),
                    starttime = meeting.StartTime,
                    endtime = meeting.EndTime
                });
            }
            else
            {
                HttpContext.Session.SetString("Participants", JsonConvert.SerializeObject(new List<ParticipantViewModel>()));

                return Json(new MeetingViewModel());
            }
        }
    }
}