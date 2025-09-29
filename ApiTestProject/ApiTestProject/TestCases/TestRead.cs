using ApiTestProject.APIWrapper;
using ApiTestProject.Model;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace ApiTestProject.TestCases
{
    [Parallelizable(scope: ParallelScope.Self)]
    [TestFixture]
    public class TestRead : BaseTest
    {
        private Client _client;
        private ILogger<TestRead>? _logger;
        [SetUp]
        public void Setup()
        {
            _client = new Client();
            _logger = _client.ServiceProvider.GetService<ILogger<TestRead>>();
        }

        [TearDown]
        public void TearDown()
        {
            _client?.Dispose();
        }

        [Test, TestCaseSource(nameof(BoundaryTestValues))]
        public void GetPost_BoundaryValue(Post post)
        {
            _logger.LogInformation("Starting Test");
            var response = _client.GetPostsAsync<Post>(post.Id.ToString());
            _logger.LogInformation("Response Data: {PostId}", response.Result.Data?.Id);
            using (new AssertionScope())
            {
                response.Result.IsSuccessStatusCode.Should().BeTrue();
                response.Result.StatusCode.Should().Be(HttpStatusCode.OK);
                response.Result.Data.Should().NotBeNull()
                    .And.BeEquivalentTo(post);
            }
        }

        [Test]
        public void GetPost_EntryDoesNotExist()
        {
            var response = _client.GetPostsAsync<Post>("101");
            using (new AssertionScope())
            {
                response.Result.IsSuccessStatusCode.Should().BeFalse();
                response.Result.StatusCode.Should().Be(HttpStatusCode.NotFound);
            }
        }

        [Test]
        public void GetAllPosts()
        {
            var response = _client.GetAllPostsAsync<List<Post>>();
            _logger.LogInformation("The result StatusCode: {StatusCode}", response.Result.StatusCode);
            using (new AssertionScope())
            {
                response.Result.IsSuccessStatusCode.Should().BeTrue();
                response.Result.StatusCode.Should().Be(HttpStatusCode.OK);
            }

            string jsonString = File.ReadAllText("TestCases/testdata.json");
            List<Post>? postList = JsonConvert.DeserializeObject<List<Post>>(jsonString);
            response.Result.Data.Should().NotBeEmpty()
                .And.HaveCount(100)
                .And.ContainItemsAssignableTo<Post>()
                .And.BeEquivalentTo(postList);
        }

        private static IEnumerable<TestCaseData> BoundaryTestValues()
        {
            string jsonString = File.ReadAllText("TestCases/testdata.json");
            List<Post>? postList = JsonConvert.DeserializeObject<List<Post>>(jsonString);

            var firstPost = postList.FirstOrDefault();
            yield return new TestCaseData(firstPost).
                SetName($"Check that first Post yields {firstPost.Title}");
            var lastPost = postList.LastOrDefault();
            yield return new TestCaseData(lastPost).
                SetName($"Check that last Post yields {lastPost.Title}");
        }
    }
}
