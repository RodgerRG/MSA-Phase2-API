using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;
using Web_API.Models;
using Web_API.Data;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.KeyVault;
using Web_API.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Web_API
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
            services.AddCors();

            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            //Try loading the secrets in from secrets.json
            var databaseConfig = Configuration.GetSection("Database").Get<DatabaseSettings>();
            string connection = "";
            if (databaseConfig != null)
            {
                connection = databaseConfig.ConnectionString;
            } else
            {
                //We aren't hosted locally, use the secret on the remote key vault
                connection = Configuration["Database-Connection-String"];
            }

            services.AddDbContext<ApiContext>(options => {
                options.UseSqlServer(connection);
                options.EnableSensitiveDataLogging(true);
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Remily Webserver",
                    Version = "v1",
                });
            });

            services.AddIdentity<User, IdentityRole<int>>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddEntityFrameworkStores<ApiContext>()
            .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "Authentication-Token";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                // ReturnUrlParameter requires 
                //using Microsoft.AspNetCore.Authentication.Cookies;
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
                options.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = e =>
                    {
                        e.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        return Task.FromResult(0);
                    },
                    OnRedirectToAccessDenied = e =>
                    {
                        e.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        return Task.FromResult(0);
                    }
                };
            });

            services.AddAuthentication(x => {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtSecret"])),
                        ValidIssuer = Configuration["JwtIssuer"],
                        ValidAudience = Configuration["JwtAudience"],
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                })
                .AddFacebook(options =>
                {
                    var facebookSettings = Configuration.GetSection("Facebook").Get<FacebookSettings>();

                    string AppId = "";
                    string AppSecret = "";
                    if (facebookSettings != null)
                    {
                        AppId = facebookSettings.AppId;
                        AppSecret = facebookSettings.AppSecret;
                    }
                    else
                    {
                        AppId = Configuration["Facebook-App-Id"];
                        AppSecret = Configuration["Facebook-App-Secret"];
                    }


                    options.AppId = AppId;
                    options.AppSecret = AppSecret;
                    options.CallbackPath = "/signin-facebook";
                    options.Validate();
                })
                .AddGoogle(options =>
                {
                var googleSettings = Configuration.GetSection("Google").Get<GoogleSettings>();

                string AppId = "";
                string AppSecret = "";
                if (googleSettings != null)
                {
                    AppId = googleSettings.ClientId;
                    AppSecret = googleSettings.ClientSecret;
                }
                else
                {
                    AppId = Configuration["Google-Client-Id"];
                    AppSecret = Configuration["Google-Client-Secret"];
                }


                options.ClientId = AppId;
                options.ClientSecret = AppSecret;
                options.CallbackPath = "/signin-google";
                options.Validate();
                });


            services.Configure<PasswordHasherOptions>(option =>
            {
                option.IterationCount = 1778;
            });

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = false;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins",
                    builder =>
                    {
                        builder.WithOrigins("http//localhost:3000")
                        .AllowCredentials();
                    });
            });

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

           // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(options => 
            {
                options.WithOrigins("localhost:3000", "https://phase2-app.azurewebsites.net")
                    .AllowCredentials()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .Build();
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<SignalRHub>("/hub");
            });

            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Remily API 1.1");
                c.RoutePrefix = string.Empty; // launch swagger from root
            });
        }
    }
}
