using ApiTestProject.APIWrapper;
using ApiTestProject.Model;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace ApiTestProject.TestCases
{
    [Parallelizable(scope: ParallelScope.Self)]
    [TestFixture]
    public class TestDelete : BaseTest
    {
        private Client _client;
        private ILogger<TestDelete>? _logger;

        [SetUp]
        public void Setup()
        {
            _client = new Client();
            _logger = _client.ServiceProvider.GetService<ILogger<TestDelete>>();
        }

        [TearDown]
        public void TearDown()
        {
           _client?.Dispose();
        }

        [Test]
        public void DeleteFirstPost()
        {
            var response = _client.ExecuteDeleteAsync<Post>("1");
            _logger.LogInformation("The result data id: {post}", response.Result.Data?.Id);
            using (new AssertionScope())
            {
                response.Result.IsSuccessStatusCode.Should().BeTrue();
                response.Result.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        [Test]
        public void DeleteFirstPostTwice()
        {
            var response_one = _client.ExecuteDeleteAsync<Post>("1");
            _logger.LogInformation("The result data id: {post}", response_one.Result.Data?.Id);
            var response_two = _client.ExecuteDeleteAsync<Post>("1");
            _logger.LogInformation("The result data id: {post}", response_two.Result.Data?.Id);
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
            var response = _client.ExecuteDeleteAsync<Post>("101");
            using (new AssertionScope())
            {
                response.Result.IsSuccessStatusCode.Should().BeTrue();
                response.Result.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }
    }
}
