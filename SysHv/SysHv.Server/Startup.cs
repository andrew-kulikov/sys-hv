using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SysHv.Server.Configuration;
using SysHv.Server.DAL;
using SysHv.Server.DAL.Models;
using SysHv.Server.HostedServices;
using SysHv.Server.Hubs;
using SysHv.Server.Services;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace SysHv.Server
{
    public class Startup
    {
        private const string TokenAudience = "ExampleAudience";
        private const string TokenIssuer = "ExampleIssuer";
        private TokenConfiguration _tokenOptions;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ServerDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("ServerDbConnectionString"),
                    assembly => assembly.MigrationsAssembly(typeof(ServerDbContext).Assembly.FullName));
            });

            services.AddDefaultIdentity<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ServerDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //services.AddHostedService<ReceiverService>();
            services.AddSignalR();

            ConfigureTokenAuthorization(services);

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<ISensorService, SensorService>();
            services.AddSingleton<IHostedService, ReceiverService>();
            services.AddTransient<IHostedServiceAccessor<ReceiverService>, HostedServiceAccessor<ReceiverService>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
            app.UseSignalR(routes =>
            {
                routes.MapHub<MonitoringHub>("/monitoringHub");
            });

            app.UseAuthentication();

            app.UseMvc();
        }

        private void ConfigureTokenAuthorization(IServiceCollection services)
        {
            var keyParams = new RSACryptoServiceProvider(2048).ExportParameters(true);

            var key = new RsaSecurityKey(keyParams);
            _tokenOptions = new TokenConfiguration
            {
                Audience = TokenAudience,
                Issuer = TokenIssuer,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256Signature)
            };
            services.AddSingleton(_tokenOptions);

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(0),
                        ValidIssuer = _tokenOptions.Issuer,
                        ValidAudience = _tokenOptions.Audience
                    };
                });
        }
    }
}
