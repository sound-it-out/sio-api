using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SIO.Api.Extensions;
using SIO.Domain.Extensions;

namespace SIO.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            if(configuration == null)
                throw new ArgumentNullException(nameof(configuration));
            if(env == null)
                throw new ArgumentNullException(nameof(env));

            _configuration = configuration;
            _env = env;
        }        

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddInfrastructure(_configuration)
                .AddAuthentication(_configuration, _env)
                .AddOpenApi(_configuration)
                .AddDomain();
                //.AddRedis(o => o.ConnectionString = _configuration.GetConnectionString("Redis"))
        }

        public void Configure(IApplicationBuilder app)
        {
            if (_env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            if (!_env.IsDevelopment())
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
