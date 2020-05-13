using Microsoft.AspNetCore.Http;
using LemonExam.Infrastructure;
using LemonExam.Model;
using LemonExam.Model.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LemonExam.Infrastructure.Session
{
    public class CreateSessionResult : ICommand
    {
        #region Constructor

        public CreateSessionResult() { }

        #endregion

        #region Public Properties

        public UserSession UserSession { get; set; }

        public ISession Session { get; set; }

        #endregion

    }
}
