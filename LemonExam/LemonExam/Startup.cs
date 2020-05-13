using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using LemonExam.Infrastructure;
using LemonExam.Infrastructure.ActionFilters;
using LemonExam.Infrastructure.Session;
using LemonExam.Features.Authentication;
using LemonExam.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StructureMap;
using Microsoft.OpenApi.Models;

namespace LemonExam
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;

            var builder = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
            GetConnection.strDbContext = $"{Configuration["ConnectionStrings:strDbContext"]}";

        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            string secretKey = Configuration["JwtIssuerOptions:SigningKey"];
            var _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

            services.AddDistributedMemoryCache();
            services.AddSession(o => { o.Cookie.Name = ".SiNotificationService.Session"; o.IdleTimeout = TimeSpan.FromSeconds(60); });
            services.AddOptions();
            services.AddMvc()
                .AddControllersAsServices()
                .AddJsonOptions(o => o.SerializerSettings.ContractResolver = new DefaultContractResolver());
            services.AddEntityFrameworkSqlServer()
                    .AddDbContext<LocalDbContext>(o => o.UseSqlServer(Configuration.GetConnectionString("strDbContext")));
            services.AddEntityFrameworkSqlServer();
            services.AddScoped<ISessionRepository, SessionRepository>();
            services.AddScoped<IMediator, Mediator>();
            services.AddTransient<SingleInstanceFactory>(sp => t => sp.GetService(t));
            services.AddTransient<MultiInstanceFactory>(sp => t => sp.GetServices(t));
            services.AddMediatorHandlers(typeof(Startup).GetTypeInfo().Assembly);
            services.AddSingleton<ValidateJWTAttribute>();
            services.Configure<JwtIssuerOptions>(Configuration.GetSection(nameof(JwtIssuerOptions)));
            services.Configure<JwtIssuerOptions>(o => o.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256));
            

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    version = "v1",
                    title = "My API Test",
                    description = "Documentation for Lemontech Test"
                });
            });

            var container = new Container(cfg => {
                cfg.Scan(_ => {
                    _.AssemblyContainingType<Startup>(); // Our assembly with requests & handlers
                    _.WithDefaultConventions();
                    _.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<,>));
                    _.ConnectImplementationsToTypesClosing(typeof(IAsyncRequestHandler<,>));
                    _.ConnectImplementationsToTypesClosing(typeof(INotificationHandler<>));
                    _.ConnectImplementationsToTypesClosing(typeof(IAsyncNotificationHandler<>));
                });
                cfg.For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => t => ctx.GetInstance(t));
                cfg.For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => t => ctx.GetAllInstances(t));
                cfg.For<IMediator>().Use<Mediator>();
                cfg.For<IDependencyResolver>().Use<StructureMapDependencyResolver>();
                cfg.For<IHttpContextAccessor>().Use<HttpContextAccessor>().Singleton();

                cfg.Populate(services);
            });

            return container.GetInstance<IServiceProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors("CorsPolicy");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseExceptionHandler("/error");

            }
            app.UseSession();
            app.UseFileServer();
            app.UseStaticFiles();
            app.UseMvc(routes => {
                routes.MapRoute(name: "default", template: "{controller=Account}/{action=Access}/{id?}/{param2?}/{param3?}");
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API Test");
            });
        }

        public IConfiguration Configuration { get; }
    }

    internal class Info : OpenApiInfo
    {
        public object version { get; set; }
        public object title { get; set; }
        public object description { get; set; }
    }
}
