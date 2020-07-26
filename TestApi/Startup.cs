using IdentityServer4.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace TestApi
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
            services.AddControllers();

            //services.AddDbContextPool<ApplicationDbContext>(opt =>
            //{
            //    opt.UseSqlServer(Configuration.GetConnectionString("SqlConnection"));
            //    opt.EnableSensitiveDataLogging();
            //    opt.EnableServiceProviderCaching();
            //});

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("SqlConnection")));

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentityServer(opt =>
            {

            }).AddApiAuthorization<IdentityUser, ApplicationDbContext>(opt => {
                opt.SigningCredential = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding
                        .UTF8
                        .GetBytes("my_not_very_strong_secret_key_for_encryption")),
                    SecurityAlgorithms.HmacSha256Signature);
                opt.Clients.Add(new Client
                {
                     
                });
            });

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme =
                    JwtBearerDefaults.AuthenticationScheme;

                opt.DefaultScheme =
                    JwtBearerDefaults.AuthenticationScheme;

                opt.DefaultChallengeScheme =
                    JwtBearerDefaults.AuthenticationScheme;
            }).AddIdentityServerJwt().AddJwtBearer(opt =>
            {
                var secretKey = Encoding
                        .UTF8
                        .GetBytes("my_not_very_strong_secret_key_for_encryption");

                var vp = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.Zero,
                    RequireSignedTokens = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey =
                           new SymmetricSecurityKey(secretKey),
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ValidateAudience = true,
                    ValidAudience = "TestApi",
                    ValidIssuer = "TestApi",
                    ValidateIssuer = true
                };

                opt.RequireHttpsMetadata = false;
                opt.SaveToken = true;
                opt.TokenValidationParameters = vp;
            });

            services.AddScoped<IJwtService, JwtService>();



            services.AddAuthorization();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
