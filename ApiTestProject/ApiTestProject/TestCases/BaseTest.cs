using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ApiTestProject.TestCases
{
    [TestFixture]
    public abstract class BaseTest
    {
        protected static ServiceProvider ServiceProvider { get; private set; }
        protected static IConfiguration Configuration { get; private set; }

        [OneTimeSetUp]
        public virtual void OneTimeSetup()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var services = new ServiceCollection();
            services.AddLogging(builder =>
            {
                builder.ClearProviders(); // Clear default providers
                builder.SetMinimumLevel(LogLevel.Information);
                builder.AddConsole();
            });

            ServiceProvider = services.BuildServiceProvider();
        }

        [OneTimeTearDown]
        public virtual void OneTimeTeardown()
        {
            ServiceProvider.Dispose();
        }
    }
}
