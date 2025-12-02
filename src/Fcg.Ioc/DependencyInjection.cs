using Fcg.Data.Repository;
using Fcg.Data.Services;
using Fcg.Domain.Interfaces;
using Fcg.Shareable.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fcg.IOC;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ConnectionString")));

        services.AddScoped<IGameRepository, GameRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(Fcg.Domain.UserHandlers.CreateUserHandler).Assembly));

        services.AddValidatorsFromAssemblyContaining<UserDtoValidator>();
        services.AddFluentValidationAutoValidation();

        return services;
    }
}