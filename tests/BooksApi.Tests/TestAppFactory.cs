using BooksApi;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace BooksApi.Tests;

public class TestAppFactory : WebApplicationFactory<Program>
{
    protected override IWebHostBuilder? CreateWebHostBuilder()
    {
        return Microsoft.AspNetCore.WebHost.CreateDefaultBuilder()
            .ConfigureAppConfiguration((ctx, cfg) =>
            {
                cfg.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["BookstoreDatabaseSettings:ConnectionString"] = "mongodb://localhost:27017",
                    ["BookstoreDatabaseSettings:DatabaseName"] = "BookstoreDbTest",
                    ["BookstoreDatabaseSettings:BooksCollectionName"] = "Books",
                });
            })
            .UseStartup<Startup>();
    }
}
