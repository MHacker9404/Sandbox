using Lamar;
using Marketplace.Domain;
using Marketplace.WebApi.Contracts.V1;
using Marketplace.WebApi.Repositories;
using Marketplace.WebApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;

namespace Marketplace.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        //  this is the default - use ConfigureContainer since we are using Lamar for DI
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        //public void ConfigureServices(IServiceCollection services)
        //{
        //    services.AddCors(options => { options.AddDefaultPolicy(builder => { builder.AllowAnyOrigin(); }); });
        //    // Supports ASP.Net Core DI abstractions
        //    //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0).AddNewtonsoftJson();
        //    services.AddControllers().AddNewtonsoftJson().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

        //    services.AddLogging();

        //    // Register the Swagger generator, defining 1 or more Swagger documents
        //    services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "My API", Version = "v1"}); });
        //}

        // Take in Lamar's ServiceRegistry instead of IServiceCollection
        // as your argument, but fear not, it implements IServiceCollection
        // as well
        public void ConfigureContainer(ServiceRegistry services)
        {
            Log.Debug($"ConfigureContainer");

            services.AddCors(options => { options.AddDefaultPolicy(builder => { builder.AllowAnyOrigin(); }); });

            // Supports ASP.Net Core DI abstractions
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0).AddNewtonsoftJson();
            services.AddControllers().AddNewtonsoftJson().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddLogging();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "My API", Version = "v1"}); });

            // Also exposes Lamar specific registrations
            // and functionality
            services.Scan(s =>
                          {
                              s.TheCallingAssembly();
                              s.WithDefaultConventions();
                          });

            //services.AddScoped<IHandleCommand<CreateAdCommand>,ClassifiedAdAppService>();
            services.AddScoped<IClassifiedAdRepository, ClassifiedAdRepository>();
            services.AddSingleton<ICurrencyLookup, FixedCurrencyLookup>();
            services.AddScoped<IApplicationService, ClassifiedAdAppService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Log.Debug($"Configure");

            app.UseCors();

            //if (env.IsEnvironment("Debug"))
            if (env.IsDevelopment())
            {
                // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseSwagger();

                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
                // specifying the Swagger JSON endpoint.
                app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });

                app.UseDeveloperExceptionPage();

                var container = (IContainer)app.ApplicationServices;
                Log.Debug(container.WhatDidIScan());
                Log.Debug(container.WhatDoIHave());
            }

            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}