using System.Reflection;

namespace ServiceLocation;

public static class ServiceLocator
{
    private static readonly AsyncLocal<ServiceProviderKeeper> _serviceProviderKepper = new();
    internal static IServiceProvider Empty = new EmptyServiceProvider();
    private static IServiceProvider _rootServiceProvider = Empty;

    public static IServiceProvider Current => _serviceProviderKepper?.Value?.ServiceProvider ?? _rootServiceProvider;

    public static void SetServiceProvider(IServiceProvider serviceProvider)
    {
        _rootServiceProvider = serviceProvider;
    }

    public static void SetScopedServiceProvider(IServiceProvider serviceProvider)
    {
        if (_serviceProviderKepper.Value == null)
        {
            if (serviceProvider == null)
            {
                return;
            }

            _serviceProviderKepper.Value = new ServiceProviderKeeper
            {
                ServiceProvider = serviceProvider
            };
        }
        else
        {
            _serviceProviderKepper.Value.ServiceProvider = serviceProvider;
            if (serviceProvider != null)
            {
                return;
            }

            _serviceProviderKepper.Value = null;
        }
    }

    private class ServiceProviderKeeper
    {
        public IServiceProvider ServiceProvider { get; set; }
    }

    private class EmptyServiceProvider : IServiceProvider
    {
        private readonly Lazy<MethodInfo> _emptyFactory = new(() =>
            typeof(Enumerable).GetMethod("Empty", BindingFlags.Static | BindingFlags.Public));

        public object GetService(Type serviceType)
        {
            if (!serviceType.IsGenericType || !(typeof(IEnumerable<>) == serviceType.GetGenericTypeDefinition()))
            {
                return null;
            }

            return _emptyFactory.Value.MakeGenericMethod(serviceType.GetGenericArguments()[0]).Invoke(null, null);
        }
    }
}