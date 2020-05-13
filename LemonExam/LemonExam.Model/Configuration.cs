using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LemonExam.Model
{
    public class Configuration
    {
        public Guid ConfigurationId { get; set; }
        public int StoreId { get; set; }
        public int TerminalNumber { get; set; }
        public string Section { get; set; }
        public string Parameter { get; set; }
        public int ParameterIndex { get; set; }
        public string Value { get; set; }
        public int ParameterIndex2 { get; set; }
        public int DataType { get; set; }
        public Guid? DeviceId { get; set; }
    }
    public static class GetConnection
    {
        public static string strDbContext { get; set; }
    }
}
