using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LemonExam.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System.Text;
using LemonExam.Shared;
using LemonExam.Features.Authentication;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace LemonExam.Features.Account
{
    [Route("api/v1/account")]
    //[ApiController]
    public class AccountController : BaseController
    {
        #region Private Fields
        
        private readonly IHttpContextAccessor _accessor;
        private HttpResponse _response => _accessor.HttpContext.Response;

        #endregion

        #region Constructor

        public AccountController(IMediator mediator, IHttpContextAccessor accessor) : base(mediator)
        {
            _accessor = accessor;
        }

        #endregion

        /// <summary>
        /// GET /access/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("access/{id}")]
        public async Task Access(int id)
        {
            string responseBody = null;
            var statusCode = 0;
            var ipaddress = this.HttpContext.Request.Host.Value;

            if (id > 0)
            {
                var userInfo = _mediator.Request(new UserInfo { SecNumber = id }); 
                if (userInfo.Data != null)
                {     //StoreNumber is valid, user was found
                    var userToken = _mediator.Send(new CheckUserAuth { User = userInfo.Data.User });    //Validate auth header for credentials
                    if (userToken.JWToken.Length > 0)
                    { //Credentials are good...create JWT, write it to user's UserAccount record, return token to user
                        var result = _mediator.Send(new UpdateUserToken { User = userInfo.Data.User, JWToken = userToken.JWToken, TokenExpiration = userToken.TokenExpiration });   //Update UserAccount record
                    }
                    statusCode = userToken.StatusCode;
                    responseBody = DefaultApiResponse.Create(userToken.JWToken, statusCode, userToken.Message);    //Format JWT bearer token response to user
                }   //End if(userInfo
                else
                {      //No UserInfo record in database
                    statusCode = userInfo.Data.StatusCode;
                    responseBody = DefaultApiResponse.Create(null, statusCode, userInfo.Data.Message);
                }
                //try
                //{
                //    Elmahio.Logger.Notify(
                //        string.Format("SignOn ({0})",
                //        userInfo.Data.User.SiteName),
                //        responseBody,
                //        Severity.Information.ToString(),
                //        "access",
                //        "api/v1/account/",
                //        ipaddress,
                //        statusCode);
                //}
                //catch(Exception ex)
                //{

                //}
                //log to elmah.io


            }   //End if(id > 0)
            else
            {      
                //No StoreNumber specified
                statusCode = StatusCodes.Status400BadRequest;
                responseBody = DefaultApiResponse.Create(null, statusCode, "Invalid access request; store number missing");
                //try
                //{
                //    Elmahio.Logger.Notify(
                //    "SignOn",
                //    responseBody,
                //    Severity.Information.ToString(),
                //    "access",
                //    "api/v1/account/",
                //    ipaddress,
                //    statusCode);
                //}
                //catch(Exception ex)
                //{

                //}
                //log to elmah.io
            
            }

            //Set the status code & content type and write to HttpResponse body
            _response.StatusCode = statusCode;
            _response.ContentType = new MediaTypeHeaderValue("application/json").ToString();
           _response.ContentLength = responseBody.Length;

            await _response.WriteAsync(responseBody, Encoding.UTF8);       //Send the response
        }

      
    }
}
