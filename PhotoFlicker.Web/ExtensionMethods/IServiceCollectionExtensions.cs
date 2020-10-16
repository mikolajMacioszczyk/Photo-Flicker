using Microsoft.Extensions.DependencyInjection;
using PhotoFlicker.Web.Db.Repository.Page;

namespace PhotoFlicker.Web.ExtensionMethods
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services.AddScoped<IPhotoRepository, PhotoRepository>();
        }
    }
}