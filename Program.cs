using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace BooksApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls(urls: "http://0.0.0.0:4000")
                .Build();
    }
}
