/*
 * Caleb Edwards
 This class is used for Identity. It also provides email confirmation
 */

using System;
using Learning_Outcomes.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(Learning_Outcomes.Areas.Identity.IdentityHostingStartup))]
namespace Learning_Outcomes.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<IdentityContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("IdentityContextConnection")));

/*                services.AddDefaultIdentity<IdentityUser>()
                    .AddRoles<IdentityRole>();*/
                    //.AddEntityFrameworkStores<IdentityContext>();


                // Email Confirmation
                services.AddDefaultIdentity<IdentityUser>(config =>
                {
                    config.SignIn.RequireConfirmedEmail = true;
                })
                    .AddRoles<IdentityRole>()
                    .AddDefaultUI(UIFramework.Bootstrap4)
                    .AddEntityFrameworkStores<IdentityContext>();

                services.AddTransient<IEmailSender, EmailSender>();
                services.AddTransient<UserManager<IdentityUser>>();
                services.AddTransient<IdentityContext>();

            });
        }
    }
}