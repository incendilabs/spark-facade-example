using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Spark.Engine;
using Spark.Facade.Services;
using Spark.Engine.Extensions;
using Spark.Engine.Service;
using Spark.Engine.Service.FhirServiceExtensions;
using Spark.Engine.Store.Interfaces;
using Spark.Facade.Store;
using CapabilityStatementService = Spark.Facade.Services.CapabilityStatementService;

namespace Spark.Facade
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var settings = new SparkSettings();
            Configuration.Bind("SparkSettings", settings);
            var storeSettings = new StoreSettings();
            Configuration.Bind("StoreSettings", storeSettings);
            
            services.AddIdGenerator<GuidGenerator>();
            services.AddStore<IFhirStore, SqlStore>(storeSettings);

            services.AddFhirFacade(options =>
            {
                options.Settings = settings;

                options.FhirExtensions.TryAdd<ICapabilityStatementService, CapabilityStatementService>();
                options.FhirExtensions.TryAdd<IQueryService, QueryService>();

                options.FhirServices.TryAdd<IAsyncFhirService, AsyncFhirService>();
                options.FhirServices.TryAdd<PatientService>();

                options.MvcOption = mvcOptions =>
                {
                    mvcOptions.EnableEndpointRouting = false;
                    mvcOptions.InputFormatters.RemoveType<SystemTextJsonInputFormatter>();
                    mvcOptions.OutputFormatters.RemoveType<SystemTextJsonOutputFormatter>();
                };
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseRouting();

            app.UseFhir(builder => builder.MapRoute(name: "default", template: "{controller}/{action}/{id?}"));

            // app.UseEndpoints(endpoints =>
            // {
            //     endpoints.MapControllerRoute(
            //         name: "default",
            //         pattern: "{controller}/{action}{id?}"
            //     );
            // });
            //app.UseEndpoints(endpoints => { endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Hello World!"); }); });
        }
    }
}
