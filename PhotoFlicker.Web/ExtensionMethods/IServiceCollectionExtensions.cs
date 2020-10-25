using Microsoft.Extensions.DependencyInjection;
using PhotoFlicker.Application.Repository.Photo;
using PhotoFlicker.Application.Repository.Tag;
using PhotoFlicker.Application.Service.Photo;
using PhotoFlicker.Application.Service.Tag;

namespace PhotoFlicker.Web.ExtensionMethods
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositoriesWithServices(this IServiceCollection services)
        {
            return services.AddScoped<IPhotoRepository, PhotoRepository>()
                .AddScoped<ITagRepository, TagRepository>()
                .AddScoped<IPhotoService, PhotoService>()
                .AddScoped<ITagService, TagService>();
        }
    }
}