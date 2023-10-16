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
                return PartialView("LoginAuthenticatedView");
            }
            return PartialView("LoginDefaultView");
        }


        [HttpGet("authview")]
        public IActionResult GetLoginAuthenticatedView()
        {
            if(Request.Cookies.ContainsKey("SessionID"))
            {
                return PartialView("LoginAuthenticatedView");
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
            var response = new { login = false };

            if (user != null)
            {
                RestClient restClient = new RestClient("http://localhost:5134");
                RestRequest restRequest = new RestRequest("/api/Profiles/Name/{name}", Method.Get);
                restRequest.AddUrlSegment("name", user.Username);
                RestResponse restResponse = restClient.Execute(restRequest);

                Profile profile = JsonConvert.DeserializeObject<Profile>(restResponse.Content);

                if (restResponse.IsSuccessful && profile.Pwd.Equals(user.Password))
                {
                    Response.Cookies.Append("SessionID", profile.Name);
                    response = new { login = true };

                    Response.Cookies.Append("UserID", profile.UserID.ToString());
                    Response.Cookies.Append("Email", profile.Email);
                    Response.Cookies.Append("Address", profile.Address);
                    Response.Cookies.Append("Phone", profile.Phone.ToString());
                    Response.Cookies.Append("Pwd", profile.Pwd);
                }
            }

            return Json(response);
        }



    }
}
