using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LemonExam.Model;
using LemonExam.Model.Master;
using LemonExam.Infrastructure;

namespace LemonExam.Infrastructure.Session
{
    public class SessionRepository : ISessionRepository
    {
        #region Private Fields

        private readonly LocalDbContext _context;

        #endregion

        #region Constructor

        public SessionRepository(LocalDbContext context)
        {
            _context = context;
        }

        #endregion
        #region Public Methods

        public List<UserSession> GetSessions()
        {
            return _context.Sessions.ToList();
        }

        public UserSession GetSession(int id)
        {
            return _context.Sessions.First(t => t.ID == id);
        }

        [HttpPost]
        public void Post(UserSession session)
        {
            _context.Sessions.Add(session);
            _context.SaveChanges();
        }

        public void Put(int id, [FromBody]UserSession session)
        {
            _context.Sessions.Update(session);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = _context.Sessions.First(t => t.ID == id);
            _context.Sessions.Remove(entity);
            _context.SaveChanges();
        }

        #endregion
    }
}
