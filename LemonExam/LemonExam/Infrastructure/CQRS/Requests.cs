
using System.Threading;
using System.Threading.Tasks;

namespace LemonExam.Infrastructure
{
    #region IRequest Interfaces

    public interface IRequest : IRequest<Unit> { }      // Marker interface to represent a request with a void response

    public interface IRequest<out TResponse> { }        // Marker interface to represent a request with a response

    public interface IAsyncRequest : IAsyncRequest<Unit> { }    // Marker interface to represent an asynchronous request with a void response

    public interface IAsyncRequest<out TResponse> { }       // Marker interface to represent a request with a response

    public interface ICancellableAsyncRequest : ICancellableAsyncRequest<Unit> { }      // Marker interface to represent a cancellable, asynchronous request with a void response

    public interface ICancellableAsyncRequest<out TResponse> { }        // Marker interface to represent a cancellable, asynchronous request with a response

    #endregion

    #region IRequestHandler Interfaces

    public interface IRequestHandler<in TRequest, out TResponse> where TRequest : IRequest<TResponse>
    {     // Defines a handler for a request
        TResponse Handle(TRequest request);         // Handles a request
    }

    public interface IAsyncRequestHandler<in TRequest, TResponse> where TRequest : IAsyncRequest<TResponse>
    {       // Handles an asynchronous request
        Task<TResponse> Handle(TRequest request);
    }

    public interface ICancellableAsyncRequestHandler<in TRequest, TResponse> where TRequest : ICancellableAsyncRequest<TResponse>
    { // Defines a cancellable, asynchronous handler for a request
        Task<TResponse> Handle(TRequest message, CancellationToken cancellationToken);      // Handles a cancellable, asynchronous request
    }

    public abstract class CancellableAsyncRequestHandler<TMessage> : ICancellableAsyncRequestHandler<TMessage, Unit>        // Helper class for cancellable, asynchronous requests that return a void response
        where TMessage : ICancellableAsyncRequest
    {
        public async Task<Unit> Handle(TMessage message, CancellationToken cancellationToken)
        {
            await HandleCore(message, cancellationToken);

            return Unit.Value;
        }

        protected abstract Task HandleCore(TMessage message, CancellationToken cancellationToken);      // Handles a void request
    }

    #endregion

    #region RequestHandler Methods

    public abstract class RequestHandler<TRequest> : IRequestHandler<TRequest, Unit> where TRequest : IRequest
    {    // Helper class for requests that return a void response
        public Unit Handle(TRequest request)
        {
            HandleCore(request);

            return Unit.Value;
        }

        protected abstract void HandleCore(TRequest request);       // Handles a void request
    }

    #endregion
}
