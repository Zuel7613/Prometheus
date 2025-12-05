using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace ApiTestProject.APIWrapper
{
    public class Client : IPlaceholder, IDisposable
    {
        private readonly RestClient _restClient;
        private readonly ILogger<Client>? _logger;
        private readonly IConfiguration _configuration;
        private ServiceProvider _serviceProvider;

        public ServiceProvider ServiceProvider => _serviceProvider;

        public Client()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var services = new ServiceCollection();
            services.AddLogging(builder =>
            {
                builder.ClearProviders(); // Clear default providers
                builder.SetMinimumLevel(LogLevel.Information);
                builder.AddConsole();
            });

            _serviceProvider = services.BuildServiceProvider();

            _logger = _serviceProvider.GetService<ILogger<Client>>();
            string? baseUrl = _configuration["BaseUrl"];
            ArgumentNullException.ThrowIfNull(baseUrl);
            var options = new RestClientOptions(baseUrl);
            _restClient = new RestClient(options);
        }

        internal static Client Create()
        {
            return new Client();
        }

        public async Task<RestResponse<T>> ExecuteGetAsync<T>(string id)
        {
            _logger.LogInformation("Sending GET for post of id: {PostId}", id);
            var request = new RestRequest(Endpoints.POSTS_ID, Method.Get)
                .AddUrlSegment("id", id);
            return await _restClient.ExecuteAsync<T>(request);
        }

        public async Task<RestResponse<T>> ExecuteGetAsync<T>()
        {
            _logger.LogInformation("Sending GET for all Posts");
            var request = new RestRequest(Endpoints.POSTS, Method.Get);
            return await _restClient.ExecuteAsync<T>(request);
        }

        public async Task<RestResponse<T>> ExecuteCreateAsync<T>(T payload)
        {
            _logger.LogInformation("Sending POST for post: {Post}", payload);
            var request = new RestRequest(Endpoints.POSTS, Method.Post)
                .AddBody(payload);
            return await _restClient.ExecuteAsync<T>(request);
        }

        public async Task<RestResponse<T>> ExecutePutAsync<T>(string id, T payload)
        {
            _logger.LogInformation("Sending PUT posts for id: {PostId} and post: {payload}", id, payload);
            var request = new RestRequest(Endpoints.POSTS_ID, Method.Put)
                .AddUrlSegment("id", id)
                .AddBody(payload);
            return await _restClient.ExecuteAsync<T>(request);
        }

        public async Task<RestResponse<T>> ExecuteDeleteAsync<T>(string id)
        {
            _logger.LogInformation("Sending DELETE post for id: {PostId}", id);
            var request = new RestRequest(Endpoints.POSTS_ID, Method.Delete)
                .AddUrlSegment("id", id);
            return await _restClient.ExecuteAsync<T>(request);
        }
        public void Dispose()
        {
            _logger.LogInformation("Disposing RestClient and ServiceProider");
            _restClient?.Dispose();
            _serviceProvider?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
