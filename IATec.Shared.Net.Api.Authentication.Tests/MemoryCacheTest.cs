using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using IATec.Shared.Api.Authentication.Services;
using IATec.Shared.Api.Authentication.Extension;
using Microsoft.Extensions.Caching.Distributed;
using Bogus;

namespace IATec.Shared.Net.Api.Authentication.Tests;
public class MemoryCacheTest
{
    [Fact]
    public async Task TokenShouldStoreOnMemoryCache()
    {
        //Arrange
        var faker = new Faker();

        var inMemorySettings = new Dictionary<string, string>
        {
            {"Cache:UseDistributedCache", "true"}
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();

        var services = new ServiceCollection();
        services.AddCacheConfiguration(configuration);
        services.AddScoped<TokenStoreService>();

        var provider = services.BuildServiceProvider();

        var tokenStore = provider.GetRequiredService<TokenStoreService>();

        string tokenName = faker.Person.FirstName;
        string tokenValue = faker.Random.Guid().ToString();

        //Act
        await tokenStore.StoreTokenAsync(tokenName, tokenValue);

        var cache = provider.GetRequiredService<IDistributedCache>();
        var result = await cache.GetStringAsync(tokenName);

        //Assert
        Assert.Equal(tokenValue, result);
    }    
}
