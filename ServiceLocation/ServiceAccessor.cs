namespace ServiceLocation;

public delegate TService ServiceAccessor<out TService>();

public delegate IEnumerable<TService> ServiceCollectionAccessor<out TService>();