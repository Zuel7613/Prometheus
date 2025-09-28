using ApiTestProject.APIWrapper;
using ApiTestProject.Model;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net;

namespace ApiTestProject.TestCases
{
    [Parallelizable(scope: ParallelScope.All)]
    [TestFixture]
    public class TestDelete : BaseTest
    {
        private Client _client;
        private ILogger<TestDelete> _logger;

        [SetUp]
        public void Setup()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfiguration configuration = builder.Build();
            string? baseUrl = configuration["BaseUrl"];
            _logger = ServiceProvider.GetService<ILogger<TestDelete>>();
            var client_logger = ServiceProvider.GetService<ILogger<Client>>();
            _client = new Client(baseUrl, client_logger);
        }

        [TearDown]
        public void TearDown()
        {
            _client?.Dispose();
        }

        [Test]
        public void DeleteFirstPost()
        {
            var response = _client.DeletePostsAsync<Post>("1");
            using (new AssertionScope())
            {
                response.Result.IsSuccessStatusCode.Should().BeTrue();
                response.Result.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        [Test]
        public void DeleteFirstPostTwice()
        {
            var response_one = _client.DeletePostsAsync<Post>("1");
            var response_two = _client.DeletePostsAsync<Post>("1");
            using (new AssertionScope())
            {
                response_one.Result.IsSuccessStatusCode.Should().BeTrue();
                response_one.Result.StatusCode.Should().Be(HttpStatusCode.OK);
                response_two.Result.IsSuccessStatusCode.Should().BeTrue();
                response_two.Result.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        [Test]
        public void DeleteFakePost()
        {
            var response = _client.DeletePostsAsync<Post>("101");
            using (new AssertionScope())
            {
                response.Result.IsSuccessStatusCode.Should().BeTrue();
                response.Result.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }
    }
}
