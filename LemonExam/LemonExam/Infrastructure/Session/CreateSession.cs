using LemonExam.Infrastructure;
using LemonExam.Model.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LemonExam.Infrastructure.Session
{
    public class CreateSession : ICommand<CreateSessionResult>
    {
        public UserAccount User { get; set; }
    }
}
