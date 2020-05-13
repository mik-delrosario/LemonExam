using Microsoft.AspNetCore.Http;
using LemonExam.Features.Account;
using LemonExam.Infrastructure;
using LemonExam.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiNotificationService.Features.Account
{
    public class UserByStoreIDQuery : IRequestHandler<UserInfo, UserInfoResponse>
    {
        #region Private Fields

        //private readonly LocalDbContext _core;
        private readonly LocalDbContext _core;

        #endregion


        #region Constructor

        public UserByStoreIDQuery(LocalDbContext core)
        {
            _core = core;
        }

        public UserInfoResponse Handle(UserInfo request)
        {
            if (request == null)
                throw new ArgumentNullException("request");
            var response = new UserInfoResponse();

            try
            {
                response.User = _core.UserAccounts
                    .Where(c => c.ID == request.SecNumber)
                    .FirstOrDefault();
                if (response.User != null)
                {
                    response.StatusCode = StatusCodes.Status200OK;
                    response.Message = $"User {response.User.SiteName} is requesting api access";
                }   //End if
                else
                {
                    response.StatusCode = StatusCodes.Status404NotFound;
                    response.Message = "No matching user account on file";
                }
            }   //End try
            catch (Exception ex)
            {
                response.StatusCode = StatusCodes.Status500InternalServerError;
                response.Message = ex.Message;
            }

            return response;
        }

        #endregion
    }
}
