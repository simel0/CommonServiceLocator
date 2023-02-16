using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceLocation;

namespace CommonServiceLocator.HostBuilders;

public class ServiceLocatorProviderFactoryFacade<TContainerBuilder> : IServiceProviderFactory<TContainerBuilder>
{
    private readonly IServiceProviderFactory<TContainerBuilder> _defaultInstance;
    private readonly HostBuilderContext _hostBuilderContext;

    public ServiceLocatorProviderFactoryFacade(
        HostBuilderContext hostBuilderContext,
        IServiceProviderFactory<TContainerBuilder> defaultInstance)
    {
        _defaultInstance = defaultInstance;
        _hostBuilderContext = hostBuilderContext;
    }

    public TContainerBuilder CreateBuilder(IServiceCollection services)
    {
        return _defaultInstance.CreateBuilder(services);
    }

    public IServiceProvider CreateServiceProvider(TContainerBuilder containerBuilder)
    {
        var serviceProvider = _defaultInstance.CreateServiceProvider(containerBuilder);
        ServiceLocator.SetServiceProvider(serviceProvider);
        serviceProvider.GetRequiredService<IRootServiceScopeFactory>();
        return serviceProvider;
    }
}