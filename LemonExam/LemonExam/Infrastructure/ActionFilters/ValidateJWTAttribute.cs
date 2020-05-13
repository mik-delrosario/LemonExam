using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using LemonExam.Model;
using Newtonsoft.Json;
using System.Dynamic;
using System.Diagnostics;
using LemonExam.Infrastructure;
using LemonExam.Model.Master;

namespace LemonExam.Infrastructure.ActionFilters
{
    public class ValidateJWTAttribute : ActionFilterAttribute
    {
        #region Private Fields

        private readonly LocalDbContext _core;

        //LocalDbContext _core = new LocalDbContext();
        JwtSecurityTokenHandler _handler;
        //LocalDbContext _local = new LocalDbContext();

        #endregion

        public ValidateJWTAttribute(LocalDbContext core)
        {
            _core = core;
            _handler = new JwtSecurityTokenHandler();
        }


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var req = context.HttpContext.Request;
            var authHdr = req.Headers["Authorization"];

            var response = req.HttpContext.Response;
            DateTime now = DateTime.Now;
            string msg = string.Empty;

            try
            {
                if (authHdr.Count > 0)
                {
                    string[] parts = authHdr.ToString().Split(new Char[] { ' ', ':' }, StringSplitOptions.RemoveEmptyEntries); //Split on blank or colon
                    if (parts[0].Contains("Bearer"))
                    {
                        string jwtStr = parts[1].Replace("}", "").Replace("\"", "").Replace(" ", "");
                        var token = _handler.ReadToken(jwtStr);
                        if (token.ValidFrom != now && token.ValidTo != now)
                        {
                            parts = jwtStr.Split('.');
                            if (parts.Length == 3)
                            {
                                string jsonPayLoad = EncodeBase64(parts[1]);
                                if (!validateToken(jsonPayLoad))
                                {  //Check contents of jwt and set _core.CurrentUser
                                    response.Headers.Add("WWW-Authenticate", "JWT token rejected");
                                    context.Result = new BadRequestObjectResult("JWT token rejected");
                                    msg = "JWT token rejected";
                                }
                            }   //End if(parts
                            else
                            {
                                response.Headers.Add("WWW-Authenticate", "Invalid JWT token format");
                                context.Result = new BadRequestObjectResult("Invalid JWT token format");
                                msg = "Invalid JWT token format";
                            }
                        }   //End if(token
                        else
                        {
                            response.Headers.Add("WWW-Authenticate", "JWT token expired");
                            context.Result = new BadRequestObjectResult("JWT token expired");
                            msg = "JWT token expired";
                        }
                    }   //End if(parts[0]
                    else
                    {
                        response.Headers.Add("WWW-Authenticate", "Bearer realm=\"Users\"");
                        context.Result = new BadRequestObjectResult("Invalid auth scheme");
                        msg = "Invalid auth scheme";
                    }
                }   //End if(authHdr.Count
                else
                {
                    response.Headers.Add("WWW-Authenticate", "Missing authorization header");
                    context.Result = new BadRequestObjectResult("Missing authorization header");
                    msg = "Missing authorization header";
                }
            }
            catch (Exception ex)
            {
                response.Headers.Add("WWW-Authenticate", "Invalid authorization header");
                context.Result = new BadRequestObjectResult("Invalid authorization header");
                msg = "Invalid authorization header";
            }

            
        }

        public string EncodeBase64(string data)
        {
            string s = data.Trim().Replace(" ", "+");
            if (s.Length % 4 > 0)
                s = s.PadRight(s.Length + 4 - s.Length % 4, '=');
            return Encoding.UTF8.GetString(Convert.FromBase64String(s));
        }

        #region Private Methods

        private bool validateToken(string jwt)
        {
            IDictionary<string, object> dict = null;
            UserAccount user = null;
            int tryInt = 0;
            bool result = false;

            try
            {            
                var token = JsonConvert.DeserializeObject<ExpandoObject>(jwt);
                dict = (IDictionary<string, object>)token;
                string iss = dict["iss"].ToString();
                string siteName = dict["sub"].ToString();
                string sitePswd = dict["jti"].ToString();
                string id = dict["nonce"].ToString();
        
                if (int.TryParse(id, out tryInt))
                {
                    user = (from u in _core.UserAccounts
                            where u.SiteName == siteName && u.SitePassword == sitePswd && u.ID == tryInt
                            select u).FirstOrDefault();
                    if (user != null)
                    {
                        _core.CurrentUser = user;
                        result = true;
                    }
                }
            }
            catch (Exception ex) {
                var error = ex.InnerException.ToString();
                Debug.WriteLine(error);
            }

            return result;
        }

        #endregion
    }
}
