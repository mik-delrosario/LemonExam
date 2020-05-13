using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace LemonExam.Model.Master
{
    [Table("UserAccount")]
    public class UserAccount : BaseEntity
    {
        public string SiteName { get; set; }

        public string SitePassword { get; set; }

        public string AccessToken { get; set; }

        public DateTime TokenExpirationDate { get; set; }
        
        public string SigningKey { get; set; }
    }
}
