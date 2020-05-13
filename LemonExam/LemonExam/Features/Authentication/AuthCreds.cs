using LemonExam.Features.Account;
using LemonExam.Features.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LemonExam.Authentication
{
    public class AuthCreds
    {

        public AuthCreds(UserInfoResponse result)
        {
            this.UserInfo = result;
        }

        public UserInfoResponse UserInfo { get; set; }

        public string ApiToken { get; set; }
    }
}
