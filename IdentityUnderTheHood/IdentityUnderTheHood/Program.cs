using IdentityUnderTheHood.Authorization;
using IdentityUnderTheHood.Pages.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace IdentityUnderTheHood
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthentication("MyCookieAuth")
                .AddCookie("MyCookieAuth", options =>
                {
                    options.Cookie.Name = "MyCookieAuth";
                    options.ExpireTimeSpan = TimeSpan.FromSeconds(20);
                    //options.LoginPath = "/account1/login";
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("MustBelongToHRDepartment", policy => policy.RequireClaim("Department", "HR"));
                options.AddPolicy("MustBeAdmin", policy => policy.RequireClaim("Admin"));
                options.AddPolicy("MustBelongToHRManagent", policy => policy
                    .RequireClaim("Department", "HR")
                    .RequireClaim("Management")
                    .Requirements.Add(new HRManagerProbationRequirement(3)));
            });

            builder.Services.AddSession(options =>
            { 
                options.Cookie.HttpOnly = true;
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.IsEssential = true;
            });
            
            builder.Services.AddRazorPages();
            builder.Services.AddSingleton<IAuthorizationHandler, HRManagerProbationRequirementHandler>();

            builder.Services.AddHttpClient("OurWebAPI", client => 
            {
                client.BaseAddress = new Uri("https://localhost:7108/");
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
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
            app.UseSession();

            app.MapRazorPages();

            app.Run();
        }
    }
}