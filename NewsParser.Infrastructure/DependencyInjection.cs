using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NewsParser.Contracts.Parsing;


namespace NewsParser.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        
        AddSql(services, configuration);
        AddHttpClients(services, configuration);
        AddAuth(services, configuration);
        AddParsing(services, configuration);

        return services;
    }
    
    private static IServiceCollection AddSql(this IServiceCollection services, ConfigurationManager configuration)
    {
        var sqlSettings = new SqlSettings();
        configuration.Bind(SqlSettings.SectionName, sqlSettings);
        services.AddSingleton(Options.Create(sqlSettings));
        
        services.AddScoped<IPostsRepository, PostsRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<SqlServerDatabaseContext>(x => new SqlServerDatabaseContext(sqlSettings.ConnectionStrings.AfterDbCreationConnection));
        services.AddHostedService<DatabaseHostedService>();
        services.AddScoped<IDatabaseCreationService, DatabaseCreationService>();
        services.AddScoped<IDatabaseSeedingService, DatabaseSeedingService>();
        
        return services;
    }
    
    private static IServiceCollection AddHttpClients(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddHttpClient<ITechCrunchClient, TechCrunchClient>(client =>
        {
            client.BaseAddress = new Uri($"{configuration.GetValue<string>("TechCrunchAddress")}");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        });
        
        return services;
    }
    
    public static IServiceCollection AddAuth(this IServiceCollection services, ConfigurationManager configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.SectionName, jwtSettings);
        services.AddSingleton(Options.Create(jwtSettings));
        
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
                };
            });
        
        return services;
    }
    
    public static IServiceCollection AddParsing(this IServiceCollection services, ConfigurationManager configuration)
    {
        var parsingSettings = new ParsingSettings();
        configuration.Bind(ParsingSettings.SectionName, parsingSettings);
        services.AddSingleton(Options.Create(parsingSettings));
        services.AddSingleton<ICustomJsonSerializer<Entity>, CustomJsonSerializer<Entity>>();
        services.AddScoped<IParsingService, ParsingService>();
        
        return services;
    }
}