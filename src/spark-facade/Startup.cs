/*
 * Copyright (c) 2021-2023, Incendi (info@incendi.no)
 *
 * SPDX-License-Identifier: BSD-2-Clause
 */

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            services.AddFhirFacade(options =>
            {
                options.Settings = settings;
                options.StoreSettings = storeSettings;

                options.FhirExtensions.TryAdd<ICapabilityStatementService, CapabilityStatementService>();
                options.FhirExtensions.TryAdd<IQueryService, QueryService>();
                options.FhirExtensions.TryAdd<IPatchService, PatchService>();

                options.FhirServices.TryAdd<IFhirService, FhirService>();
                options.FhirServices.TryAdd<PatientService>();

                options.FhirStores.TryAdd<IFhirStore, PatientStore>();

                options.MvcOption = mvcOptions =>
                {
                    mvcOptions.EnableEndpointRouting = false;
                    mvcOptions.InputFormatters.RemoveType<SystemTextJsonInputFormatter>();
                    mvcOptions.OutputFormatters.RemoveType<SystemTextJsonOutputFormatter>();
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseFhir(builder => builder.MapRoute(name: "default", template: "{controller}/{action}/{id?}"));
        }
    }
}
