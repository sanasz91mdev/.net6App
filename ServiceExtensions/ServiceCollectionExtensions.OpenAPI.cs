namespace Microsoft.Extensions.DependencyInjection;

using Microsoft.OpenApi.Models;

public static partial class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwagger();

        return builder;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo()
            {
                Description = "Digital banking APIs",
                Title = "Digital banking APIs",
                Version = "v1",
                Contact = new OpenApiContact()
                {
                    Name = "Sana",
                    Url = new Uri("https://github.com/nikiforovall")
                }
            });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,

                    },
                    new List<string>()
            }});
            c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
        });

        return services;
    }
}
