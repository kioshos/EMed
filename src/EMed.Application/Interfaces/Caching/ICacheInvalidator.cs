namespace EMed.Application.Interfaces.Caching;

public interface ICacheInvalidator
{
    Task RemoveByPrefixAsync(string prefix, CancellationToken  cancellationToken = default);
}