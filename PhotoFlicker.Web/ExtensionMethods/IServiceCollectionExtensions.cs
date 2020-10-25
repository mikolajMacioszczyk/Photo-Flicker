using Microsoft.Extensions.DependencyInjection;
using PhotoFlicker.Application.Repository.Page;
using PhotoFlicker.Application.Repository.Tag;

namespace PhotoFlicker.Web.ExtensionMethods
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services.AddScoped<IPhotoRepository, PhotoRepository>()
                .AddScoped<ITagRepository, TagRepository>();
        }
    }
}