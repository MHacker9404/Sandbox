using System;
using EventStore.ClientAPI;
using Lamar;
using Marketplace.Domain.Shared;
using Marketplace.Framework;
using Marketplace.WebApi.Controllers.UserProfile;
using Marketplace.WebApi.Infrastructure;
using Marketplace.WebApi.Projections;
using Marketplace.WebApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;
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

            services.AddSingleton<ICurrencyLookup, FixedCurrencyLookup>();

            //  EventStore
            var eventStoreConnection = EventStoreConnection.Create(Configuration["eventStore:connectionString"]
                                                                   , ConnectionSettings.Create().KeepReconnecting()
                                                                   , Configuration["ApplicationName"]);
            services.AddSingleton(eventStoreConnection);
            var eventStore = new EsAggregateStore(eventStoreConnection);
            services.AddSingleton<IAggregateStore>(eventStore);

            //  RavenDB
            var documentStore = ConfigureRavenDb(Configuration.GetSection("ravenDb"));
            Func<IAsyncDocumentSession> getSession = () => documentStore.OpenAsyncSession();
            services.AddSingleton(getSession);
            //services.AddSingleton(_ => getSession());

            services.AddSingleton<ClassifiedAdAppService>();

            var pClient = new PurgoMalumClient();
            services.AddSingleton<CheckTextForProfanity>(text => pClient.CheckForProfanity(text));
            services.AddSingleton<UserProfileAppService>();

            services.AddSingleton<ICheckpointStore>(s =>
                                                        new RavenDbCheckpointStore(getSession
                                                                                   , s.GetService<ILogger>()));

            services.AddSingleton(s => new ClassifiedAdDetailsProjection(s.GetService<Func<IAsyncDocumentSession>>()
                                                                         , s.GetService<ILogger>()
                                                                         , async userId =>
                                                                               (await getSession.GetUserDetailsAsync(userId))?.DisplayName));
            services.AddSingleton(s => new ClassifiedAdUpcasterProjection(s.GetService<IEventStoreConnection>()
                                                                          , async userId =>
                                                                                (await getSession.GetUserDetailsAsync(userId))?.PhotoUrl
                                                                          , s.GetService<ILogger>()));
            services.AddSingleton<UserDetailsProjection>();

            services.AddSingleton(s => new ProjectionManager(eventStoreConnection
                                                             , new IProjection[]
                                                               {
                                                                   s.GetService<ClassifiedAdDetailsProjection>()
                                                                   , s.GetService<ClassifiedAdUpcasterProjection>()
                                                                   , s.GetService<UserDetailsProjection>()
                                                               }
                                                             , s.GetService<ILogger>()
                                                             , s.GetService<ICheckpointStore>()));

            services.AddSingleton<IHostedService, EventStoreHostedService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // This will make the HTTP requests log as rich logs instead of plain text.
            app.UseSerilogRequestLogging();

            Log.Debug("Configure");

            app.UseCors();

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

            //app.UseAuthorization( );
            //app.UseHttpsRedirection();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private static IDocumentStore ConfigureRavenDb(IConfiguration configuration)
        {
            var store = new DocumentStore
                        {
                            Urls = new[] {configuration["server"]}, Database = configuration["database"]
                            , Conventions =
                            {
                                FindIdentityProperty = m => m.Name == "DbId"
                                , CustomizeJsonDeserializer = serializer =>
                                                              {
                                                                  serializer.ContractResolver = new PrivateResolver();
                                                                  serializer.ConstructorHandling =
                                                                      ConstructorHandling.AllowNonPublicDefaultConstructor;
                                                              }
                            }
                        };
            store.Initialize();
            var record = store.Maintenance.Server.Send(new GetDatabaseRecordOperation(store.Database));
            if (record == null)
                store.Maintenance.Server.Send(
                    new CreateDatabaseOperation(new DatabaseRecord(store.Database)));

            return store;
        }
    }
}