using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace ServiceLocation;

internal class ServiceLocatorHttpContextFactory : IHttpContextFactory
{
    public ServiceLocatorHttpContextFactory(IHttpContextFactory httpContextFactory)
    {
        Default = httpContextFactory;
    }

    protected IHttpContextFactory Default { get; }

    public HttpContext Create(IFeatureCollection featureCollection)
    {
        var httpContext = Default.Create(featureCollection);
        httpContext.Response.OnCompleted(() =>
        {
            ServiceLocator.SetScopedServiceProvider(null);
            return Task.CompletedTask;
        });
        ServiceLocator.SetScopedServiceProvider(httpContext.RequestServices);
        return httpContext;
    }

    public void Dispose(HttpContext httpContext)
    {
        ServiceLocator.SetScopedServiceProvider(null);
        Default.Dispose(httpContext);
    }
}