using FirebaseAdmin;
using FirebaseAdmin.Auth;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;

namespace Todo.Api.Configurations;

internal static class StartupBuilderConfigurations
{
    /// <summary>
    ///     Adds the Firebase app, auth, and messaging services to the application builder.
    /// </summary>
    /// <param name="builder">
    ///     The WebApplicationBuilder to add the Firebase services to.
    /// </param>
    public static void AddFirebaseServices(this WebApplicationBuilder builder)
    {
        var firebaseApp = FirebaseApp.Create(new AppOptions
        {
            Credential = GoogleCredential.FromFile("OAuthClientCredentials.json")
        });
        var firebaseAuth = FirebaseAuth.GetAuth(firebaseApp);
        var firebaseMessaging = FirebaseMessaging.GetMessaging(firebaseApp);

        builder.Services.AddSingleton(firebaseApp);
        builder.Services.AddSingleton(firebaseAuth);
        builder.Services.AddSingleton(firebaseMessaging);
    }

    /// <summary>
    ///     Adds the Firebase authentication service to the application builder.
    /// </summary>
    /// <param name="builder">builder — The WebApplicationBuilder to add the Firebase authentication services to</param>
    public static void AddFirebaseAuthentication(this WebApplicationBuilder builder)
    {
        var firebaseCredentials = builder.Configuration.GetSection("Authentication:Firebase").Get<FirebaseCredentials>()!;

        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
                    ValidateLifetime = true
                };
                options.RequireHttpsMetadata = false;
            });
    }

    /// <summary>
    ///    Adds the database connection string to the application builder.
    /// </summary>
    /// <param name="builder">builder — The WebApplicationBuilder to add the CORS services to</param>
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