using Microsoft.Extensions.Logging;
using RestSharp;

namespace ApiTestProject.APIWrapper
{
    public class Client : IPlaceholder, IDisposable
    {
        private readonly RestClient _restClient;
        private readonly ILogger<Client> _logger;

        public Client(string? baseUrl, ILogger<Client> logger)
        {
            _logger = logger;
            ArgumentNullException.ThrowIfNull(baseUrl);
            var options = new RestClientOptions(baseUrl);
            _restClient = new RestClient(options);
        }

        public async Task<RestResponse<T>> GetPostsAsync<T>(string id)
        {
            _logger.LogInformation("Sending GET for post of id: {PostId}", id);
            var request = new RestRequest(Endpoints.POSTS_ID, Method.Get)
                .AddUrlSegment("id", id);
            return await _restClient.ExecuteAsync<T>(request);
        }

        public async Task<RestResponse<T>> GetAllPostsAsync<T>()
        {
            _logger.LogInformation("Sending GET for all Posts");
            var request = new RestRequest(Endpoints.POSTS, Method.Get);
            return await _restClient.ExecuteAsync<T>(request);
        }

        public async Task<RestResponse<T>> CreatePostsAsync<T>(T payload)
        {
            _logger.LogInformation("Sending POST for post: {Post}", payload);
            var request = new RestRequest(Endpoints.POSTS, Method.Post)
                .AddBody(payload);
            return await _restClient.ExecuteAsync<T>(request);
        }

        public async Task<RestResponse<T>> UpdatePostsAsync<T>(string id, T payload)
        {
            _logger.LogInformation("Sending PUT posts for id: {PostId} and post: {payload}", id, payload);
            var request = new RestRequest(Endpoints.POSTS_ID, Method.Put)
                .AddUrlSegment("id", id)
                .AddBody(payload);
            return await _restClient.ExecuteAsync<T>(request);
        }

        public async Task<RestResponse<T>> DeletePostsAsync<T>(string id)
        {
            _logger.LogInformation("Sending DELETE post for id: {PostId}", id);
            var request = new RestRequest(Endpoints.POSTS_ID, Method.Delete)
                .AddUrlSegment("id", id);
            return await _restClient.ExecuteAsync<T>(request);
        }
        public void Dispose()
        {
            _restClient?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
