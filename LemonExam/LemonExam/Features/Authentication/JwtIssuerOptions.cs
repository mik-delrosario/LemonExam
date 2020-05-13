using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LemonExam.Features.Authentication
{
    public class JwtIssuerOptions
    {
        #region Constructor

        public JwtIssuerOptions() { }

        #endregion

        #region Public Properties

        public int ID { get; set; }      

        public string SiteName { get; set; }       

        public string SitePassword { get; set; }    

        public string Issuer { get; set; }         

        public string Subject { get; set; }         

        public string Audience { get; set; }       

        public DateTime NotBefore { get; set; }     

        public DateTime IssuedAt { get; set; } = DateTime.UtcNow;

        public TimeSpan ValidFor { get; set; } = TimeSpan.FromMinutes(10);  

        public DateTime Expiration => IssuedAt.Add(ValidFor);

        public Func<Task<string>> NonceGenerator { get; set; }      
            = new Func<Task<string>>(() => Task.FromResult(Guid.NewGuid().ToString()));

        public SigningCredentials SigningCredentials { get; set; }      

        #endregion
    }
}
