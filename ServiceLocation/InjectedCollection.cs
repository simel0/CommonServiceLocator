using System.Collections;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceLocation;

[Serializable]
public struct InjectedCollection<T> : IEnumerable<T> where T : class
{
    [NonSerialized] private readonly ServiceCollectionAccessor<T> _accessor;
    [NonSerialized] private IEnumerable<T> _services;

    public InjectedCollection(ServiceCollectionAccessor<T> accessor)
    {
        _services = null;
        _accessor = accessor;
    }

    public InjectedCollection(IEnumerable<T> services)
    {
        _services = services;
        _accessor = null;
    }

    private ServiceCollectionAccessor<T> Accessor => _accessor ??
                                                     ServiceLocator.Current
                                                         .GetServices<T>;

    public IEnumerable<T> Services
    {
        get => _services ?? (_services = Accessor());
        set => _services = value;
    }

    public IEnumerator<T> GetEnumerator()
    {
        return Services.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public static implicit operator InjectedCollection<T>(List<T> services)
    {
        return new InjectedCollection<T>(services);
    }

    public static implicit operator InjectedCollection<T>(T[] services)
    {
        return new InjectedCollection<T>(services);
    }
}