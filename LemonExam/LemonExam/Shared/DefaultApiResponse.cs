using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LemonExam.Shared
{
    public static class DefaultApiResponse {

        public static string Create(object content, int statusCode, string message) {
            ApiResponse apiResponse = null;
            string result = string.Empty;

            try {
                result = JsonConvert.SerializeObject(content);
                apiResponse = new ApiResponse(result, statusCode, message);
                result = JsonConvert.SerializeObject(apiResponse);
            }
            catch (Exception ex) {
                string msg = ex.Message;
            }

            return result;
        }

        public static string Create<T>(T content, int statusCode, string message) {
            ApiResponse apiResponse = null;
            string result = string.Empty;

            try {
                result = JsonConvert.SerializeObject(content);
                apiResponse = new ApiResponse(result, statusCode, message);
                result = JsonConvert.SerializeObject(apiResponse);
            }
            catch (Exception ex) {
                string msg = ex.Message;
            }

            return result;
        }

      
        public static string CreateList<T>(List<T> content, int statusCode, string message) {
            ApiResponse apiResponse = null;
            string result = string.Empty;

            try {
                result = JsonConvert.SerializeObject(content);
                apiResponse = new ApiResponse(result, statusCode, message);
                result = JsonConvert.SerializeObject(apiResponse);
            }
            catch (Exception ex) {
                string msg = ex.Message;
            }

            return result;
        }
    }
}
