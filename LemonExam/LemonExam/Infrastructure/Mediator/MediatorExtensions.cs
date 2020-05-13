using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace LemonExam.Infrastructure {
    public static class MediatorExtensions {
        public static Result<UnitType> Send(this IMediator mediator, ICommand command) {
            Result<UnitType> result = mediator.Send(command);

            if (result.HasException())
                throw result.Exception;

            return result;
        }

        public static IServiceCollection AddMediatorHandlers(this IServiceCollection services, Assembly assembly) {
            var classTypes = assembly.ExportedTypes.Select(t => t.GetTypeInfo()).Where(t => t.IsClass && !t.IsAbstract);

            foreach (var type in classTypes) {
                var interfaces = type.ImplementedInterfaces.Select(i => i.GetTypeInfo());

                foreach (var handlerType in interfaces.Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))) {
                    services.AddTransient(handlerType.AsType(), type.AsType());
                }

                foreach (var handlerType in interfaces.Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IAsyncRequestHandler<,>))) {
                    services.AddTransient(handlerType.AsType(), type.AsType());
                }

                foreach (var handlerType in interfaces.Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>))) {
                    services.AddTransient(handlerType.AsType(), type.AsType());
                }

                foreach (var handlerType in interfaces.Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandResultHandler<,>))) {
                    services.AddTransient(handlerType.AsType(), type.AsType());
                }
            }

            return services;
        }

        //public static TResponse Send<TResponse>(this IMediator mediator, IQuery<TResponse> query) {
        //    var response = mediator.Request(query);

        //    if (response.HasException())
        //        throw response.Exception;

        //    return response.Data;
        //}
    }
}
