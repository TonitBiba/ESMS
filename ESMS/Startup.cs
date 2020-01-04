using ESMS.Areas.Identity;
using ESMS.Data;
using ESMS.Data.Model;
using ESMS.General_Classes;
using ESMS.Security;
using ESMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ESMS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("esmsConnection")));

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, CustomClaismAuthentication>();

            services.AddTransient<IEmailSender, EmailSender>();

            services.AddRazorPages();

            services.AddMvc(
            //    options =>
            //{
            //    options.Filters.Add(new OnActionFilter());
            //}
            ).AddViewLocalization();

            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

            services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();

            services.AddSignalR();

            //Password policy
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 9;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
            });


        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            IList<CultureInfo> supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("sq-AL"),
                new CultureInfo("en-US"),
            };
            
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("sq-AL"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<NotificationHub>("/notificationHub");
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });

        }
    }
}
