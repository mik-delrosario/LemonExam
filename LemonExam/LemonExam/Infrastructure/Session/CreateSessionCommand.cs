using Microsoft.AspNetCore.Http;
using LemonExam.Shared;
using LemonExam.Infrastructure;
using LemonExam.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LemonExam.Infrastructure.Session
{
    public sealed class CreateSessionCommand : CommandResultHandler<CreateSession, CreateSessionResult>
    {
        #region Private Fields

        private readonly LocalDbContext _core;
        private readonly IHttpContextAccessor _accessor;
        private ISession _session => _accessor.HttpContext.Session;
        private readonly DateTime now = DateTime.Now;

        #endregion

        #region Constructor

        public CreateSessionCommand(IHttpContextAccessor accessor, LocalDbContext dbCore)
        {
            _accessor = accessor;
            _core = dbCore;
        }

        public override CreateSessionResult Handle(CreateSession command)
        {
            var sessionResult = new CreateSessionResult();
            int rowsAffected = 0;

            _session.SetString("LastTask", "Create Session");       //Update LastTask 

            //Populate the UserSession property
            sessionResult.UserSession.ID = _core.Sessions.NextID();
            sessionResult.UserSession.CreateDate = now;
            sessionResult.UserSession.LastModifiedDate = now;
            sessionResult.UserSession.CreatedBy = "System.CreateSessionCommand";
            sessionResult.UserSession.LastModifiedBy = "System.CreateSessionCommand";
            sessionResult.UserSession.SiteName = command.User.SiteName;
            sessionResult.UserSession.AccessToken = command.User.AccessToken;
            sessionResult.UserSession.SitePassword = command.User.SitePassword;
            sessionResult.UserSession.SessionID = _accessor.HttpContext.Session.Id;
            sessionResult.UserSession.SessionEnd = DateTime.MinValue;
            sessionResult.UserSession.IPAddr = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
            sessionResult.UserSession.UserType = (int)UserType.WebUser;

            sessionResult.Session = _session;   //Populate the ISession property
            _core.Sessions.Add(sessionResult.UserSession);
            _core.CurrentSession = sessionResult.UserSession;   //Set current session
            rowsAffected = _core.SaveChanges();     //Write new UserSession record

            return sessionResult;
        }

        #endregion
    }
}
