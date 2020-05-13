using LemonExam.Infrastructure;
using LemonExam.Features.Account;
using System;
using LemonExam.Model.Master;

namespace LemonExam.Features.Account
{
    public class UpdateUserToken : ICommand<UpdateUserTokenResponse>
    {
        public UserAccount User { get; set; }

        public string JWToken { get; set; }

        public DateTime TokenExpiration { get; set; }
    }
}
