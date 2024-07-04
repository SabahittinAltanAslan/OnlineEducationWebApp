using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineEducationWebApp.Data.Context;
using OnlineEducationWebApp.Data.Hubs;
using OnlineEducationWebApp.Data.Services;
using OnlineEducationWebApp.Interfaces;
using System.Net;
using System.Text;

namespace OnlineEducationWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddDbContext<ProjectContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped<IStudentService, StudentService>();
            builder.Services.AddScoped<ILessonService, LessonService>();
            builder.Services.AddScoped<ITeacherService, TeacherService>();
            builder.Services.AddScoped<IDocumentService, DocumentService>();
            builder.Services.AddScoped<IStudentLessonService, StudentLessonService>();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                  .AddCookie(opts =>
                  {
                      opts.Cookie.Name = $".appname.auth";
                      opts.AccessDeniedPath = "/access-denied";
                      opts.LoginPath = "/sign-in";
                      opts.SlidingExpiration = true;
                  })
                  .AddJwtBearer(options =>
                  {
                      options.RequireHttpsMetadata = false;
                      options.SaveToken = true;
                      options.TokenValidationParameters = new TokenValidationParameters
                      {
                          ValidateIssuer = false,
                          ValidateAudience = false,
                          ValidateLifetime = true,
                          ValidateIssuerSigningKey = true,
                          IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String("superSecretKey@345-superSecretKey@345_superSecretKey@345-superSecretKey@345?")),
                          ClockSkew = TimeSpan.FromSeconds(5)
                      };
                  });

            builder.Services.AddSession(options =>
            {
                options.Cookie.Name = $"appname.session"; // todo : deðiþtirin.
                options.IdleTimeout = TimeSpan.FromMinutes(180);
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddAuthorization();

            builder.Services.AddSignalR();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseSession();
            app.Use(async (context, next) =>
            {
                var token = context.Request.Cookies["AccessToken"];

                if (!string.IsNullOrEmpty(token) &&
                    !context.Request.Headers.ContainsKey("Authorization"))
                {
                    context.Request.Headers.Add("Authorization", "Bearer " + token);
                }

                await next();
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<LessonHub>("/lessonHub");
            });

            app.Run();
        }
    }
}