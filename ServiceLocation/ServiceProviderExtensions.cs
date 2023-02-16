using CommonServiceLocator.ServiceCollections;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ServiceLocation;

public static class ServiceProviderExtensions
{
    private static readonly
        Func<IServiceProvider, IHttpContextFactory, IHttpContextFactory> HttpContextFactoryInterceptor =
            (serviceProvider, defaultHttpContextFactory) =>
                new ServiceLocatorHttpContextFactory(defaultHttpContextFactory);

    public static IServiceScope CreateServiceLocatorScope(
        this IServiceProvider serviceProvider)
    {
        return new ServiceLocatorServiceProviderScope(serviceProvider);
    }

    public static IServiceCollection AddServiceLocator(
        this IServiceCollection services)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.TryIntercept(HttpContextFactoryInterceptor);
        services.TryAddSingleton<IRootServiceScopeFactory, RootServiceScopeFactory>();
        return services;
    }
}