using System;
using Dr_Hesabi.Classes.Class;
using Dr_Hesabi.Classes.Interface;
using Dr_Hesabi.Classes.Service;
using Dr_Hesabi.DataLayers.Context;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebMarkupMin.AspNetCore3;

namespace Dr_Hesabi
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
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/LogOut";
                options.ExpireTimeSpan = TimeSpan.FromHours(120);
                options.SlidingExpiration = true;
            });
            services.AddDbContext<DataBaseContext>(option =>
            {
                option.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddControllersWithViews();
            services.AddTransient<IAccount, AccountService>();
            services.AddTransient<IHome, HomeService>();
            services.AddTransient<IVisitDocument, VisitsDocumentService>();
            services.AddTransient<DataBaseContext>();
            services.AddTransient<IViewComponents, ViewComponentsService>();
            services.AddTransient<IPanel, PanelService>();
            services.AddTransient<ISetting, SettingService>();
            services.AddTransient<IConnections, ConnectionsService>();
            services.AddTransient<ISurveys, SurveysService>();
            services.AddTransient<IProfile, ProfileService>();
            services.AddTransient<ITests, TestsService>();
            services.AddTransient<RenderToString.IViewRenderService, RenderToString.ViewRenderService>();
            services.AddTransient<IContents, ContentsService>();
            services.AddMvc(option => option.EnableEndpointRouting = false);
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new RequireHttpsAttribute());
            });
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
            services.AddWebMarkupMin(options =>
                {
                    options.AllowCompressionInDevelopmentEnvironment = false;
                    options.AllowMinificationInDevelopmentEnvironment = false;
                })
                .AddHtmlMinification()
                .AddHttpCompression();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            var Rewriter = new RewriteOptions().AddRedirectToHttps();
            app.UseRewriter(Rewriter);
            app.UseAuthentication();
            app.UseWebMarkupMin();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStatusCodePagesWithReExecute("/Home/HttpNotFound");
            app.UseMvcWithDefaultRoute();
            app.UseRouting();
            app.UseMvc();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapAreaControllerRoute(
                //    name: "Admin",
                //    areaName: "Admin",
                //    pattern: "Admin/{controller=Home}/{action=Index}/{id?}"
                //);
                //endpoints.MapAreaControllerRoute(
                //    name: "Teacher",
                //    areaName: "Teacher",
                //    pattern: "Teacher/{controller=Home}/{action=Index}/{id?}"
                //);
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                    );
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                    );
            });
        }
    }
}
