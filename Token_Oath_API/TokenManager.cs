using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Web;

namespace Token_Oath_API
{
    public class TokenManager
    {
       /// <summary>
       /// This property used for Token key
       /// </summary>
        private static string Secret = "MN05OPLoDvbTTa/QkqLNMI7cPLguaRyHzyg7n5qNBvjQmtBhZ4SzYh4NBVCXi3kJHISXKP+oi2+bxr6CUYTR";
       /// <summary>
       /// This method used for the Token generation 
       /// </summary>
       /// <param name="userName"></param>
       /// <returns></returns>
        public static string GenerateToken( string userName)
        {
            byte[] key = Convert.FromBase64String(Secret); 
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
               Subject = new ClaimsIdentity(claims: new[] { new Claim(type: ClaimTypes.Name,value:userName) }),
               Expires = DateTime.UtcNow.AddMinutes(30),
               SigningCredentials = new SigningCredentials(securityKey,
               algorithm:SecurityAlgorithms.HmacSha256Signature)
             
            };
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);

            return handler.WriteToken(token);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwttoken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                if(jwttoken == null)
                {
                    return null;
                }
                byte[] key = Convert.FromBase64String(Secret);
                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
                SecurityToken securityToken;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, parameters, out securityToken);
                return principal;
            }
            catch 
            {
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string ValidateToken(string token)
        {
            string username = null;
            ClaimsPrincipal principal = GetPrincipal(token);
            if (principal == null)
                return null;
            ClaimsIdentity identity = null;
            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch(NullReferenceException)
            {
                return null;
            }
            Claim usernameClaim = identity.FindFirst(type: ClaimTypes.Name);
            username = usernameClaim.Value;
            return username;
        }
    }
}