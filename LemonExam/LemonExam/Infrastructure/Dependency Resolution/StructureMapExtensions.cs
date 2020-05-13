using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using StructureMap;
using System;

namespace LemonExam.Infrastructure {
    public static class StructureMapExtensions {
        //public static IServiceCollection AddStructureMap(this IServiceCollection services, Action configure = null) {
        //    return services.AddSingleton > (new StructureMapServiceProviderFactory(configure));
        //}

        //public static IWebHostBuilder UseStructureMap(this IWebHostBuilder builder, Action configure = null) {
        //    return builder.ConfigureServices(services => services.AddStructureMap(configure));
        //}

        //private class StructureMapServiceProviderFactory : IServiceProviderFactory {
        //    public StructureMapServiceProviderFactory(Action configure) {
        //        Configure = configure ?? (config => { });
        //    }

        //    private Action Configure { get; }

        //    public IContainer CreateBuilder(IServiceCollection services) {
        //        var container = new Container(Configure);

        //        container.Populate(services);

        //        return container;
        //    }

        //    public IServiceProvider CreateServiceProvider(IContainer container) {
        //        return container.GetInstance();
        //    }
        //}
    }
}
