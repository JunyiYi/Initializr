using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

{{#ActuatorsOrDynamicLogger}}
using Steeltoe.Extensions.Logging;
{{/ActuatorsOrDynamicLogger}}
{{#CloudFoundry}}
{{^ConfigServer}}
using Steeltoe.Extensions.Configuration;
using Steeltoe.Extensions.Configuration.CloudFoundry;
{{/ConfigServer}}
{{/CloudFoundry}}
{{#ConfigServer}}
using Steeltoe.Extensions.Configuration.ConfigServer;
{{/ConfigServer}}
{{#PlaceholderConfig}}
using Steeltoe.Extensions.Configuration.PlaceholderCore;
{{/PlaceholderConfig}}
{{#RandomValueConfig}}
using Steeltoe.Extensions.Configuration.RandomValue;
{{/ RandomValueConfig}}
namespace {{ProjectNameSpace}}
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args)
            .Build()
            {{#AnyEFCore}}
            .InitializeDbContexts()
            {{/AnyEFCore}}
            .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var builder = WebHost.CreateDefaultBuilder(args)
                .UseDefaultServiceProvider(configure => configure.ValidateScopes = false)
                {{#CloudFoundry}}
                .UseCloudFoundryHosting() //Enable listening on a Env provided port
                {{^ConfigServer}}
                .AddCloudFoundry() //Add cloudfoundry environment variables as a configuration source
                {{/ConfigServer}}
                {{/CloudFoundry}}
                {{#ConfigServer}}
			    .AddConfigServer()
                {{/ConfigServer}}
                {{#PlaceholderConfig}}
                .AddPlaceholderResolver()
                {{/PlaceholderConfig}}
                {{#RandomValueConfig}}
                .ConfigureAppConfiguration((b) => b.AddRandomValueSource())
                {{/RandomValueConfig}}
                .UseStartup<Startup>();
{{#ActuatorsOrDynamicLogger}}
            builder.ConfigureLogging((hostingContext, loggingBuilder) =>
            {
                loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                loggingBuilder.AddDynamicConsole();
            });
{{/ActuatorsOrDynamicLogger}}
            return builder;
        }
    }
}
