
using System.Threading;
using System.Threading.Tasks;

namespace LemonExam.Infrastructure {
    public interface IMediator
    {
        Response<TResponse> Request<TResponse>(IRequest<TResponse> query);

        Task<Response<TResponse>> RequestAsync<TResponse>(IAsyncRequest<TResponse> query);

        Result<TResult> Send<TResult>(ICommand<TResult> command);     //Send a command that returns a result

        TResponse Send<TResponse>(IRequest<TResponse> request);         //Send a request to a single handler

        Task<TResponse> SendAsync<TResponse>(IAsyncRequest<TResponse> request); // Asynchronously send a request to a single handler

        Task<TResponse> SendAsync<TResponse>(ICancellableAsyncRequest<TResponse> request, CancellationToken cancellationToken); // Asynchronously send a cancellable request to a single handler

        void Publish(INotification notification);                   //Send a notification to multiple handlers

        Task PublishAsync(IAsyncNotification notification);         // Asynchronously send a notification to multiple handlers

        Task PublishAsync(ICancellableAsyncNotification notification, CancellationToken cancellationToken); // Asynchronously send a cancellable notification to multiple handlers

        //Response Notify<TNotification>(TNotification notification);
        //Task<Response> NotifyAsync<TNotification>(TNotification notification);
    }
}
