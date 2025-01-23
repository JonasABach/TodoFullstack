using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Todo.Core.Entities;
using Todo.Core.Exceptions;
using Todo.Data.DatabaseContexts;
using Todo.Infrastructure.Configurations;

namespace Todo.Api.Configurations;

public static class StartupBuilderConfigurations
{
    private static HttpClient _httpClient = new();
    private static Dictionary<string, string> _cachedKeys;
    private static DateTime _keyExpirationTime;

    /// <summary>
    ///     Adds the database connection string to the application builder.
    /// </summary>
    /// <param name="builder">
    ///     The WebApplicationBuilder to initialize.
    /// </param>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the connection string is not found in the configuration.
    /// </exception>
    public static void AddSqliteDb(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString(Constants.SqliteConnectionStringName)
                               ?? throw new InvalidOperationException(
                                   $"Connection string '{Constants.SqliteConnectionStringName}' not found.");
        builder.Services.AddDbContext<TodoIdentityContext>(options => options.UseSqlite(connectionString));
    }

    /// <summary>
    ///     Initializes the JWT configurations for the application builder.
    /// </summary>
    /// <param name="builder">
    ///     The WebApplicationBuilder to initialize.
    /// </param>
    public static void InitializeJwtConfigurations(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<JwtConfigurations>(
            builder.Configuration.GetSection(Constants.JwtConfigurationsSectionKey));
    }

    /// <summary>
    ///     Adds the authentication service to the application builder with JWT authentication.
    /// </summary>
    /// <param name="builder">
    ///     The WebApplicationBuilder to add the authentication service to.
    /// </param>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the JWT Signing Key is not found in the configuration.
    /// </exception>
    public static void AddAuthenticationService(this WebApplicationBuilder builder)
    {
        var jwtConfigurations = new JwtConfigurations();
        builder.Configuration.GetSection(Constants.JwtConfigurationsSectionKey).Bind(jwtConfigurations);

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtConfigurations.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtConfigurations.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfigurations.SecretKey)),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });
    }

    public static void AddFirebaseAuthentication(this WebApplicationBuilder builder)
    {
        var firebaseCredentials = new FirebaseCredentials();
        builder.Configuration.GetSection("Authentication:Firebase").Bind(firebaseCredentials);
        

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.IncludeErrorDetails = true;
                options.Authority = $"https://securetoken.google.com/{firebaseCredentials.Project_Id}";
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = $"https://securetoken.google.com/{firebaseCredentials.Project_Id}",
                    ValidateAudience = true,
                    ValidAudience = firebaseCredentials.Project_Id,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(firebaseCredentials.Private_Key))
                };
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        try
                        {
                            // Receive the JWT token that firebase has provided
                            var firebaseToken = context.SecurityToken as Microsoft.IdentityModel.JsonWebTokens.JsonWebToken;
                            // Get the Firebase UID of this user
                            var firebaseUid = firebaseToken?.Claims.FirstOrDefault(c => c.Type == "user_id")?.Value;
                            if (!string.IsNullOrEmpty(firebaseUid))
                            {
                                // Use the Firebase UID to find or create the user in your Identity system
                                var userManager = context.HttpContext.RequestServices
                                    .GetRequiredService<UserManager<User>>();
                                var user = await userManager.FindByNameAsync(firebaseUid);
                                if (user is null)
                                {
                                    var email = firebaseToken?.Claims.FirstOrDefault(c => c.Type == "email")?.Value ??
                                                throw new InvalidEmailException("Email not found in Firebase token.");
                                    user = new User
                                    {
                                        Id = firebaseUid,
                                        Email = email,
                                        Username = email,
                                        FirstName = firebaseToken.Claims.FirstOrDefault(c => c.Type == "name")?.Value ?? $"Tasker {email}",
                                        LastName = string.Empty,
                                        Lists = [],
                                        RefreshToken = null
                                    };
                                    await userManager.CreateAsync(user);
                                }
                            }
                        } catch(Exception ex)
                        {
                            context.Fail(ex);
                            throw new FirebaseTokenValidationException("Invalid Firebase token", ex);
                        }
                    }
                };
            });
    }

    /// <summary>
    ///     Adds the Identity service to the application builder.
    ///     Configures the Identity options for the application.
    /// </summary>
    /// <param name="builder">
    ///     The WebApplicationBuilder to add the Identity service to.
    /// </param>
    public static void AddIdentityService(this WebApplicationBuilder builder)
    {
        builder.Services.AddIdentity<User, IdentityRole>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;

            options.Password.RequiredLength = 12;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredUniqueChars = 0;

            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            options.User.AllowedUserNameCharacters = Constants.AllowedUserNameCharacters;
            options.User.RequireUniqueEmail = true;
        }).AddEntityFrameworkStores<TodoIdentityContext>();
    }

    /// <summary>
    ///    Adds the database connection string to the application builder.
    /// </summary>
    /// <param name="builder"></param>
    public static void AddCorsService(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(Constants.ClientCrossOriginPolicyDevName, builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
            options.AddPolicy(Constants.ClientCrossOriginPolicyProductionName, builder =>
            {
                builder
                    .WithOrigins(Constants.ClientCrossOriginPolicyProductionURL)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });
    }

    /// <summary>
    ///     Adds the Swagger service to the application builder for API documentation with JWT authentication.
    /// </summary>
    /// <param name="builder">
    ///     The WebApplicationBuilder to add the Swagger service to.
    /// </param>
    public static void AddSwaggerService(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,

                Flows = new OpenApiOAuthFlows
                {
                    Password = new OpenApiOAuthFlow
                    {
                        TokenUrl = new Uri("/v1/auth", UriKind.Relative),
                        Extensions = new Dictionary<string, IOpenApiExtension>
                        {
                            { "returnSecureToken", new OpenApiBoolean(true) },
                        },
                    }
                }
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        },
                        Scheme = "oauth2",
                        Name = JwtBearerDefaults.AuthenticationScheme,
                        In = ParameterLocation.Header,
                    },
                    new List<string> { "openid", "email", "profile" }
                }
            });
        });
    }

    /// <summary>
    ///     Adds the logging service to the application builder.
    /// </summary>
    /// <param name="builder">
    ///     The WebApplicationBuilder to add the logging service to.
    /// </param>
    public static void AddLoggingService(this WebApplicationBuilder builder)
    {
        builder.Services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddConsole()
                .AddFilter("Microsoft.AspNetCore.Authentication", LogLevel.Debug)
                .AddFilter("Microsoft.AspNetCore.Authorization", LogLevel.Debug);
        });
        builder.Services.AddHttpLogging(configureOptions: options =>
        {
            options.LoggingFields = HttpLoggingFields.All;
            options.RequestBodyLogLimit = 4096;
            options.ResponseBodyLogLimit = 4096;
            options.RequestHeaders.Add("Authorization");
            options.ResponseHeaders.Add("Authorization");
        });
    }
}