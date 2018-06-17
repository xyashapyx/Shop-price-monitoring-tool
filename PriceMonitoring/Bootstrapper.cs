using System.Web.Http;
using System.Web.Mvc;
using PriceMonitoring.Inerfaces.Repository;
using PriceMonitoring.Inerfaces.Services;
using PriceMonitoring.Repository;
using PriceMonitoring.Services;
using Unity;
using Unity.Lifetime;

namespace PriceMonitoring
{
    public class Bootstrapper
    {
        public static IUnityContainer Initialise()
        {
            var container = BuildUnityContainer();
            DependencyResolver.SetResolver(new Unity.Mvc5.UnityDependencyResolver(container));

            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
            return container;
        }
        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        }
        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IProductRepository, ProductRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IImageRepository, ImageRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IPriceRepository, PriceRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IInformationImportService, InformationImportService>(new HierarchicalLifetimeManager());
            container.RegisterType<IDataReader, DataReader>(new HierarchicalLifetimeManager());
        }
    }
}