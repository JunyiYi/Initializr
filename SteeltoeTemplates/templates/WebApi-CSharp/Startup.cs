﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
{{#RequiresHttps}}
using Microsoft.AspNetCore.HttpsPolicy;
{{/RequiresHttps}}
using Microsoft.AspNetCore.Mvc;
{{#Auth}}
using Microsoft.AspNetCore.Authentication;
{{/Auth}}
{{#OrganizationalAuth}}
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
{{/OrganizationalAuth}}
{{#IndividualB2CAuth}}
using Microsoft.AspNetCore.Authentication.AzureADB2C.UI;
{{/IndividualB2CAuth}}
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Company.WebApplication1
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
{{#OrganizationalAuth}}
            services.AddAuthentication(AzureADDefaults.BearerAuthenticationScheme)
                .AddAzureADBearer(options => Configuration.Bind("AzureAd", options));
{{/OrganizationalAuth}}
{{#IndividualB2CAuth}}
            services.AddAuthentication(AzureADB2CDefaults.BearerAuthenticationScheme)
                .AddAzureADB2CBearer(options => Configuration.Bind("AzureAdB2C", options));
{{/IndividualB2CAuth}}
{{#CloudfoundryHosting}}

{{/CloudfoundryHosting}}
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
{{#RequiresHttps}}
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
{{/RequiresHttps}}


{{#Auth}}
            app.UseAuthentication();
{{/Auth}}
            app.UseMvc();
        }
    }
}
