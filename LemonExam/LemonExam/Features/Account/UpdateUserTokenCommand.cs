using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using LemonExam.Infrastructure;
using LemonExam.Model;
using System;
using System.Dynamic;
using System.Linq;
using LemonExam.Model.Master;

namespace LemonExam.Features.Account
{
    public class UpdateUserTokenCommand : ICommandHandler<UpdateUserToken, UpdateUserTokenResponse>
    {
        #region Private Fields

        //private readonly LocalDbContext _core;
        private readonly LocalDbContext _core;
        private readonly DateTime now = DateTime.Now;

        #endregion

        #region Constructor

        public UpdateUserTokenCommand(LocalDbContext dbCore)
        {
            _core = dbCore;
        }

        public UpdateUserTokenResponse Handle(UpdateUserToken cmd)
        {
            var response = new UpdateUserTokenResponse();
            UserAccount user = null;

            try
            {
                user = (from u in _core.UserAccounts
                        where u.SiteName == cmd.User.SiteName && u.SitePassword == cmd.User.SitePassword
                        select u).FirstOrDefault();
                if (user != null)
                {
                    dynamic tknJson = JsonConvert.DeserializeObject<ExpandoObject>(cmd.JWToken);  //Parse the JSON order
                    string token = tknJson.bearer;
                    user.AccessToken = token;
                    user.TokenExpirationDate = cmd.User.TokenExpirationDate;
                    user.LastModifiedBy = "Mik";
                    user.LastModifiedDate = DateTime.Now;
                    response.IsUpdated = (_core.SaveChanges() > 0);
                    response.Message = $"Access token updated for user {user.SiteName}";
                    response.StatusCode = StatusCodes.Status200OK;
                }
            }
            catch (Exception ex)
            {
                response.IsUpdated = false;
                response.StatusCode = StatusCodes.Status500InternalServerError;
                response.Message = ex.Message;
            }

            return response;
        }

        #endregion
    }
}
