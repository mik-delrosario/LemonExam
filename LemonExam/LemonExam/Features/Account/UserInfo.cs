using LemonExam.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LemonExam.Features.Account
{
    public class UserInfo : IRequest<UserInfoResponse>
    {
        public int SecNumber { get; set; }
    }
}
