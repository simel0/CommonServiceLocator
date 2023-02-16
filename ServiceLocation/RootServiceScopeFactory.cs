using Microsoft.Extensions.DependencyInjection;

namespace ServiceLocation;

public interface IRootServiceScopeFactory
{
    IServiceScope CreateRootScope();
}

internal class RootServiceScopeFactory : IRootServiceScopeFactory
{
    private readonly IServiceProvider _rootServiceProvider;

    public RootServiceScopeFactory(IServiceProvider serviceProvider)
    {
        _rootServiceProvider = serviceProvider;
    }

    public IServiceScope CreateRootScope()
    {
        return _rootServiceProvider.CreateServiceLocatorScope();
    }
}