using System.Linq;
using AutoMapper;
using BaseWebApi.Infrastructure.Extensions;
using Lamar;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;

namespace BaseWebApi
{
    public class Startup
    {
        private readonly ILogger _logger;

        public Startup(IConfiguration configuration, ILogger logger)
        {
            _logger = logger;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureContainer(ServiceRegistry services)
        {
            Log.Debug("ConfigureContainer");

            services.AddLogging();

            services.AddCors(options => { options.AddDefaultPolicy(builder => { builder.AllowAnyOrigin(); }); });

            services.AddControllers(config =>
                                    {
                                        config.RespectBrowserAcceptHeader = true;
                                        config.ReturnHttpNotAcceptable = true;
                                        config.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
                                    })
                    .AddNewtonsoftJson()
                    .AddXmlDataContractSerializerFormatters()
                    .SetCompatibilityVersion(CompatibilityVersion.Latest);

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "My API", Version = "v1"}); });

            services.AddAutoMapper(typeof(Startup));

            // Also exposes Lamar specific registrations
            // and functionality
            services.Scan(s =>
                          {
                              s.TheCallingAssembly();
                              s.WithDefaultConventions();
                          });
        }

        /// <summary>
        /// https://docs.microsoft.com/en-us/aspnet/core/web-api/jsonpatch?view=aspnetcore-5.0
        /// </summary>
        /// <returns></returns>
        private NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
        {
            var builder = new ServiceCollection( )
                          .AddLogging( )
                          .AddMvc( )
                          .AddNewtonsoftJson( )
                          .Services.BuildServiceProvider( );

            return builder
                   .GetRequiredService<IOptions<MvcOptions>>( )
                   .Value
                   .InputFormatters
                   .OfType<NewtonsoftJsonPatchInputFormatter>( )
                   .First( );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // This will make the HTTP requests log as rich logs instead of plain text.
            app.UseSerilogRequestLogging();
            app.ConfigureExceptionHandler(_logger);

            Log.Debug("Configure");

            app.UseCors();
            //app.UseStaticFiles( );    //  not sure this is needed

            app.UseForwardedHeaders(new ForwardedHeadersOptions
                                    {
                                        ForwardedHeaders = ForwardedHeaders.All
                                    });

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });

            //if (env.IsEnvironment("Debug"))
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                var container = (IContainer) app.ApplicationServices;
                Log.Debug(container.WhatDidIScan());
                Log.Debug(container.WhatDoIHave());
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}