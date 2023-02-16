using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CommonServiceLocator.HostBuilders;

public class CustomServiceProviderFactory : ServiceLocatorProviderFactoryFacade<IServiceCollection>
{
    public CustomServiceProviderFactory(HostBuilderContext hostBuilderContext,
        ServiceProviderOptions serviceProviderOptions)
        : base(hostBuilderContext, new DefaultServiceProviderFactory(serviceProviderOptions))
    {
    }

    public CustomServiceProviderFactory(HostBuilderContext hostBuilderContext)
        : this(hostBuilderContext, new ServiceProviderOptions())
    {
    }

    public CustomServiceProviderFactory(HostBuilderContext hostBuilderContext,
        IServiceProviderFactory<IServiceCollection> defaultInstance) : base(hostBuilderContext, defaultInstance)
    {
    }
}