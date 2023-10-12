using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    public class DashController : Controller
    {

        [HttpGet("view")]
        public IActionResult GetView()
        {
            if (Request.Cookies.ContainsKey("SessionID"))
            {
                var cookieValue = Request.Cookies["SessionID"];

                RestClient restClient = new RestClient("http://localhost:5134");
                RestRequest restRequest = new RestRequest("/api/Profiles/Name/{name}", Method.Get);
                restRequest.AddUrlSegment("name", cookieValue);
                RestResponse restResponse = restClient.Execute(restRequest);

                Profile profile = JsonConvert.DeserializeObject<Profile>(restResponse.Content);
                ViewData["UserID"] = profile.UserID;
                ViewData["Name"] = profile.Name;
                ViewData["Email"] = profile.Email;
                ViewData["Phone"] = profile.Phone;
                ViewData["Address"] = profile.Address;
                ViewData["Pwd"] = profile.Pwd;

                if (cookieValue == "admin")
                {
                    return PartialView("AdminDashView");
                }
                else
                {
                    return PartialView("UserDashView");
                }
            }

            return PartialView("DefaultDashView");
        }


        [HttpPut("updateMyDetails")]
        public IActionResult UpdateMyDetails([FromBody] Profile profile)
        {
            var response = new { ok = false };

            if (profile != null)
            {
                profile.UserID = (uint)ViewData["UserID"];
                profile.Name = (string)ViewData["Name"];
                profile.Address = (string)ViewData["Address"];
                profile.Pwd = (string)ViewData["Pwd"];

                RestClient restClient = new RestClient("http://localhost:5134");
                RestRequest restRequest = new RestRequest("/api/Profiles/{id}", Method.Put);
                restRequest.AddUrlSegment("id", profile.UserID);
                restRequest.AddBody(profile);
                RestResponse restResponse = restClient.Execute(restRequest);

                if (restResponse.IsSuccessful)
                {
                    response = new { ok = true };
                }
            }

            return Json(response);
        }



    }
}