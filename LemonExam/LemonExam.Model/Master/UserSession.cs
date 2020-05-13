using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LemonExam.Model.Master
{
    public class UserSession : BaseEntity
    {
        public string SessionID { get; set; }
        public string IPAddr { get; set; }
        public string SiteName { get; set; }
        public string SitePassword { get; set; }
        public string AccessToken { get; set; }
        public DateTime SessionStart { get; set; }
        public DateTime SessionEnd { get; set; }
        public int UserType { get; set; }
        public int AuthLevel { get; set; }
    }
}
