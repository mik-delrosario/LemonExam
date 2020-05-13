using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LemonExam.Infrastructure {

    public class Mediator : IMediator {

        #region Private Fields

        private readonly IDependencyResolver _dependencyResolver;
        private readonly SingleInstanceFactory _singleInstanceFactory;
        private readonly MultiInstanceFactory _multiInstanceFactory;
        private readonly ConcurrentDictionary<Type, Type> _genericHandlerCache;
        private readonly ConcurrentDictionary<Type, Type> _wrapperHandlerCache;

        #endregion

        #region Constructor

        //public Mediator(SingleInstanceFactory singleInstanceFactory, MultiInstanceFactory multiInstanceFactory, IDependencyResolver dependencyResolver)
        //{
        //    _singleInstanceFactory = singleInstanceFactory;
        //    _multiInstanceFactory = multiInstanceFactory;
        //    _dependencyResolver = dependencyResolver;
        //    _genericHandlerCache = new ConcurrentDictionary<Type, Type>();
        //    _wrapperHandlerCache = new ConcurrentDictionary<Type, Type>();
        //}

        public Mediator(SingleInstanceFactory singleInstanceFactory, MultiInstanceFactory multiInstanceFactory, IDependencyResolver dependencyResolver)
        {
            _singleInstanceFactory = singleInstanceFactory;
            _multiInstanceFactory = multiInstanceFactory;
            _dependencyResolver = dependencyResolver;
            _genericHandlerCache = new ConcurrentDictionary<Type, Type>();
            _wrapperHandlerCache = new ConcurrentDictionary<Type, Type>();
        }


        #endregion

        #region Request Methods

        public virtual Response<TResponse> Request<TResponse>(IRequest<TResponse> request) {
            var response = new Response<TResponse>();

            try {
                var plan = new MediatorPlan<TResponse>(typeof(IRequestHandler<,>), "Handle", request.GetType(), _dependencyResolver);
                response.Data = plan.Invoke(request);
            }
            catch (Exception e) {
                response.Exception = e;
            }


     
            return response;
        }

        public async Task<Response<TResponse>> RequestAsync<TResponse>(IAsyncRequest<TResponse> query) {
            var response = new Response<TResponse>();

            try {
                var plan = new MediatorPlan<TResponse>(typeof(IAsyncRequestHandler<,>), "HandleAsync", query.GetType(), _dependencyResolver);

                response.Data = await plan.InvokeAsync(query);
            }
            catch (Exception e) {
                response.Exception = e;
            }

            return response;
        }

        #endregion

        #region Send Methods

        public TResponse Send<TResponse>(IRequest<TResponse> request) {     //Send a Request
            var defaultHandler = GetHandler(request);
            var result = defaultHandler.Handle(request);

            return result;
        }

        public Result<TResult> Send<TResult>(ICommand<TResult> command) {           //Send a Command
            var result = new Result<TResult>();

            try {
                var plan = new MediatorPlan<TResult>(typeof(ICommandHandler<,>), "Handle", command.GetType(), _dependencyResolver);
                result.Data = plan.Invoke(command);
            }
            catch (Exception e) {
                result.Exception = e;
            }

            return result;
        }


        public Task<TResponse> SendAsync<TResponse>(IAsyncRequest<TResponse> request) {
            var defaultHandler = GetHandler(request);

            var result = defaultHandler.Handle(request);

            return result;
        }

        public Task<TResponse> SendAsync<TResponse>(ICancellableAsyncRequest<TResponse> request, CancellationToken cancellationToken) {
            var defaultHandler = GetHandler(request);

            var result = defaultHandler.Handle(request, cancellationToken);

            return result;
        }

        #endregion

        #region Notify Methods

        //public Response Notify<TNotification>(TNotification notification) {
        //    var handlers = _dependencyResolver.GetInstances<INotificationHandler<TNotification>>();

        //    var response = new Response();
        //    List<Exception> exceptions = null;

        //    foreach (var handler in handlers)
        //        try {
        //            handler.Handle(notification);
        //        }
        //        catch (Exception e) {
        //            (exceptions ?? (exceptions = new List<Exception>())).Add(e);
        //        }
        //    if (exceptions != null)
        //        response.Exception = new AggregateException(exceptions);
        //    return response;
        //}

        //public async Task<Response> NotifyAsync<TNotification>(TNotification notification) {
        //    var handlers = _dependencyResolver.GetInstances<IAsyncNotificationHandler<TNotification>>();

        //    return await Task
        //        .WhenAll(handlers.Select(x => notifyAsync(x, notification)))
        //        .ContinueWith(task => {
        //            var exceptions = task.Result.Where(exception => exception != null).ToArray();
        //            var response = new Response();

        //            if (exceptions.Any()) {
        //                response.Exception = new AggregateException(exceptions);
        //            }

        //            return response;
        //        });
        //}

        //static async Task<Exception> notifyAsync<TNotification>(IAsyncNotificationHandler<TNotification> asyncCommandHandler, TNotification message) {
        //    try {
        //        await asyncCommandHandler.HandleAsync(message);
        //    }
        //    catch (Exception e) {
        //        return e;
        //    }

        //    return null;
        //}

        #endregion

        #region Publish Methods

        public void Publish(INotification notification) {
            var notificationHandlers = GetNotificationHandlers(notification);

            foreach (var handler in notificationHandlers) {
                handler.Handle(notification);
            }
        }

        public Task PublishAsync(IAsyncNotification notification) {
            var notificationHandlers = GetNotificationHandlers(notification)
                .Select(handler => handler.Handle(notification))
                .ToArray();

            return Task.WhenAll(notificationHandlers);
        }

        public Task PublishAsync(ICancellableAsyncNotification notification, CancellationToken cancellationToken) {
            var notificationHandlers = GetNotificationHandlers(notification)
                .Select(handler => handler.Handle(notification, cancellationToken))
                .ToArray();

            return Task.WhenAll(notificationHandlers);
        }

        #endregion

        #region Private Handlers

        private RequestHandlerWrapper<TResponse> GetHandler<TResponse>(IRequest<TResponse> request) {
            return GetHandler<RequestHandlerWrapper<TResponse>, TResponse>(request,
                typeof(IRequestHandler<,>),
                typeof(RequestHandlerWrapper<,>));
        }

        private AsyncRequestHandlerWrapper<TResponse> GetHandler<TResponse>(IAsyncRequest<TResponse> request) {
            return GetHandler<AsyncRequestHandlerWrapper<TResponse>, TResponse>(request,
                typeof(IAsyncRequestHandler<,>),
                typeof(AsyncRequestHandlerWrapper<,>));
        }

        private CancellableAsyncRequestHandlerWrapper<TResponse> GetHandler<TResponse>(ICancellableAsyncRequest<TResponse> request) {
            return GetHandler<CancellableAsyncRequestHandlerWrapper<TResponse>, TResponse>(request,
                typeof(ICancellableAsyncRequestHandler<,>),
                typeof(CancellableAsyncRequestHandlerWrapper<,>));
        }

        private TWrapper GetHandler<TWrapper, TResponse>(object request, Type handlerType, Type wrapperType) {
            var requestType = request.GetType();

            var genericHandlerType = _genericHandlerCache.GetOrAdd(requestType, handlerType, (type, root) => root.MakeGenericType(type, typeof(TResponse)));
            var genericWrapperType = _wrapperHandlerCache.GetOrAdd(requestType, wrapperType, (type, root) => root.MakeGenericType(type, typeof(TResponse)));

            var handler = GetHandler(request, genericHandlerType);

            return (TWrapper)Activator.CreateInstance(genericWrapperType, handler);
        }

        private object GetHandler(object request, Type handlerType) {
            try {
                var instance = _singleInstanceFactory(handlerType);
                return instance;
            }
            catch (Exception e) {
                throw BuildException(request, e);
            }
        }

        private IEnumerable<NotificationHandlerWrapper> GetNotificationHandlers(INotification notification) {
            return GetNotificationHandlers<NotificationHandlerWrapper>(notification,
                typeof(INotificationHandler<>),
                typeof(NotificationHandlerWrapper<>));
        }

        private IEnumerable<AsyncNotificationHandlerWrapper> GetNotificationHandlers(IAsyncNotification notification) {
            return GetNotificationHandlers<AsyncNotificationHandlerWrapper>(notification,
                typeof(IAsyncNotificationHandler<>),
                typeof(AsyncNotificationHandlerWrapper<>));
        }

        private IEnumerable<CancellableAsyncNotificationHandlerWrapper> GetNotificationHandlers(ICancellableAsyncNotification notification) {
            return GetNotificationHandlers<CancellableAsyncNotificationHandlerWrapper>(notification,
                typeof(ICancellableAsyncNotificationHandler<>),
                typeof(CancellableAsyncNotificationHandlerWrapper<>));
        }

        private IEnumerable<TWrapper> GetNotificationHandlers<TWrapper>(object notification, Type handlerType, Type wrapperType) {
            var notificationType = notification.GetType();

            var genericHandlerType = _genericHandlerCache.GetOrAdd(notificationType, handlerType, (type, root) => root.MakeGenericType(type));
            var genericWrapperType = _wrapperHandlerCache.GetOrAdd(notificationType, wrapperType, (type, root) => root.MakeGenericType(type));

            return GetNotificationHandlers(notification, genericHandlerType)
                .Select(handler => Activator.CreateInstance(genericWrapperType, handler))
                .Cast<TWrapper>()
                .ToList();
        }

        private IEnumerable<object> GetNotificationHandlers(object notification, Type handlerType) {
            try {
                return _multiInstanceFactory(handlerType);
            }
            catch (Exception e) {
                throw BuildException(notification, e);
            }
        }

        private static InvalidOperationException BuildException(object message, Exception inner) {
            return new InvalidOperationException("Handler was not found for request of type " + message.GetType() + ".\r\nContainer or service locator not configured properly or handlers not registered with your container.", inner);
        }

        #endregion
    }
}
