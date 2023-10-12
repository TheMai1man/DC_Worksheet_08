using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {

        [HttpGet("defaultview")]
        public IActionResult GetDefaultView()
        {
            if(Request.Cookies.ContainsKey("SessionID"))
            {
                var cookieValue = Request.Cookies["SessionID"];

                // TODO: check sessionID matches a user name
                if(cookieValue == "admin" || cookieValue == "user")
                {
                    return PartialView("LoginAuthenticatedView");
                }
            }

            return PartialView("LoginDefaultView");
        }


        [HttpGet("authview")]
        public IActionResult GetLoginAuthenticatedView()
        {
            if(Request.Cookies.ContainsKey("SessionID"))
            {
                var cookieValue = Request.Cookies["SessionID"];

                //TODO: check session ID matches a user name from DB
                if (cookieValue == "admin" || cookieValue == "user")
                {
                    return PartialView("LoginAuthenticatedView");
                }
            }

            return PartialView("LoginErrorView");
        }


        [HttpGet("error")]
        public IActionResult GetLoginErrorView()
        {
            return PartialView("LoginErrorView");
        }


        [HttpPost("auth")]
        public IActionResult Authenticate([FromBody] User user)
        {
            // Return the partial view as HTML
            var response = new { login = false };

            // TODO: implement admin users into the system
            // validates admin
            if (user != null && user.Username.Equals("admin") && user.Password.Equals("adminPassword"))
            {
                Response.Cookies.Append("SessionID", "admin");
                response = new { login = true };
            }

            // validates regular users
            if(user != null)
            {
                RestClient restClient = new RestClient("http://localhost:5134");
                RestRequest restRequest = new RestRequest("/api/Profiles/Name/{name}", Method.Get);
                restRequest.AddUrlSegment("name", user.Username);
                RestResponse restResponse = restClient.Execute(restRequest);

                Profile profile = JsonConvert.DeserializeObject<Profile>(restResponse.Content);

                if (restResponse.IsSuccessful && profile.Pwd.Equals(user.Password))
                {
                    Response.Cookies.Append("SessionID", "user");
                    response = new { login = true };
                }
            }

            return Json(response);
        }


    }
}
