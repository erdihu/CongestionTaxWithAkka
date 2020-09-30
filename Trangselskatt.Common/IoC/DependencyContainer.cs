
using StructureMap;
using Trangselskatt.Common.Business.Providers;
using Trangselskatt.Common.Business.Services;
using Trangselskatt.Common.Contracts;

namespace Trangselskatt.Common.IoC
{
    public class DependencyContainer
    {
        public static IContainer Build()
        {
            var container = new Container(cfg =>
            {
                cfg.For<IDateTimeProvider>().Use<LocalDateTimeProvider>();
                cfg.For<IRedDayProvider>().Use<RedDayProviderFor2020>().Singleton();
                cfg.For<ITrangselskattCalculator>().Use<GoteborgTrangselskattCalculator>();
                cfg.For<IPassagePriceProvider>().Use<PassagePriceProvider>();
            });

            return container;

        }
    }
}
