using System.IO;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using SampleApi.Options;
using Toolbox.Logstash;
using Toolbox.WebApi;

namespace SampleApi
{
    public class Startup
    {
		public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
		{
            ApplicationBasePath = appEnv.ApplicationBasePath;
            ConfigPath = Path.Combine(ApplicationBasePath, "_config");
            
            var builder = new ConfigurationBuilder()
                .SetBasePath(ConfigPath)
                .AddJsonFile("logging.json")
                .AddJsonFile("app.json")
                .AddEnvironmentVariables();
            
            Configuration = builder.Build();
		}
		
        public IConfigurationRoot Configuration { get; private set; }
        public string ApplicationBasePath { get; private set; }
        public string ConfigPath { get; private set; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            // Check out ExampleController to find out how these configs are injected into other classes
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            
			services.AddMvc()
                .AddActionOverloading()
                .AddVersioning();
            
            services.AddBusinessServices();
            services.AddAutoMapper();
                
            services.AddSwaggerGen();
		}
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
            loggerFactory.AddSeriLog(Configuration.GetSection("SeriLog"));
            loggerFactory.AddConsole(Configuration.GetSection("ConsoleLogging"));
            loggerFactory.AddDebug(LogLevel.Debug);

            loggerFactory.AddLogstashHttp(app, opt => 
            {
                opt.AppId = "LogstashToolboxSample";
                opt.Index = "LogstashToolboxSample";
                opt.MessageVersion = "1";
                opt.MinimumLevel = LogLevel.Information;
                opt.Url = "http://e27-elk.cloudapp.net:8080/api/v2/messages";
            });
            
			// CORS
            app.UseCors((policy) => {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.AllowAnyOrigin();
                policy.AllowCredentials();
            });

            app.UseIISPlatformHandler();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "api/{controller}/{id?}");
			});
            
            app.UseSwaggerGen();
            app.UseSwaggerUi();
            app.UseSwaggerUiRedirect();
		}
        
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
