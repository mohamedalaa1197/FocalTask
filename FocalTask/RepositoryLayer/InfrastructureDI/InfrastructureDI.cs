using FocalTask.ApplicationLayer;
using FocalTask.ApplicationLayer.Interfaces;
using FocalTask.ApplicationLayer.Services;
using FocalTask.RepositoryLayer.Interfaces;
using FocalTask.RepositoryLayer.Services;

namespace FocalTask.RepositoryLayer.InfrastructureDI;

public static class InfrastructureDI
{
    public static IServiceCollection AddInfastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IMovieService, MovieService>();
        services.AddScoped<IMovieRepository, MovieRepository>();
        services.AddAutoMapper(typeof(MapperProfile));
        return services;
    }
}