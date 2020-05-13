using Microsoft.AspNetCore.Mvc;
using LemonExam.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LemonExam.Model.Master;

namespace LemonExam.Infrastructure.Session
{
    public interface ISessionRepository
    {
        List<UserSession> GetSessions();

        UserSession GetSession(int Id);

        void Post(UserSession session);

        void Put(int Id, [FromBody]UserSession session);

        void Delete(int Id);
    }
}
