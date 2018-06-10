using System.Web.Mvc;
using PriceMonitoring.Inerfaces.Repository;
using PriceMonitoring.Inerfaces.Services;
using PriceMonitoring.Repository;
using PriceMonitoring.Services;
using Unity;
using Unity.AspNet.Mvc;
using Unity.Lifetime;

namespace PriceMonitoring
{
    public class Bootstrapper
    {
        public static IUnityContainer Initialise()
        {
            var container = BuildUnityContainer();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
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
            container.RegisterType<IProductRepository, ProductRepository>(new SingletonLifetimeManager());
            container.RegisterType<IImageRepository, ImageRepository>(new SingletonLifetimeManager());
            container.RegisterType<IPriceRepository, PriceRepository>(new SingletonLifetimeManager());
            container.RegisterType<IInformationImportService, InformationImportService>(new SingletonLifetimeManager());
        }
    }
}