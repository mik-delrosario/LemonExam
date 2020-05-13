using LemonExam.Infrastructure;
using LemonExam.Model;
using LemonExam.Features.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LemonExam.Model.Master;

namespace LemonExam.Features.Authentication
{
    public class CheckUserAuth : IRequest<CheckUserAuthResponse>
    {
        public UserAccount User { get; set; }
    }
}
