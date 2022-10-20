using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.FeatureFilters;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Common;
using Ninject.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebApplication3.Controllers;

namespace WebApplication3
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();
        private static IConfigurationRefresher _refresher;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            IConfigurationRoot configuration = new ConfigurationBuilder()
               .AddAzureAppConfiguration(options =>
               {
                   options.Connect("Endpoint=https://poc-core.azconfig.io;Id=Lz6A-l6-s0:mMC85TF+9U8kZ4pgmtPE;Secret=D00IZT2F/4xds1SBXrRBkcwigE/QuXdsbslCCFLccA0=")
                           .ConfigureRefresh(c =>
                           {
                               c.Register("cqrs", true).SetCacheExpiration(TimeSpan.FromSeconds(2));
                           })
                          .UseFeatureFlags();
                   _refresher = options.GetRefresher();
               }).Build();

            IServiceCollection services = new ServiceCollection();

            Assembly asm = Assembly.GetExecutingAssembly();
            foreach (Type type in asm.GetTypes())
            {
                if (type.Namespace == "WebApplication3.Controllers" && type.IsPublic)
                    services.AddTransient(type);
            }
            services.AddSingleton<IConfiguration>(configuration).AddFeatureManagement().AddFeatureFilter<ContextualTargetingFilter>();
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            //var defaultResolver = new DefaultDependencyResolver(serviceProvider);
            //DependencyResolver.SetResolver(defaultResolver);
            var kernel = new StandardKernel();
            kernel.Bind<IRepository>().To<Repository>();
            kernel.Bind<IFeatureManager>().ToMethod(c =>
            {
                return serviceProvider.GetService<IFeatureManager>();
            });
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            _ = _refresher.TryRefreshAsync();
        }
        public class DefaultDependencyResolver : IDependencyResolver
        {
            private IServiceProvider serviceProvider;
            public DefaultDependencyResolver(IServiceProvider serviceProvider)
            {
                this.serviceProvider = serviceProvider;
            }

            public object GetService(Type serviceType)
            {
                return this.serviceProvider.GetService(serviceType);
            }

            public IEnumerable<object> GetServices(Type serviceType)
            {
                return this.serviceProvider.GetServices(serviceType);
            }
        }
    }
    public interface IRepository { }
    public class Repository : IRepository { }
}
