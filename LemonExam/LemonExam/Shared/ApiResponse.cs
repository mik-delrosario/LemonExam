using Newtonsoft.Json;
using System.Reflection;
using System.Runtime.Serialization;

namespace LemonExam.Shared {
    [DataContract]
    public class ApiResponse
    {
        #region Constructor

        public ApiResponse () : this(null, 0, "")
            { }

        public ApiResponse(object result, int statusCode, string message = null) {
            Result = result;
            StatusCode = statusCode;
            Message = message;
        }

        #endregion

        #region Public Properties

        [JsonProperty(PropertyName = "version")]
        public string Version { get { return Assembly.GetEntryAssembly().GetName().Version.ToString(); } }

        [JsonProperty(PropertyName = "status-code")]
        public int StatusCode { get; set; }

        [JsonProperty(PropertyName = "result")]
        public object Result { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        #endregion
    }
}
