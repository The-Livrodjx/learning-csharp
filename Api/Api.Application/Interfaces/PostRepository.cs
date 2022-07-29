using Api.Api.Application.Dtos;
using Refit;

namespace Api.Application.Interfaces
{
    public interface IPostRepository
    {
        [Get("/posts")]
        public Task<PostDto[]> ReturnPosts();

        [Post("/posts")]
        public Task<PostDto> InsertPosts(PostDto post);
    }
}
