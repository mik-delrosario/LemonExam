using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LemonExam.Features.Authentication
{
    public class CheckUserAuthResponse
    {
        public string JWToken { get; set; }

        public DateTime TokenExpiration { get; set; }

        public int StatusCode { get; set; }

        public string Message { get; set; }
    }
}
