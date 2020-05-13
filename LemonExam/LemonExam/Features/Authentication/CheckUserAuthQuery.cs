using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using LemonExam.Features.Authentication;
using LemonExam.Infrastructure;
using LemonExam.Infrastructure;
using LemonExam.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LemonExam.Model.Master;

namespace LemonExam.Features.Authentication
{
    public class CheckUserAuthQuery : IRequestHandler<CheckUserAuth, CheckUserAuthResponse>
    {
        #region Private Fields
        
        private readonly IHttpContextAccessor _accessor;
        IHeaderDictionary _headers => _accessor.HttpContext.Request.Headers;
        private IOptions<JwtIssuerOptions> _options;

        #endregion

        #region Constructor

        public CheckUserAuthQuery(IHttpContextAccessor accessor, IOptions<JwtIssuerOptions> options)
        {
            _accessor = accessor;
            _options = options;
        }

        public CheckUserAuthResponse Handle(CheckUserAuth checkAuth)
        {
            if (checkAuth == null)
                throw new ArgumentNullException("checkAuth");
            var response = new CheckUserAuthResponse();
            UserAccount user = checkAuth.User;
            string token = string.Empty;

            try
            {
                if (_headers.ContainsKey("Authorization"))
                {
                    string authHeader = _headers["Authorization"].ToString();
                    string[] parts = authHeader.ToString().Split(new Char[] { ' ', ':' }, StringSplitOptions.RemoveEmptyEntries); //Split on blank or colon
                    if (parts[0].Contains(user.SiteName))
                    {     //Check name of key
                        token = parts[1].Replace("}", "").Replace("\"", "").Replace(" ", "");      //Extract the token value
                        //Ed Beltran
                        if (String.Compare(token, user.SitePassword) == 0)
                        {    //Generate a token and pass it back to the user
                            _options.Value.Issuer = $"LemonExam@{user.ID}";
                            _options.Value.Audience = "1";
                            _options.Value.SiteName = user.SiteName;
                            _options.Value.SitePassword = user.SitePassword;
                            _options.Value.ID = user.ID;
                            _options.Value.Subject = user.SiteName;
                            _options.Value.NotBefore = DateTime.Now;
                            var _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(user.SigningKey));
                            _options.Value.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
                            var newJWT = new GenerateJWT(_options);
                            response.JWToken = newJWT.CreateToken(user);
                            response.Message = $"{user.SiteName} is authorized.";
                            response.StatusCode = StatusCodes.Status200OK;
                          
                            response.TokenExpiration = DateTime.Now.AddMinutes(_options.Value.ValidFor.Minutes);
                        }
                        else
                        {      //Client password failed
                            response.StatusCode = StatusCodes.Status401Unauthorized;
                            response.Message = "Client not authorized.";
                        }
                    }   //End if(parts[0]
                    else
                    {      //Client site name failed
                        response.StatusCode = StatusCodes.Status400BadRequest;
                        response.Message = "Client name not recognized";
                    }
                }   //End if(_headers
                else
                {      //No authorization header
                    response.StatusCode = StatusCodes.Status401Unauthorized;
                    response.Message = "Authorization header is missing";
                }
            }   //End try
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.StatusCode = StatusCodes.Status500InternalServerError;
            }

            return response;
        }

        #endregion
    }
}
