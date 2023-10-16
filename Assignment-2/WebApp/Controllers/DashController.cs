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
                profile.UserID = Convert.ToUInt32(Request.Cookies["UserID"]);
                profile.Name = Request.Cookies["SessionID"];
                profile.Address = Request.Cookies["Address"];
                profile.Pwd = Request.Cookies["Pwd"];

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


        [HttpPost("createUser")]
        public IActionResult CreateUser([FromBody] Profile profile)
        {
            var response = new { ok = false };

            if (profile != null)
            {
                RestClient restClient = new RestClient("http://localhost:5134");
                RestRequest restRequest = new RestRequest("/api/Profiles", Method.Post);
                restRequest.AddBody(profile);
                RestResponse restResponse = restClient.Execute(restRequest);

                if (restResponse.IsSuccessful)
                {
                    response = new { ok = true };
                }
            }

            return Json(response);
        }


        [HttpPut("pwdReset")]
        public IActionResult PwdReset([FromBody] Profile profile)
        {
            var response = new { ok = false };

            if (profile != null)
            {
                profile.UserID = Convert.ToUInt32(Request.Cookies["UserID"]);
                profile.Name = Request.Cookies["SessionID"];
                profile.Address = Request.Cookies["Address"];
                profile.Phone = Convert.ToUInt32(Request.Cookies["Phone"]);
                profile.Email = Request.Cookies["Email"];

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


        [HttpGet("searchUser/{username}")]
        public IActionResult SearchUser(string username)
        {
            var response = new { ok = false };

            RestClient restClient = new RestClient("http://localhost:5134");
            RestRequest restRequest = new RestRequest("/api/Profiles/Name/{name}", Method.Get);
            restRequest.AddUrlSegment("name", username);
            RestResponse restResponse = restClient.Execute(restRequest);

            Profile profile = JsonConvert.DeserializeObject<Profile>(restResponse.Content);

            if(restResponse.IsSuccessful && profile != null)
            {
                ViewData["SearchID"] = profile.UserID;
                ViewData["SearchName"] = profile.Name;
                ViewData["SearchPhone"] = profile.Phone;
                ViewData["SearchEmail"] = profile.Email;
                ViewData["SearchAddress"] = profile.Address;
                ViewData["SearchPwd"] = profile.Pwd;

                response = new { ok = true };
            }

            return Json(response);
        }



    }
}