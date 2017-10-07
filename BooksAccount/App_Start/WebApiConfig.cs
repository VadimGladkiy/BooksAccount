using System;
using System.Web.Http;
using Microsoft.Practices.Unity;
using BooksAccount.Infrastructure;

namespace BooksAccount
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var container = new UnityContainer();
            container.RegisterType<IDataProvider, 
                DataContext>(new HierarchicalLifetimeManager());
            config.DependencyResolver = new Util.UnityResolver(container);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "HomeApi",
                routeTemplate: "api/{controller}/{cPage}/{howSkip}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
