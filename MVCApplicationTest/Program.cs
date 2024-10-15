using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MVCApplicationTest.BLL.Interfaces;
using MVCApplicationTest.BLL.Repositories;
using MVCApplicationTest.DAL.Contexts;
using MVCApplicationTest.DAL.Models;
using MVCApplicationTestPL.MapperProfiles;

namespace MVCApplicationTestPL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services that allow DP
            //Add MVC service
            builder.Services.AddControllersWithViews();

            //Add DbContext Service
            builder.Services.AddDbContext<MVCApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });


            //Add Repository Services
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            //services.AddScoped<IDepartmentRepository,DepartmentRepository>();
            //services.AddScoped<IEmployeeRepository,EmployeeRepository>();


            //Add Profiler Services
            builder.Services.AddAutoMapper(m => m.AddProfile(new DepartmentProfile()));
            builder.Services.AddAutoMapper(m => m.AddProfile(new EmployeeProfile()));
            builder.Services.AddAutoMapper(m => m.AddProfile(new UserProfile()));
            builder.Services.AddAutoMapper(m => m.AddProfile(new RoleProfile()));

            //Add Identty Services
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(option =>
                {
                    option.LoginPath = "Account/Login";
                    option.AccessDeniedPath = "Home/Error";
                });
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(option =>
            {
                option.Password.RequiredLength = 4;
                option.Password.RequireUppercase = true;
            })
                .AddEntityFrameworkStores<MVCApplicationDbContext>()
                .AddDefaultTokenProviders();
            //services.AddScoped<UserManager<ApplicationUser>>();
            //services.AddScoped<SignInManager<ApplicationUser>>();
            //services.AddScoped<RoleManager<ApplicationUser>>();
            #endregion

            var app = builder.Build();

            #region Configure Http Request PipeLine
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
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
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            #endregion
            app.Run();
        }

    }


}

