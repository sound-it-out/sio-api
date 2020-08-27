using System;
using Microsoft.Extensions.DependencyInjection;
using SIO.Infrastructure.AWS;
using SIO.Infrastructure.Extensions;
using SIO.Infrastructure.Files;
using SIO.Testing.Abstractions;
using Xunit;

namespace SIO.Infrastructure.Local.Tests.Files.LocalFileClient
{
    public abstract class LocalFileClientSpecification<TResult> : Specification<TResult>, IClassFixture<FileClientFixture>
    {
        private readonly Lazy<FileClientFixture> _fileClientFixture;
        protected IFileClient FileClient => _fileClientFixture.Value;

        public LocalFileClientSpecification(FileClientFixture fileClientFixture)
        {
            _fileClientFixture = new Lazy<FileClientFixture>(() =>
            {
                fileClientFixture.InitFileClient(_serviceProvider.GetRequiredService<IFileClient>());
                return fileClientFixture;
            });
        }

        protected override void BuildServices(IServiceCollection services)
        {
            base.BuildServices(services);
            services.AddSIOInfrastructure()
                .AddLocalFiles();
        }
    }

    public abstract class LocalFileClientSpecification : Specification, IClassFixture<FileClientFixture>
    {
        private readonly Lazy<FileClientFixture> _fileClientFixture;
        protected IFileClient FileClient => _fileClientFixture.Value;

        public LocalFileClientSpecification(FileClientFixture fileClientFixture)
        {
            _fileClientFixture = new Lazy<FileClientFixture>(() =>
            {
                fileClientFixture.InitFileClient(_serviceProvider.GetRequiredService<IFileClient>());
                return fileClientFixture;
            });
        }

        protected override void BuildServices(IServiceCollection services)
        {
            base.BuildServices(services);
            services.AddSIOInfrastructure()
                .AddLocalFiles();
        }
    }
}
