using RestSharp;

namespace ApiTestProject.APIWrapper
{
    public interface IPlaceholder
    {
        Task<RestResponse<T>> GetPostsAsync<T>(string id);
        Task<RestResponse<T>> GetAllPostsAsync<T>();
        Task<RestResponse<T>> CreatePostsAsync<T>(T payload);
        Task<RestResponse<T>> UpdatePostsAsync<T>(string id, T payload);
        Task<RestResponse<T>> DeletePostsAsync<T>(string id);
    }
}
