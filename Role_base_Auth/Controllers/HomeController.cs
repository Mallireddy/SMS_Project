using Newtonsoft.Json;
using Role_base_Auth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Role_base_Auth.Controllers
{
    public class HomeController : Controller
    {
        private static string Webapiurl= "https://localhost:44303/";
        
        [HttpPost]
        public async Task< ActionResult> Gettoken(LoginModel model)
        {
            var tokenBased = string.Empty;
            using(var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.BaseAddress = new Uri(Webapiurl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType: "application/json"));
                //var responseMessage = await client.GetAsync(requestUri: "Account/ValidLogin?userName=admin&userPassword=admin");
                //var responseMessage = await client.GetAsync(requestUri: "Account/ValidLogin?userName="+model.UserName+ "&Password="+model.Password);
                var user = new LoginModel { UserName = model.UserName, Password = model.Password };
                var json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var responseMessage = await client.PostAsync("Account/ValidLogin", content);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var resulyMessage = responseMessage.Content.ReadAsStringAsync().Result;
                    tokenBased = JsonConvert.DeserializeObject<string>(resulyMessage);
                    //Session["TokenNumber"] = tokenBased;
                    //Session["UserName"] = model.UserName;
                   
                }
            }
            //return Content(tokenBased);
            return Content(JsonConvert.SerializeObject(new { token = tokenBased,userName =model.UserName }), "application/json");
        }
        public async Task< ActionResult> GetEmployee()
        {
            string RetrunMessage = String.Empty;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.BaseAddress = new Uri(Webapiurl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType: "application/json"));
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                //    parameter: Session["TokenNumber"].ToString()+":"+ Session["UserName"]);
                var authorizationHeader = Request.Headers["Authorization"].ToString();
                var tokenAndUsername = authorizationHeader.Replace("Bearer ", "").Split(':');
                var token = tokenAndUsername[0];
                var username = tokenAndUsername[1];
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer", parameter: token + ":" + username);
                var responseMessage = await client.GetAsync(requestUri: "Account/GetEmployee");
                if (responseMessage.IsSuccessStatusCode)
                {
                    var reslutMessage = responseMessage.Content.ReadAsStringAsync().Result;
                    RetrunMessage = JsonConvert.DeserializeObject<string>(reslutMessage);

                }
                else
                {
                    RetrunMessage = responseMessage.ReasonPhrase+','+responseMessage.StatusCode;
                }
            }
            return Content(RetrunMessage);
        }
    }

}