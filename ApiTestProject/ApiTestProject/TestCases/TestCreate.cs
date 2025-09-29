using ApiTestProject.APIWrapper;
using ApiTestProject.Model;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace ApiTestProject.TestCases
{
    [Parallelizable(scope: ParallelScope.Self)]
    [TestFixture]
    public class TestCreate : BaseTest
    {
        private ServiceProvider _serviceProvider;
        private Client _client;
        private ILogger<TestCreate> _logger;

        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();
            services.AddLogging(builder =>
            {
                builder.ClearProviders(); // Clear default providers
                builder.SetMinimumLevel(LogLevel.Information);
                builder.AddConsole();
            });

            _serviceProvider = services.BuildServiceProvider();

            string? baseUrl = Configuration["BaseUrl"];
            _logger = _serviceProvider.GetService<ILogger<TestCreate>>();
            var client_logger = _serviceProvider.GetService<ILogger<Client>>();
            _client = new Client(baseUrl, client_logger);
        }

        [TearDown]
        public void TearDown()
        {
            _client?.Dispose();
            _serviceProvider.Dispose();
        }

        [Test]
        public void CreatePost()
        {
            var post = new Post();
            post.Id = 101;
            post.Title = "Frankenstein";
            post.Body = "A person made from other people";
            post.UserId = 11;
            var response = _client.CreatePostsAsync<Post>(post);
            using (new AssertionScope())
            {
                response.Result.IsSuccessStatusCode.Should().BeTrue();
                response.Result.StatusCode.Should().Be(HttpStatusCode.Created);
                response.Result.Data.Should().NotBeNull()
                    .And.BeEquivalentTo(post);
            }
        }

        [Test]
        public void CreatePost_WithoutTitle()
        {
            var post = new Post();
            post.Id = 101;
            post.Body = "A person made from other people";
            post.UserId = 11;
            var response = _client.CreatePostsAsync<Post>(post);
            using (new AssertionScope())
            {
                response.Result.IsSuccessStatusCode.Should().BeTrue();
                response.Result.StatusCode.Should().Be(HttpStatusCode.Created);
                response.Result.Data.Should().NotBeNull()
                    .And.BeEquivalentTo(post);
            }
        }

        [Test]
        public void CreatePost_WithoutBody()
        {
            var post = new BadPost();
            post.Id = 101;
            post.Title = "Frankenstein";
            post.UserId = 11;
            var response = _client.CreatePostsAsync<BadPost>(post);
            _logger.LogInformation("The result data: {post}", response.Result.Data);
            using (new AssertionScope())
            {
                response.Result.IsSuccessStatusCode.Should().BeTrue();
                response.Result.StatusCode.Should().Be(HttpStatusCode.Created);
                response.Result.Data.Should().NotBeNull()
                    .And.BeEquivalentTo(post);
            }
        }
    }
}
