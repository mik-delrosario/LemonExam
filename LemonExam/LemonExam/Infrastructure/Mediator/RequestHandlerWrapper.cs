using System.Threading;
using System.Threading.Tasks;

namespace LemonExam.Infrastructure {
    internal abstract class RequestHandlerWrapper<TResult> {
        public abstract TResult Handle(IRequest<TResult> message);
    }

    internal class RequestHandlerWrapper<TCommand, TResult> : RequestHandlerWrapper<TResult> where TCommand : IRequest<TResult> {
        private readonly IRequestHandler<TCommand, TResult> _inner;

        public RequestHandlerWrapper(IRequestHandler<TCommand, TResult> inner) {
            _inner = inner;
        }

        public override TResult Handle(IRequest<TResult> message) {
            return _inner.Handle((TCommand)message);
        }
    }

    internal abstract class AsyncRequestHandlerWrapper<TResult> {
        public abstract Task<TResult> Handle(IAsyncRequest<TResult> message);
    }

    internal class AsyncRequestHandlerWrapper<TCommand, TResult> : AsyncRequestHandlerWrapper<TResult> where TCommand : IAsyncRequest<TResult> {
        private readonly IAsyncRequestHandler<TCommand, TResult> _inner;

        public AsyncRequestHandlerWrapper(IAsyncRequestHandler<TCommand, TResult> inner) {
            _inner = inner;
        }

        public override Task<TResult> Handle(IAsyncRequest<TResult> message) {
            return _inner.Handle((TCommand)message);
        }
    }

    internal abstract class CancellableAsyncRequestHandlerWrapper<TResult> {
        public abstract Task<TResult> Handle(ICancellableAsyncRequest<TResult> message, CancellationToken cancellationToken);
    }

    internal class CancellableAsyncRequestHandlerWrapper<TCommand, TResult> : CancellableAsyncRequestHandlerWrapper<TResult> where TCommand : ICancellableAsyncRequest<TResult> {
        private readonly ICancellableAsyncRequestHandler<TCommand, TResult> _inner;

        public CancellableAsyncRequestHandlerWrapper(ICancellableAsyncRequestHandler<TCommand, TResult> inner) {
            _inner = inner;
        }

        public override Task<TResult> Handle(ICancellableAsyncRequest<TResult> message, CancellationToken cancellationToken) {
            return _inner.Handle((TCommand)message, cancellationToken);
        }
    }
}
