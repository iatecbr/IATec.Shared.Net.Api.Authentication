using Bogus;
using IATec.Shared.Api.Authentication.Extension;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
        services.AddAuthenticationConfiguration(configuration);
        
        var provider = services.BuildServiceProvider();        

        string tokenName = faker.Person.FirstName;
        string tokenValue = faker.Random.Guid().ToString();

        var cache = provider.GetRequiredService<IDistributedCache>();

        await cache.SetStringAsync(tokenName, tokenValue);
        var result = await cache.GetStringAsync(tokenName);

        //Assert
        Assert.Equal(tokenValue, result);
    }
}
