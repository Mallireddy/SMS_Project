using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Token_Oath_API.Models
{
    public class LoginModel
    {
        public int UserId { get; set; }
        public string userName { get; set; }
        public string Password { get; set; }
    }
}