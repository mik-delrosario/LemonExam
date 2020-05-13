using LemonExam.Model;
using LemonExam.Model.Master;

namespace LemonExam.Features.Account
{
    public class UserInfoResponse
    {
        #region Public Properties

        public UserAccount User { get; set; }

        public int StatusCode { get; set; }

        public string Message { get; set; }

        #endregion
    }
}
