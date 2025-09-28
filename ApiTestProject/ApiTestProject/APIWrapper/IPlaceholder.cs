using RestSharp;

namespace ApiTestProject.APIWrapper
{
    public interface IPlaceholder
    {
        Task<RestResponse<T>> GetPostsAsync<T>(string id);
        Task<RestResponse<T>> GetAllPostsAsync<T>();
        Task<RestResponse<T>> CreatePostsAsync<T>(T Payload);
        Task<RestResponse<T>> UpdatePostsAsync<T>(string id, T Payload);
        Task<RestResponse<T>> DeletePostsAsync<T>(string id);
    }
}
