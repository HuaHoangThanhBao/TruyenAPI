using DataAccessLayer;
using CoreLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using EmailService;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using System;
using CoreLibrary.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http;
using LoggerService;
using CoreLibrary.Helpers;
using API.Service;

namespace API
{
    public static class ServiceExtensions
    {
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config["ConnectionStrings:default"];
            services.AddDbContext<RepositoryContext>(o => o.UseSqlServer(connectionString));
        }

        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            services.AddTransient<ITokenService, TokenService>();
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        }

        public static void ConfigureLoop(this IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(o => o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

        public static void ConfigureServices(this IServiceCollection services, IConfiguration Configuration)
        {
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowOrigin",
            //        builder => builder.WithOrigins("http://localhost:4000")
            //                    .WithMethods("GET").WithMethods("POST").WithMethods("PUT").WithMethods("DELETE"));
            //});

            services.AddSingleton<ILoggerManager, LoggerManager>();

            services.AddIdentity<ApplicationUser, IdentityRole>(opt =>
            {
                opt.Password.RequiredLength = Data.PassRequiredMinLength;
                opt.Password.RequireDigit = false;

                opt.User.RequireUniqueEmail = true;

                opt.Lockout.AllowedForNewUsers = true;
                opt.Lockout.DefaultLockoutTimeSpan = Data.LockOutAccountTime;
                opt.Lockout.MaxFailedAccessAttempts = Data.MaxFailedAccessAttempts;
            })
             .AddEntityFrameworkStores<RepositoryContext>()
             .AddDefaultTokenProviders();

            services.AddScoped<JwtHandler>();

            var emailConfig = Configuration
                .GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);
            services.AddScoped<IEmailSender, EmailSender>();

            services.Configure<FormOptions>(o => {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin", builder => builder.AllowAnyOrigin()
                .AllowAnyHeader().AllowAnyMethod().WithExposedHeaders("X-Pagination"));
            });

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = Configuration[$"{NamePars.JwtSettings}:{NamePars.ValidIssuer}"],
                    ValidAudience = Configuration[$"{NamePars.JwtSettings}:{NamePars.ValidAudience}"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration[$"{NamePars.JwtSettings}:{NamePars.SecurityKeys}"]))
                };
            });
            services.AddControllers();
        }
    }
}
