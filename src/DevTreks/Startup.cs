using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DevTreks.Models;
using DevTreks.Services;
using Microsoft.AspNetCore.Localization;

namespace DevTreks
{
    /// <summary>
    ///Purpose:		Configure the web app and start MVC web page
    ///             delivery.
    ///Author:		www.devtreks.org
    ///Date:		2016, June
    ///References:	www.devtreks.org
    /// </summary>
    public class Startup
    {
        public IConfigurationRoot Configuration { get; set; }
        //set config and httpcontext settings
        DevTreks.Data.ContentURI ContentURI { get; set; }
        //distinguish localhost from azure
        private static string PlatformType { get; set; }

        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
            ContentURI = new DevTreks.Data.ContentURI();
            //azure or localhost?
            PlatformType = env.WebRootPath;
        }
        private static bool IsAzurePlatform()
        {
            bool bIsAzure = false;
            if (PlatformType.StartsWith("https"))
            {
                bIsAzure = true;
            }
            return bIsAzure;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(Configuration["ConnectionStrings:DebugConnection"]));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();

            //refer to https://docs.asp.net/en/1.0.0-rc2/fundamentals/localization.html
            //doesn't include the errors project -don't refactor because not clear why this is actually needed
            services.AddLocalization(options => options.ResourcesPath = "DevTreks.Resources");

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            services.AddOptions();
            //this will be passed to Controller constructor and used to 
            //construct services and repositories 
            //inject config settings in controller and pass to views and repositories
            services.Configure<DevTreks.Data.ContentURI>(ContentURI =>
            {
                string connection = string.Empty;
                string path = string.Empty;
                //azure is debugged by commenting in and out the azure for localhost:5000 appsettings
                connection = Configuration["ConnectionStrings:DebugConnection"];
                ContentURI.URIDataManager.DefaultConnection = connection;
                //azure release has to modify this
                connection = Configuration["ConnectionStrings:DebugStorageConnection"];
                ContentURI.URIDataManager.StorageConnection = connection;
                path = Configuration["DebugPaths:DefaultRootFullFilePath"];
                ContentURI.URIDataManager.DefaultRootFullFilePath = path;
                path = Configuration["DebugPaths:DefaultRootWebStoragePath"];
                ContentURI.URIDataManager.DefaultRootWebStoragePath = path;
                path = Configuration["DebugPaths:DefaultWebDomain"];
                ContentURI.URIDataManager.DefaultWebDomain = path;
                path = Configuration["DebugPaths:ExtensionsPath"];
                ContentURI.URIDataManager.ExtensionsPath = path;
                path = Configuration["Site:FileSizeValidation"];
                ContentURI.URIDataManager.FileSizeValidation = path;
                path = Configuration["Site:FileSizeDBStorageValidation"];
                ContentURI.URIDataManager.FileSizeDBStorageValidation = path;
                path = Configuration["Site:PageSize"];
                ContentURI.URIDataManager.PageSize
                = DevTreks.Data.Helpers.GeneralHelpers.ConvertStringToInt(path);
                path = Configuration["Site:PageSizeEdits"];
                ContentURI.URIDataManager.PageSizeEdits = path;
                path = Configuration["Site:RExecutable"];
                ContentURI.URIDataManager.RExecutable = path;
                path = Configuration["Site:PyExecutable"];
                ContentURI.URIDataManager.PyExecutable = path;
                path = Configuration["Site:HostFeeRate"];
                ContentURI.URIDataManager.HostFeeRate = path;
                path = Configuration["URINames:ResourceURIName"];
                ContentURI.URIDataManager.ResourceURIName = path;
                path = Configuration["URINames:ContentURIName"];
                ContentURI.URIDataManager.ContentURIName = path;
                path = Configuration["URINames:TempDocsURIName"];
                ContentURI.URIDataManager.TempDocsURIName = path;

            });
        }
        public void ConfigureProductionServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(Configuration["ConnectionStrings:ReleaseConnection"]));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();

            //refer to https://docs.asp.net/en/1.0.0-rc2/fundamentals/localization.html
            services.AddLocalization(options => options.ResourcesPath = "DevTreks.Resources");

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            services.AddOptions();
            //this will be passed to Controller constructor and used to 
            //construct services and repositories 
            //inject config settings in controller and pass to views and repositories
            services.Configure<DevTreks.Data.ContentURI>(ContentURI =>
            {
                string connection = string.Empty;
                string path = string.Empty;
                //azure and web server use release paths
                path = Configuration["ReleasePaths:DefaultRootFullFilePath"];
                ContentURI.URIDataManager.DefaultRootFullFilePath = path;
                //getplatformtype expects this to be string empty to debug azure on localhost
                path = Configuration["ReleasePaths:DefaultRootWebStoragePath"];
                ContentURI.URIDataManager.DefaultRootWebStoragePath = path;
                path = Configuration["ReleasePaths:DefaultWebDomain"];
                ContentURI.URIDataManager.DefaultWebDomain = path;
                path = Configuration["ReleasePaths:ExtensionsPath"];
                ContentURI.URIDataManager.ExtensionsPath = path;
                if (IsAzurePlatform())
                {
                    connection = Configuration["AzureConnection"];
                    ContentURI.URIDataManager.DefaultConnection = connection;
                    connection = Configuration["AzureStorage"];
                    ContentURI.URIDataManager.StorageConnection = connection;
                }
                else
                {
                    //web server on localhost
                    if (ContentURI.URIDataManager.DefaultRootWebStoragePath.Contains("localhost"))
                    {
                        //visual studio debugging uses 
                        connection = Configuration["ConnectionStrings:ReleaseConnection"];
                        ContentURI.URIDataManager.DefaultConnection = connection;
                        //not used on web servers
                        connection = Configuration["ConnectionStrings:ReleaseStorageConnection"];
                        ContentURI.URIDataManager.StorageConnection = connection;
                    }
                    else
                    {
                        //azure debug using localhost uses debug paths
                        path = Configuration["DebugPaths:DefaultRootFullFilePath"];
                        ContentURI.URIDataManager.DefaultRootFullFilePath = path;
                        //test the use of internet urls to 
                        path = Configuration["DebugPaths:DefaultRootWebStoragePath"];
                        ContentURI.URIDataManager.DefaultRootWebStoragePath = path;
                        path = Configuration["DebugPaths:DefaultWebDomain"];
                        ContentURI.URIDataManager.DefaultWebDomain = path;
                        path = Configuration["DebugPaths:ExtensionsPath"];
                        ContentURI.URIDataManager.ExtensionsPath = path;
                        //connections
                        connection = Configuration["DevTreksLocalConnection"];
                        ContentURI.URIDataManager.DefaultConnection = connection;
                        connection = Configuration["DevTreksLocalStorage"];
                        ContentURI.URIDataManager.StorageConnection = connection;
                    }
                }
                path = Configuration["Site:FileSizeValidation"];
                ContentURI.URIDataManager.FileSizeValidation = path;
                path = Configuration["Site:FileSizeDBStorageValidation"];
                ContentURI.URIDataManager.FileSizeDBStorageValidation = path;
                path = Configuration["Site:PageSize"];
                ContentURI.URIDataManager.PageSize
                = DevTreks.Data.Helpers.GeneralHelpers.ConvertStringToInt(path);
                path = Configuration["Site:PageSizeEdits"];
                ContentURI.URIDataManager.PageSizeEdits = path;
                path = Configuration["Site:RExecutable"];
                ContentURI.URIDataManager.RExecutable = path;
                path = Configuration["Site:PyExecutable"];
                ContentURI.URIDataManager.PyExecutable = path;
                path = Configuration["Site:HostFeeRate"];
                ContentURI.URIDataManager.HostFeeRate = path;
                path = Configuration["URINames:ResourceURIName"];
                ContentURI.URIDataManager.ResourceURIName = path;
                path = Configuration["URINames:ContentURIName"];
                ContentURI.URIDataManager.ContentURIName = path;
                path = Configuration["URINames:TempDocsURIName"];
                ContentURI.URIDataManager.TempDocsURIName = path;
            });
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //refer to the services.AddLocalization url
            var supportedCultures = new[]
            {
                new CultureInfo("en"),
                new CultureInfo("es")
             };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en"),
                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,
                // UI strings that have been localized.
                SupportedUICultures = supportedCultures
            });



            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                // For more details on creating database during deployment see http://go.microsoft.com/fwlink/?LinkID=615859
                try
                {
                    using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                        .CreateScope())
                    {
                        serviceScope.ServiceProvider.GetService<ApplicationDbContext>()
                             .Database.Migrate();
                    }
                }
                catch { }
            }
            //rc2 moved to Program.cs and webconfig
            //app.UseIISPlatformHandler(options => options.AuthenticationDescriptions.Clear());

            app.UseStaticFiles();

            app.UseIdentity();

            // To configure external authentication please see http://go.microsoft.com/fwlink/?LinkID=532715
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                //route name
                name: "Default",
                //{*contenturipattern} is a variable route
                template: "{controller=Home}/{action=Index}/{*contenturipattern}"
                );
            });
        }
    }
}
