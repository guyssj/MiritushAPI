using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Miritush.DAL.Model;
using Miritush.Services;
using Miritush.Services.Abstract;
using Miritush.Services.DomainProfile;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;
using System.Reflection;

namespace Miritush.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

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
                    .UseMySql(Configuration.GetConnectionString("BooksDB"), serverVersion)
            // // The following three options help with debugging, but should
            // // be changed or removed for production.
            // .LogTo(Console.WriteLine, LogLevel.Information)
            // .EnableSensitiveDataLogging()
            // .EnableDetailedErrors()
            );


            services.AddControllers();
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
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Miritush.API v1"));
            }
            app.UseApiVersioning();
            app.UseHttpsRedirection();

            app.UseRouting();

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

        }
    }
}
