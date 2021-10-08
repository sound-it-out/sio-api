using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SIO.Api.Extensions;
using SIO.AWS.Extensions;
using SIO.Domain.Extensions;
using SIO.Google.Extensions;
using SIO.Redis.Extensions;

namespace SIO.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }        

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddInfrastructure(_configuration)
                .AddAuthentication(_configuration)
                .AddOpenApi(_configuration)
                .AddDomain()
                //.AddRedis(o => o.ConnectionString = _configuration.GetConnectionString("Redis"))
                .AddGoogleTranslations(_configuration)
                .AddAWSTranslations();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            if (!env.IsDevelopment())
                app.UseHttpsRedirection();

            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials());

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseOpenApi();
        }
    }
}
