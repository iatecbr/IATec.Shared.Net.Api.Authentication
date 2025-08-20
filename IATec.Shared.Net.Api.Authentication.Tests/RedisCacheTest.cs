using Bogus;
using IATec.Shared.Api.Authentication.Extension;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.Redis;

namespace IATec.Shared.Net.Api.Authentication.Tests;
public class CacheTokenTest : IAsyncLifetime
{
    private readonly RedisContainer _redisContainer;


    public CacheTokenTest()
    {
        var redisImage = Environment.GetEnvironmentVariable("REDIS_TEST_IMAGE") ?? "redis:7.2-alpine";

        _redisContainer = new RedisBuilder()
            .WithImage(redisImage)
            .WithCleanUp(true)
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _redisContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _redisContainer.DisposeAsync();
    }


    [Fact]
    public async Task TokenShouldStoreOnRedisCache()
    {
        //Arrange 
        var faker = new Faker();

        var redisConnection = $"{_redisContainer.Hostname}:{_redisContainer.GetMappedPublicPort(6379)}";

        var inMemorySettings = new Dictionary<string, string>
        {
            {"Cache:UseDistributedCache", "false"},
            {"Cache:RedisConnection", redisConnection}
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();

        var services = new ServiceCollection();
        
        services.AddAuthenticationConfiguration(configuration);

        var provider = services.BuildServiceProvider();

        var tokenName = faker.Person.FirstName;
        var tokenValue = faker.Random.Guid().ToString();

        //Act    
        var cache = provider.GetRequiredService<IDistributedCache>();

        var result = await cache.GetStringAsync(tokenName);

        //Assert
        Assert.Equal(tokenValue, result);
    }
}
