using Refit;
using Microsoft.OpenApi.Models;
using Api.Application.Interfaces;

namespace Api
{
    public class Startup
    {

        public void ConfigureServices(IServiceCollection services)
        {
     
            services.AddControllers();
       
            services.AddRefitClient<IPostRepository>().ConfigureHttpClient(c => {
                c.BaseAddress = new Uri("https://jsonplaceholder.typicode.com");
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RefitExample", Version = "v1" });
            });

        }
    }
}
