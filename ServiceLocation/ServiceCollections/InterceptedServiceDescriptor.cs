using Microsoft.Extensions.DependencyInjection;

namespace CommonServiceLocator.ServiceCollections;

internal abstract class InterceptedServiceDescriptor : ServiceDescriptor
{
    public InterceptedServiceDescriptor(
        ServiceDescriptor orginalDescriptor,
        Func<IServiceProvider, object> factory)
        : base(orginalDescriptor.ServiceType, factory, orginalDescriptor.Lifetime)
    {
    }

    public abstract object Interceptor { get; }
}

internal class InterceptedServiceDescriptor<T> : InterceptedServiceDescriptor
{
    public InterceptedServiceDescriptor(
        ServiceDescriptor orginalDescriptor,
        Func<IServiceProvider, object> factory,
        Func<IServiceProvider, T, T> interceptor)
        : base(orginalDescriptor, factory)
    {
        Interceptor = interceptor;
    }

    public override object Interceptor { get; }
}