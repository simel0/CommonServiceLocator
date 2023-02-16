using ServiceLocation;

namespace CommonServiceLocator.Services;

public interface IService1
{
    object GetMetrics();
}

public class Service1 : IService1
{
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly Injected<IHttpContextAccessor> _inject;

    public Service1(IConfiguration configuration, IHttpContextAccessor accessor)
    {
        _configuration = configuration;
        _httpContextAccessor = accessor;
    }

    public object GetMetrics()
    {
        return new
        {
            Direct = _httpContextAccessor.HttpContext.TraceIdentifier,
            ViaLocator = ServiceLocator.Current.GetService<IHttpContextAccessor>().HttpContext.TraceIdentifier,
            Inject = _inject.Service.HttpContext.TraceIdentifier
        };
    }
}