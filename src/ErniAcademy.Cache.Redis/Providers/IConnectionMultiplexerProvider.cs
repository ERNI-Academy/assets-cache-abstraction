using StackExchange.Redis;

namespace ErniAcademy.Cache.Redis;

public interface IConnectionMultiplexerProvider
{
    public ConnectionMultiplexer Connection { get; }
}
