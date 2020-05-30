using System;
using System.Collections.Generic;
using System.Linq;
using EventStore.ClientAPI;
using Lamar;
using Marketplace.Domain.ClassifiedAd;
using Marketplace.Domain.Shared;
using Marketplace.Domain.UserProfile;
using Marketplace.Framework;
using Marketplace.WebApi.Controllers.ClassifiedAds.ReadModels;
using Marketplace.WebApi.Infrastructure;
using Marketplace.WebApi.Projections;
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
using ILogger = Serilog.ILogger;

namespace Marketplace.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureContainer(ServiceRegistry services)
        {
            Log.Debug("ConfigureContainer");

            services.AddLogging();

            services.AddCors(options => { options.AddDefaultPolicy(builder => { builder.AllowAnyOrigin(); }); });

            services.AddControllers().AddNewtonsoftJson().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "My API", Version = "v1"}); });

            // Also exposes Lamar specific registrations
            // and functionality
            services.Scan(s =>
                          {
                              s.TheCallingAssembly();
                              s.WithDefaultConventions();
                          });

            var users = new HashSet<UserProfile>();
            services.AddSingleton(users);
            services.AddSingleton<UserDetailsProjection>( );

            services.AddSingleton<ICurrencyLookup, FixedCurrencyLookup>( );

            var eventStoreConnection = EventStoreConnection.Create( Configuration[ "eventStore:connectionString" ]
                                                                   , ConnectionSettings.Create( ).KeepReconnecting( )
                                                                   , Configuration[ "ApplicationName" ] );
            services.AddSingleton( eventStoreConnection );
            services.AddSingleton<IAggregateStore, EsAggregateStore>( );

            string GetUserPhoto(Guid userId) => users.FirstOrDefault(u => u.Id == userId)?.PhotoUrl;

            services.AddSingleton(s => new ClassifiedAdUpcasterProjection(s.GetService<IEventStoreConnection>(), GetUserPhoto));

            var details = new HashSet<ClassifiedAdDetails>( );
            services.AddSingleton( details );
            string GetUserDisplayName( Guid userId )
            {
                var result = users.SingleOrDefault(user => user.Id == userId);
                return result?.DisplayName;
            }
            services.AddSingleton( (Func<Guid, string>)GetUserDisplayName );
            services.AddSingleton<ClassifiedAdDetailsProjection>( );
            services.AddSingleton( s => new ProjectionManager( eventStoreConnection
                                                              , new IProjection[ ]
                                                                {
                                                                   s.GetService<ClassifiedAdDetailsProjection>()
                                                                   , s.GetService<UserDetailsProjection>()
                                                                   , s.GetService<ClassifiedAdUpcasterProjection>()
                                                                }
                                                              , s.GetService<ILogger>( ) ) );
            services.AddSingleton<IHostedService, EventStoreHostedService>( );

            services.AddScoped<IClassifiedAdRepository, ClassifiedAdRepository>( );
            services.AddScoped<ClassifiedAdAppService>( );

            services.AddScoped<IUserProfileRepository, UserProfileRepository>( );
            var purgoMalumClient = new PurgoMalumClient( );
            services.AddScoped( s => new UserProfileAppService( s.GetService<IAggregateStore>( )
                                                               , text => purgoMalumClient.CheckForProfanity( text ).GetAwaiter( ).GetResult( )
                                                               , s.GetService<ILogger>( ) ) );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Log.Debug("Configure");

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

                var container = (IContainer) app.ApplicationServices;
                //Log.Debug(container.WhatDidIScan());
                Log.Debug(container.WhatDoIHave());
            }

            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}