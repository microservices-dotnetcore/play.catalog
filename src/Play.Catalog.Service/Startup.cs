using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Identity;
using Play.Common.Identity;
using Play.Common.MassTransit;
using Play.Common.MongoDB;
using Play.Common.Settings;
using Scalar.AspNetCore;

namespace Play.Catalog.Service
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        private ServiceSettings serviceSettings;
        private const string AllowedOriginSetting = "AllowedOrigin";

        public Startup(IConfiguration configuration)
        { 
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            serviceSettings = Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();

            services.AddMongo().AddMongoRepository<Item>("items");

            services.AddAuthorization(options => 
            {
                options.AddPolicy(Policies.Read, policy => {
                    policy.RequireRole(Roles.Admin);
                    policy.RequireClaim("scope", "catalog.readaccess", "catalog.fullaccess");
                });

                options.AddPolicy(Policies.Write, policy => {
                    policy.RequireRole(Roles.Admin);
                    policy.RequireClaim("scope", "catalog.writeaccess", "catalog.fullaccess");
                });
            });

            services.AddMassTransitWithRabbitMQ();

            services.AddJwtBearerAuthentication();

            services.AddControllers(options => 
            { 
                options.SuppressAsyncSuffixInActionNames = false;
            });
            services.AddOpenApi();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/openapi/v1.json", "Play.Catalog API V1");
                    c.RoutePrefix = "swagger"; // Set Swagger UI at the app's root
                });

                app.UseCors(builder => 
                {
                    builder.WithOrigins(Configuration[AllowedOriginSetting])
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                if (env.IsDevelopment())
                {
                    endpoints.MapOpenApi();
                    endpoints.MapScalarApiReference();
                }

            });

        }
    }
}