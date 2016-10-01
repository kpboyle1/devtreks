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
    ///Date:		2016, September
    ///References:	www.devtreks.org
    /// </summary>
    public class Startup
    {
        public IConfigurationRoot Configuration { get; set; }
        //set config and httpcontext settings
        DevTreks.Data.ContentURI ContentURI { get; set; }
        private static string DefaultRootFullFilePath { get; set; }

        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            //configuring user secrets 
            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }
            else
            {
                //not used
            }
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();

            ContentURI = new DevTreks.Data.ContentURI();
            //set the webroot full file path: C:\\DevTreks\\src\\DevTreks\\wwwroot
            DefaultRootFullFilePath = string.Concat(env.WebRootPath, "\\");
            //appPath is one path up from webroot: C:\\DevTreks\\src\\DevTreks
            //string sCheck = env.ContentRootPath;
            //azure web app: Use the path D:\home\site\wwwroot to refer to your app's root directory.
        }

        private static string FixSecretConnection(string secretConnection)
        {
            //allow this to generate a null exception if secretConnection
            //can't be found (in rc2 release)
            string sSecretConnection = secretConnection.Replace("\\\\", "\\");
            return sSecretConnection;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            //comment out if secrets are not being used
            //work around for testing user secrets (adds 2 extra slashes)
            //string sSecretConnection = Configuration["DevTreksLocalDb"];
            //sSecretConnection = FixSecretConnection(sSecretConnection);
            //// Add framework services for ASPNET Identity
            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(sSecretConnection));

            //comment out if secrets are being used
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration["ConnectionStrings:DebugConnection"]));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();

            //refer to https://docs.asp.net/en/1.0.0-rc2/fundamentals/localization.html
            //apparently, this is for Razor -not compiled binary use
            services.AddLocalization(options => options.ResourcesPath = "..\\DevTreks.Exceptions");
            services.AddLocalization(options => options.ResourcesPath = "..\\DevTreks.Resources");
            

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            services.AddOptions();
            //this will be passed to Controller constructor and used to 
            //construct services and repositories 
            //inject config settings in controller and pass to views and repositories
            services.Configure<DevTreks.Data.ContentURI>(ContentURI =>
            {
                //azure is debugged by commenting in and out the azure for localhost:5000 appsettings
                string connection = string.Empty;
                string path = string.Empty;
                //comment out if secrets are not being used
                //connection = sSecretConnection;
                //comment out if secrets are being used
                connection = Configuration["ConnectionStrings:DebugConnection"];
                ContentURI.URIDataManager.DefaultConnection = connection;
                //comment out if secrets are not being used
                //connection = Configuration["DevTreksLocalStorage"];
                //comment out if secrets are being used
                connection = Configuration["ConnectionStrings:DebugStorageConnection"];
                ContentURI.URIDataManager.StorageConnection = connection;

                ContentURI.URIDataManager.DefaultRootFullFilePath
                    = DefaultRootFullFilePath;
                path = Configuration["DebugPaths:DefaultRootWebStoragePath"];
                ContentURI.URIDataManager.DefaultRootWebStoragePath = path;
                path = Configuration["DebugPaths:DefaultWebDomain"];
                ContentURI.URIDataManager.DefaultWebDomain = path;
                ContentURI.URIDataManager.ExtensionsPath
                    = string.Concat(ContentURI.URIDataManager.DefaultRootFullFilePath, "Extensions\\");
                //2.0.2 added appsetting to eliminate calls to GetPlatformType()
                ContentURI.URIDataManager.PlatformType 
                    = Data.Helpers.FileStorageIO.GetPlatformType(ContentURI.URIDataManager.DefaultRootWebStoragePath);
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
                path = Configuration["Site:JuliaExecutable"];
                ContentURI.URIDataManager.JuliaExecutable = path;
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
            //comment out if secrets are not being used
            //work around for testing user secrets (adds 2 extra slashes)
            //string sSecretConnection = Configuration["DevTreksLocalDb"];
            //sSecretConnection = FixSecretConnection(sSecretConnection);
            //// Add framework services.
            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(sSecretConnection));
            //comment out if secrets are being used
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration["ConnectionStrings:ReleaseConnection"]));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();

            //refer to https://docs.asp.net/en/1.0.0-rc2/fundamentals/localization.html
            //note that DevTreks uses compiled binaries -not Razor views
            services.AddLocalization(options => options.ResourcesPath = "..\\DevTreks.Resources");
            services.AddLocalization(options => options.ResourcesPath = "..\\DevTreks.Exceptions");

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
                //comment out if secrets are not being used
                //connection = sSecretConnection;
                //comment out if secrets are being used
                connection = Configuration["ConnectionStrings:ReleaseConnection"];
                ContentURI.URIDataManager.DefaultConnection = connection;
                //comment out if secrets are not being used
                //connection = Configuration["DevTreksLocalStorage"];
                //comment out if secrets are being used
                connection = Configuration["ConnectionStrings:ReleaseStorageConnection"];
                ContentURI.URIDataManager.StorageConnection = connection;

                ContentURI.URIDataManager.DefaultRootFullFilePath
                    = DefaultRootFullFilePath;
                //getplatformtype expects this to be string empty to debug azure on localhost
                path = Configuration["ReleasePaths:DefaultRootWebStoragePath"];
                ContentURI.URIDataManager.DefaultRootWebStoragePath = path;
                path = Configuration["ReleasePaths:DefaultWebDomain"];
                ContentURI.URIDataManager.DefaultWebDomain = path;
                ContentURI.URIDataManager.ExtensionsPath
                    = string.Concat(ContentURI.URIDataManager.DefaultRootFullFilePath, "Extensions\\");
                //2.0.2 added appsetting to eliminate calls to GetPlatformType()
                ContentURI.URIDataManager.PlatformType
                    = Data.Helpers.FileStorageIO.GetPlatformType(ContentURI.URIDataManager.DefaultRootWebStoragePath);
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
                path = Configuration["Site:JuliaExecutable"];
                ContentURI.URIDataManager.JuliaExecutable = path;
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
                //this is also the syntax used to hold the resource dlls
                //in the publish path
                new CultureInfo("en"),
                //very limited testing so far
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
                //errors are displayed on html content pages -don't use a separate page
                //app.UseExceptionHandler("/Home/Error");

                //code for creating aspnet identify tables in rc2 -commented out after tables were created
                // For more details on creating database during deployment see http://go.microsoft.com/fwlink/?LinkID=615859
                //try
                //{
                //    //existing aspnet identify tables should be deleted
                //    //this code will generate new identify tables for the new core 1 technologies
                //    //after the tables are created in the database, register existing users, and 
                //    //add the AspNetUsers.Id primary key field to the Member.AspNetId foreign key.
                //    using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                //        .CreateScope())
                //    {
                //        serviceScope.ServiceProvider.GetService<ApplicationDbContext>()
                //             .Database.Migrate();
                //    }
                //}
                //catch { }
            }
            app.UseStaticFiles();

            app.UseIdentity();
            // To configure external authentication please see http://go.microsoft.com/fwlink/?LinkID=532715

            //MVC routes
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
