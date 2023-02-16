using Microsoft.Extensions.DependencyInjection;

namespace ServiceLocation;

[Serializable]
public struct Injected<T> where T : class
{
    [NonSerialized] private ServiceAccessor<T> _accessor;
    [NonSerialized] private T _service;

    public Injected(ServiceAccessor<T> accessor)
    {
        _service = default;
        _accessor = accessor;
    }

    public Injected(T service)
    {
        _service = service;
        _accessor = null;
    }

    public ServiceAccessor<T> Accessor
    {
        get
        {
            if (_accessor != null)
            {
                return _accessor;
            }

            if (_service == null)
            {
                return ServiceLocator.Current.GetRequiredService<T>;
            }

            var service = _service;
            _accessor = () => service;
            return _accessor;
        }
        set
        {
            _accessor = value;
            _service = default;
        }
    }

    public T Service
    {
        get => _service ?? (_service = Accessor());
        set
        {
            _service = value;
            _accessor = null;
        }
    }

    public static implicit operator Injected<T>(T service)
    {
        return new Injected<T>(service);
    }
}