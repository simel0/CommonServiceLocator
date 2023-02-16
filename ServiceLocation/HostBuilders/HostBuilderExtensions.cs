using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CommonServiceLocator.HostBuilders;

public static class HostBuilderExtensions
{
    public static IHostBuilder ConfigureCustomBuilder(this IHostBuilder hostBuilder)
    {
        hostBuilder.UseServiceProviderFactory(
            (Func<HostBuilderContext, IServiceProviderFactory<IServiceCollection>>)(context =>
                new CustomServiceProviderFactory(context)));
        return hostBuilder;
    }
}