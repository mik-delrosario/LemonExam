using System;
using System.Reflection;
using System.Threading.Tasks;

namespace LemonExam.Infrastructure {
    class MediatorPlan<TResult> {

        #region Fields

        readonly MethodInfo HandleMethod;
        readonly Func<object> HandlerInstanceBuilder;

        #endregion

        #region Constructor

        public MediatorPlan(Type handlerTypeTemplate, string handlerMethodName, Type messageType, IDependencyResolver dependencyResolver) {
            var handlerType = handlerTypeTemplate.MakeGenericType(messageType, typeof(TResult));
            HandleMethod = GetHandlerMethod(handlerType, handlerMethodName, messageType);
            HandlerInstanceBuilder = () => dependencyResolver.GetInstance(handlerType);
        }

        #endregion

        MethodInfo GetHandlerMethod(Type handlerType, string handlerMethodName, Type messageType) {
            return handlerType.GetMethod(handlerMethodName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod);
        }

        public TResult Invoke(object message) {
            return (TResult)HandleMethod.Invoke(HandlerInstanceBuilder(), new[] { message });
        }

        public async Task<TResult> InvokeAsync(object message) {
            return await (Task<TResult>)HandleMethod.Invoke(HandlerInstanceBuilder(), new[] { message });
        }
    }
}
