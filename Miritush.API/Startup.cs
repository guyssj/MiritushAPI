using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Miritush.API.Extensions;
using Miritush.DAL.Model;
using Miritush.Services;
using Miritush.Services.Abstract;
using Miritush.Services.DomainProfile;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace Miritush.API
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });
            var serverVersion = new MySqlServerVersion(new Version(5, 7, 34));

            // Replace 'YourDbContext' with the name of your own DbContext derived class.
            services.AddDbContext<booksDbContext>(
                dbContextOptions => dbContextOptions
                    .UseMySql(configuration.GetConnectionString("BooksDB"), serverVersion)
            // // The following three options help with debugging, but should
            // // be changed or removed for production.
            // .LogTo(Console.WriteLine, LogLevel.Information)
            // .EnableSensitiveDataLogging()
            // .EnableDetailedErrors()
            );
            services.AddHttpContextAccessor();
            services.AddHttpClient("GlobalSms", c =>
            {

                var GlobalSmsUrl = configuration.GetValue<string>("SmsProvider:BaseUrl");
                var ApiKey = configuration.GetValue<string>("SmsProvider:ApiKey");
                var queryParams = new Dictionary<string, string>();
                queryParams["ApiKey"] = ApiKey;
                queryParams["txtAddInf"] = "LocalMessageID";

                var BaseWithQuery = QueryHelpers.AddQueryString(GlobalSmsUrl, queryParams);
                c.BaseAddress = new Uri(BaseWithQuery);
            });


            services
                .AddControllers(config =>
                {
                    config.EnableEndpointRouting = false;
                    var authorizationPolicy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();
                    config.Filters.Add(new AuthorizeFilter(authorizationPolicy));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Latest)
                .AddNewtonsoftJson(config =>
                {
                    // config.SerializerSettings.DateFormatString = "yyyy-MM-ddTHH:mm:ssZ";
                    config.SerializerSettings.Converters.Add(new StringEnumConverter());
                    config.UseCamelCasing(true);
                })
                .AddJsonOptions(conf =>
                {
                    conf.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Miritush.API", Version = "v1" });
                c.DocumentFilter<ReplaceVersionWithExactValueInPath>();
                c.OperationFilter<RemoveVersionFromParameter>();
                c.CustomSchemaIds(type => type.ToString());
                c.DocInclusionPredicate((version, desc) =>
                {
                    if (!desc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;

                    var versions = methodInfo.DeclaringType
                        .GetCustomAttributes(true)
                        .OfType<ApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions);

                    var mapVersions = methodInfo
                        .GetCustomAttributes(true)
                        .OfType<MapToApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions)
                        .ToArray();

                    return versions.Any(v => $"v{v}" == version) && (mapVersions.Length == 0 || mapVersions.Any(v => $"v{v}" == version));
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
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
                        }
                    });


            });
            services.AddRouting(opt => opt.LowercaseUrls = true);

            services.AddScoped(serviceProvider =>
            {
                var domainProfile = ActivatorUtilities.GetServiceOrCreateInstance<DomainProfile>(serviceProvider);

                var config = new MapperConfiguration(config => config
                    .AddProfile(domainProfile)
                );

                return config.CreateMapper();
            });
            AddCoreServices(services);
            AddAuthServices(services);


        }


        private class RemoveVersionFromParameter : IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                var versionParameter = operation.Parameters.Single(p => p.Name == "version");
                operation.Parameters.Remove(versionParameter);
            }
        }
        private class ReplaceVersionWithExactValueInPath : IDocumentFilter
        {
            public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
            {
                var paths = swaggerDoc.Paths
                    .ToDictionary(
                        path => path.Key.Replace("v{version}", swaggerDoc.Info.Version),
                        path => path.Value
                    );
                swaggerDoc.Paths = new OpenApiPaths();

                foreach (var path in paths)
                    swaggerDoc.Paths.Add(path.Key, path.Value);
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Miritush.API v1");
                    c.EnableValidator();
                    c.EnableDeepLinking();
                    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                    c.DisplayRequestDuration();
                    c.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Example);
                    c.EnableFilter();
                    c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
                });
            }
            app.UseApiVersioning();
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void AddCoreServices(IServiceCollection services)
        {
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IServicesService, ServicesService>();
            services.AddScoped<IServiceTypeService, ServiceTypeService>();
            services.AddScoped<ILockHoursService, LockHoursService>();
            services.AddScoped<IWorkingHoursService, WorkingHoursService>();
            services.AddScoped<ITimeSlotService, TimeSlotService>();
            services.AddScoped<ICalendarService, CalendarService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserContextService, UserContextService>();
            services.AddScoped<IBookService, BookService>();
        }
        private void AddAuthServices(IServiceCollection services)
        {

            var secretKey = configuration.GetValue<string>("Auth:Secret");
            var key = Encoding.UTF8.GetBytes(secretKey);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                //TODO::
                //https://www.gwllosa.com/post/dynamic-key-validation-with-jwt-in-asp-net-core
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    RequireSignedTokens = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = configuration.GetValue<string>("Auth:ValidIssuer"),
                    ValidateAudience = true,
                    ValidAudience = configuration.GetValue<string>("Auth:ValidAudience"),
                    ValidateLifetime = true
                };

                x.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Request.Headers.TryGetValue("Authorization", out var token);

                        if (string.IsNullOrWhiteSpace(token.ToString()))
                            context.Request.Headers.TryGetValue("authorization", out token);

                        context.Token = token.ToString()
                            .Replace("Bearer ", "")
                            .ToString();

                        return Task.CompletedTask;
                    },
                    OnTokenValidated = async context =>
                    {
                        context.Principal = await context.HttpContext.AttachIdentityToContext(context.Principal);
                    }
                };
            });
        }
    }
}
