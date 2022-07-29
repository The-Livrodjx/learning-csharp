using Api.Api.Application.Dtos;
using Api.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace Api.Api.Application.Controllers
{
    [Route("/posts/")]
    public class PostController : ControllerBase
    {

        [HttpGet("/posts")]
        async public Task<PostDto[]> ReturnPosts([FromServices] IPostRepository repository)
        {
            return await repository.ReturnPosts();
        }

        [HttpPost("/post")]
        async public Task<ActionResult<PostDto>> InsertPosts([FromBody] PostDto request, [FromServices] IPostRepository repository)
        {
            return await repository.InsertPosts(request);
        }
    }
}
