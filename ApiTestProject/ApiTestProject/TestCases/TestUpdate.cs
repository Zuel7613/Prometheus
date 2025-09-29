using ApiTestProject.APIWrapper;
using ApiTestProject.Model;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net;

namespace ApiTestProject.TestCases
{
    [Parallelizable(scope: ParallelScope.Self)]
    [TestFixture]
    public class TestUpdate : BaseTest
    {
        private Client _client;
        private ILogger<TestUpdate>? _logger;

        [SetUp]
        public void Setup()
        {
            _client = new Client();
            _logger = _client.ServiceProvider.GetService<ILogger<TestUpdate>>();
        }

        [TearDown]
        public void TearDown()
        {
            _client?.Dispose();
        }

        [Test]
        public void UpdateFirstPost()
        {
            var post = new Post();
            post.Id = 1;
            post.Title = "Frankenstein";
            post.Body = "A person made from other people";
            post.UserId = 11;
            var response = _client.UpdatePostsAsync<Post>(post.Id.ToString(), post);
            _logger.LogInformation("The result data id: {post}", response.Result.Data?.Id);
            using (new AssertionScope())
            {
                response.Result.IsSuccessStatusCode.Should().BeTrue();
                response.Result.StatusCode.Should().Be(HttpStatusCode.OK);
                response.Result.Data.Should().NotBeNull()
                    .And.BeEquivalentTo(post);
            }
        }

        [Test]
        public void UpdateLastPost()
        {
            var post = new Post();
            post.Id = 100;
            post.Title = "Frankenstein";
            post.Body = "A person made from other people";
            post.UserId = 11;
            var response = _client.UpdatePostsAsync<Post>(post.Id.ToString(), post);
            _logger.LogInformation("The result data id: {post}", response.Result.Data?.Id);
            using (new AssertionScope())
            {
                response.Result.IsSuccessStatusCode.Should().BeTrue();
                response.Result.StatusCode.Should().Be(HttpStatusCode.OK);
                response.Result.Data.Should().NotBeNull()
                    .And.BeEquivalentTo(post);
            }
        }

        [Test]
        public void UpdateFakePost()
        {
            var post = new Post();
            post.Id = 101;
            post.Title = "Frankenstein";
            post.Body = "A person made from other people";
            post.UserId = 11;
            var response = _client.UpdatePostsAsync<Post>(post.Id.ToString(), post);
            _logger.LogInformation("The result StatusCode: {StatusCode}", response.Result.StatusCode);
            using (new AssertionScope())
            {
                response.Result.IsSuccessStatusCode.Should().BeFalse();
                response.Result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
                response.Result.Data.Should().BeNull();
            }
        }
    }
}
