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
    public class TestCreate : BaseTest
    {
        private Client _client;
        private ILogger<TestCreate>? _logger;

        [SetUp]
        public void Setup()
        {
            _client = new Client();
            _logger = _client.ServiceProvider.GetService<ILogger<TestCreate>>();
        }

        [TearDown]
        public void TearDown()
        {
            _client?.Dispose();
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
            _logger.LogInformation("The result data id: {post}", response.Result.Data?.Id);
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
            _logger.LogInformation("The result data id: {post}", response.Result.Data?.Id);
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
            _logger.LogInformation("The result data id: {post}", response.Result.Data?.Id);
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
