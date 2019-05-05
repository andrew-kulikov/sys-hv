using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using SysHv.Server.Configuration;
using SysHv.Server.DAL;
using SysHv.Server.DAL.Models;
using SysHv.Server.Helpers;
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

            services.AddCors(options => options.AddPolicy("CorsPolicy",
                builder =>
                {
                    builder.AllowAnyMethod().AllowAnyHeader()
                        .WithOrigins("http://localhost:3000", "https://localhost:3000",
                            "https://syshv.azurewebsites.net", "http://syshv.azurewebsites.net")
                        .AllowCredentials();
                }));

            services.AddSignalR();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddJsonOptions(options => {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            }); ;

            //services.AddHostedService<ReceiverService>();

            ConfigureTokenAuthorization(services);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Info
                    {
                        Title = "SysHv Server API", Version = "v1"
                    }); 
                
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<ISensorService, SensorService>();
            services.AddSingleton<IHostedService, ReceiverService>();
            services.AddSingleton<IConfigurationHelper, ConfigurationHelper>();
            services.AddTransient<IHostedServiceAccessor<ReceiverService>, HostedServiceAccessor<ReceiverService>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseHttpsRedirection();
            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseSignalR(routes => { routes.MapHub<MonitoringHub>("/monitoringHub"); });

            

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SysHv Server API v1");
                c.RoutePrefix = string.Empty;
            });

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
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(0),
                        ValidIssuer = _tokenOptions.Issuer,
                        ValidAudience = _tokenOptions.Audience
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            // If the request is for our hub...
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/monitoringHub")))
                            {
                                // Read the token out of the query string
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
        }
    }
}