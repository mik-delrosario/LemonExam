using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LemonExam.Model;
using LemonExam.Model.ViewModel;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.Runtime.InteropServices;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Diagnostics;

namespace LemonExam.Services
{
    public class DefaultServices : IDefaultServices
    {
        protected readonly LocalDbContext _dbContext;
        private readonly IConfiguration _config;

        public DefaultServices(LocalDbContext _dbContext, IConfiguration _config)
        {
            this._dbContext = _dbContext;
            this._config = _config;
        }

        public AccessObjectViewModel GetToken()
        {
            var jsonObject = new AccessObjectViewModel();
            using (var client = new System.Net.Http.HttpClient())
            {
                var storeId = _config["SiCloud:StoreId"];
                var baseUrl = _config["SiCloud:ApiUrlBaseAddress"].ToString();
                var ApiUrlAccess = _config["SiCloud:ApiUrlAccess"].ToString();
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync(ApiUrlAccess + storeId).Result;
                if (response.IsSuccessStatusCode)
                {
                    using (HttpContent content = response.Content)
                    {
                        Task<string> result = content.ReadAsStringAsync();
                        var jsonObj = JObject.Parse(result.Result);
                        string version = (string)jsonObj["version"];
                        string _result = (string)jsonObj["result"];
                        jsonObject = JsonConvert.DeserializeObject<AccessObjectViewModel>(_result);

                    }
                }
                else
                {
                    throw new Exception("GetTokenError");
                }

            }
            return jsonObject;
        }
        
        private string AddStyleSeat(string value)
        {
            return "ÄÄÄÄÄ[" + value + "]ÄÄÄÄÄÄÄ";
        }
        
        private static Guid CreateGuid()
        {
            const int RPC_S_OK = 0;

            Guid guid;
            int result = NativeMethods.UuidCreateSequential(out guid);
            if (result == RPC_S_OK)
                return guid;
            else
                return Guid.NewGuid();
        }
        private class NativeMethods
        {
            [DllImport("rpcrt4.dll", SetLastError = true)]
            public static extern int UuidCreateSequential(out Guid guid);
        }
        
        private static List<int> SequenceNumber(int number)
        {
            int decimalNumber = number;
            int remainder;
            string result = string.Empty;
            while (decimalNumber > 0)
            {
                remainder = decimalNumber % 2;
                decimalNumber /= 2;
                result = remainder.ToString() + result;
            }
            return result.Reverse().Select((c, i) => (c - '0') * (1 << i)).ToList();
        }
        
        public int GetLineNumberERROR(Exception ex)
        {
            var lineNumber = 0;
            const string lineSearch = ":line ";
            var index = ex.StackTrace.LastIndexOf(lineSearch);
            if (index != -1)
            {
                var lineNumberText = ex.StackTrace.Substring(index + lineSearch.Length);
                if (int.TryParse(lineNumberText, out lineNumber))
                {
                }
            }
            return lineNumber;
        }
        
    }
}
