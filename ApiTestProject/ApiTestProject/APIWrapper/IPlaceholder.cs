using RestSharp;

namespace ApiTestProject.APIWrapper
{
    public interface IPlaceholder
    {
        Task<RestResponse<T>> ExecuteGetAsync<T>(string id);
        Task<RestResponse<T>> ExecuteGetAsync<T>();
        Task<RestResponse<T>> ExecuteCreateAsync<T>(T payload);
        Task<RestResponse<T>> ExecutePutAsync<T>(string id, T payload);
        Task<RestResponse<T>> ExecuteDeleteAsync<T>(string id);
    }
}
