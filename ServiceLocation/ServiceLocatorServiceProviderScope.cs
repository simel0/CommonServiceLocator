using Microsoft.Extensions.DependencyInjection;

namespace ServiceLocation;

internal class ServiceLocatorServiceProviderScope : IServiceScope, IServiceProvider
{
    private readonly IServiceProvider _previous;
    private readonly IServiceScope _scope;
    private readonly IServiceProvider _serviceProvider;
    private IServiceProvider _next;

    internal ServiceLocatorServiceProviderScope(IServiceProvider previousServiceProvider)
    {
        _previous = previousServiceProvider ?? throw new ArgumentNullException(nameof(previousServiceProvider));
        _scope = previousServiceProvider.CreateScope();
        _serviceProvider = _scope.ServiceProvider;
        if (previousServiceProvider is ServiceLocatorServiceProviderScope serviceProviderScope)
        {
            serviceProviderScope.SetNext(this);
        }

        ServiceLocator.SetScopedServiceProvider(this);
    }

    public object? GetService(Type serviceType)
    {
        return _serviceProvider.GetService(serviceType);
    }

    public void Dispose()
    {
        if (_next != null)
        {
            throw new InvalidOperationException(
                "ServiceLocatorServiceProviderScope instances must be disposed in reverse order of their creation.");
        }

        if (_previous is ServiceLocatorServiceProviderScope previous)
        {
            previous.NextDisposed();
        }
        else
        {
            ServiceLocator.SetScopedServiceProvider(null);
        }

        _scope.Dispose();
    }

    public IServiceProvider ServiceProvider => this;

    internal void SetNext(IServiceProvider serviceProvider)
    {
        _next = serviceProvider;
    }

    internal void NextDisposed()
    {
        _next = null;
        ServiceLocator.SetScopedServiceProvider(this);
    }
}