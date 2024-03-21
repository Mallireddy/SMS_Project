using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Token_Oath_API.Models;

namespace Token_Oath_API.Controllers
{
    public class AccountController : ApiController
    {
        private readonly Form_AuthEntities context = new Form_AuthEntities();
        [HttpPost]
        public HttpResponseMessage validLogin([FromBody] User model )
        {

            var result = context.Users.Any(e => e.Username == model.Username.ToLower() && e.Password == model.Password.ToLower());
            if (result !=false)
            //if (model.Username =="admin"&& model.Password == "admin")
            {
                return Request.CreateResponse(HttpStatusCode.OK, TokenManager.GenerateToken(model.Username));
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadGateway, message: "User name and Passowrd is invalid");
            }
        }
        [HttpGet]
        [CustomAuthenticationFilter]
        public HttpResponseMessage GetEmployee()
        {
            return Request.CreateResponse(HttpStatusCode.OK, value: "Successfully Valid");
        }
    }
}
