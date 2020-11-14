using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SIO.Testing.Abstractions;
using Xunit;

namespace SIO.API.Tests.Abstractions
{
    public abstract class UnauthenticatedServerSpecification : IAsyncLifetime, IClassFixture<ConfigurationFixture>, IClassFixture<UnauthenticatedAPIWebApplicationFactory>
    {
        protected readonly ConfigurationFixture _configurationFixture;
        protected readonly UnauthenticatedAPIWebApplicationFactory _webApplicationFactory;
        protected IEventSeeder EventSeeder => _webApplicationFactory.Services.GetRequiredService<IEventSeeder>();

        protected abstract Task Given();
        protected abstract Task When();
        protected Exception Exception { get; private set; }
        protected ExceptionMode ExceptionMode { get; set; }
        protected virtual void BuildHost(IWebHostBuilder webHostBuilder) { }

        public UnauthenticatedServerSpecification(ConfigurationFixture configurationFixture, UnauthenticatedAPIWebApplicationFactory webApplicationFactory)
        {
            _configurationFixture = configurationFixture;
            _webApplicationFactory = webApplicationFactory;
            _webApplicationFactory.WithWebHostBuilder(builder => BuildHost(builder));
        }

        public virtual Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        public virtual async Task InitializeAsync()
        {
            await When();

            try
            {
                await Given();
            }
            catch (Exception e)
            {
                if (ExceptionMode == ExceptionMode.Record)
                    Exception = e;
                else
                    throw;
            }
        }
    }

    public abstract class UnauthenticatedServerSpecification<TResult> : IAsyncLifetime, IClassFixture<ConfigurationFixture>, IClassFixture<UnauthenticatedAPIWebApplicationFactory>
    {
        protected readonly ConfigurationFixture _configurationFixture;
        protected readonly UnauthenticatedAPIWebApplicationFactory _webApplicationFactory;
        protected IEventSeeder EventSeeder => _webApplicationFactory.Services.GetRequiredService<IEventSeeder>();

        protected abstract Task<TResult> Given();
        protected abstract Task When();
        protected Exception Exception { get; private set; }
        protected TResult Result { get; private set; }
        protected ExceptionMode ExceptionMode { get; set; }
        protected virtual void BuildHost(IWebHostBuilder webHostBuilder) { }

        public UnauthenticatedServerSpecification(ConfigurationFixture configurationFixture, UnauthenticatedAPIWebApplicationFactory webApplicationFactory)
        {
            _configurationFixture = configurationFixture;
            _webApplicationFactory = webApplicationFactory;
            _webApplicationFactory.WithWebHostBuilder(builder => BuildHost(builder));
        }

        public virtual Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        public virtual async Task InitializeAsync()
        {
            await When();

            try
            {
                Result = await Given();
            }
            catch (Exception e)
            {
                if (ExceptionMode == ExceptionMode.Record)
                    Exception = e;
                else
                    throw;
            }
        }
    }
}
