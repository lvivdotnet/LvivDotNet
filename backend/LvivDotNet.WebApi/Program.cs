using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace LvivDotNet
{
    /// <summary>
    /// Entry point.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Entry point.
        /// </summary>
        /// <param name="args"> Arguments. </param>
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Creates WbHostBuilder.
        /// </summary>
        /// <param name="args"> Arguments. </param>
        /// <returns> Web host builder. </returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
