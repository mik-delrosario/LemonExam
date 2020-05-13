using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LemonExam.Features.Account
{
    public class UpdateUserTokenResponse
    {
        public bool IsUpdated { get; set; }

        public int StatusCode { get; set; }

        public string Message { get; set; }
    }
}
