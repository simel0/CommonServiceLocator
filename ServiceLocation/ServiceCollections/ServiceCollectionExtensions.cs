using Microsoft.Extensions.DependencyInjection;

namespace CommonServiceLocator.ServiceCollections;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection TryIntercept<TService>(
        this IServiceCollection services,
        Func<IServiceProvider, TService, TService> interceptorFactory)
        where TService : class
    {
        return services.RegisterInterceptor(interceptorFactory, true);
    }

    public static IServiceCollection Intercept<TService>(
        this IServiceCollection services,
        Func<IServiceProvider, TService, TService> interceptorFactory)
        where TService : class
    {
        return services.RegisterInterceptor(interceptorFactory, false);
    }

    private static IServiceCollection RegisterInterceptor<TService>(
        this IServiceCollection services,
        Func<IServiceProvider, TService, TService> interceptorFactory,
        bool tryAdd)
        where TService : class
    {
        var intList = new List<int>();
        for (var index = 0; index < services.Count; ++index)
        {
            if (services[index].ServiceType == typeof(TService))
            {
                intList.Add(index);
            }
        }

        if (intList.Count == 0)
        {
            throw new InvalidOperationException("There is no service registered for " + typeof(TService).FullName);
        }

        foreach (var index in intList)
        {
            var existing = services[index];
            if (!tryAdd || !(existing is InterceptedServiceDescriptor serviceDescriptor) ||
                serviceDescriptor.Interceptor != interceptorFactory)
            {
                Func<IServiceProvider, object> func = s => interceptorFactory(s.GetRequiredService<IServiceProvider>(),
                    (TService)GetDefaultFactory(existing)(s));
                var type = typeof(InterceptedServiceDescriptor<>).MakeGenericType(typeof(TService));
                services[index] =
                    Activator.CreateInstance(type, existing, func, interceptorFactory) as ServiceDescriptor;
            }
        }

        return services;
    }

    private static Func<IServiceProvider, object> GetDefaultFactory(
        ServiceDescriptor descriptor)
    {
        if (descriptor.ImplementationInstance != null)
        {
            return s => descriptor.ImplementationInstance;
        }

        return descriptor.ImplementationType != null
            ? s => ActivatorUtilities.GetServiceOrCreateInstance(s, descriptor.ImplementationType)
            : s => descriptor.ImplementationFactory(s);
    }
}