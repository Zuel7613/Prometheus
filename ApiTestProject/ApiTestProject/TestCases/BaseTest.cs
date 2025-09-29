using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ApiTestProject.TestCases
{
    [TestFixture]
    public abstract class BaseTest
    {
        protected IConfiguration Configuration { get; private set; }

        [OneTimeSetUp]
        public virtual void OneTimeSetup()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }

        [OneTimeTearDown]
        public virtual void OneTimeTeardown()
        {
        }
    }
}
