using LemonExam.Infrastructure;
using System.Threading.Tasks;

namespace LemonExam.Infrastructure {
    public class MediatorPipeline<TRequest, TRresponse> : IRequestHandler<TRequest, TRresponse> where TRequest : IRequest<TRresponse>
    {
        private readonly IRequestHandler<TRequest, TRresponse> _inner;
        private readonly IPreRequestHandler<TRequest>[] _preRequestHandlers;
        private readonly IPostRequestHandler<TRequest, TRresponse>[] _postRequestHandlers;

        public MediatorPipeline(IRequestHandler<TRequest, TRresponse> inner,
                                IPreRequestHandler<TRequest>[] preRequestHandlers,
                                IPostRequestHandler<TRequest, TRresponse>[] postRequestHandlers) {
            _inner = inner;
            _preRequestHandlers = preRequestHandlers;
            _postRequestHandlers = postRequestHandlers;
        }

        public TRresponse Handle(TRequest message) {
            foreach (var preRequestHandler in _preRequestHandlers) {
                preRequestHandler.Handle(message);
            }

            var result = _inner.Handle(message);

            foreach (var postRequestHandler in _postRequestHandlers) {
                postRequestHandler.Handle(message, result);
            }

            return result;
        }
    }
}
