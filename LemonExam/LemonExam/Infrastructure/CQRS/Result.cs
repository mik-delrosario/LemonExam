using System;

namespace LemonExam.Infrastructure
{
    public sealed class Result : Result<UnitType> {
        public override UnitType Data {
            get { return UnitType.Default; }
            set { }
        }
    }

    public class Result<TResultData> {
        public virtual TResultData Data { get; set; }

        public virtual Exception Exception { get; set; }

        public virtual bool HasException() { return Exception != null; }
    }
}
