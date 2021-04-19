using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MotivateMe.Api.MotivateMeContext;
using MotivateMe.Api.MotivateMeContext.Jwt;
using MotivateMe.Api.Services;
using MotivateMe.Core.DIConfiguration;

namespace MotivateMe.Api
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            ConfigureDbConnectionAndAuth(services);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MotivateMe.Api", Version = "v2" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT containing userid claim",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                });
                var security =
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Id = "Bearer",
                                    Type = ReferenceType.SecurityScheme
                                },
                                UnresolvedReference = true
                            },
                            new List<string>()
                        }
                    };
                c.AddSecurityRequirement(security);
            });

            services.ConfigureFormCore(Configuration);

            //Getting user info for the current request
            services.AddTransient<UserManager<ApplicationUser>>();
            services.AddHttpContextAccessor();
            services.AddScoped<IUserIdResolver, UserIdResolver>();
            services.AddScoped<IUserResolver, UserResolver>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MotivateMe.Api v1"));
            }

            app.UseHttpsRedirection()

            .UseRouting()

            .UseAuthentication()

            .UseAuthorization()

            .UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigureDbConnectionAndAuth(IServiceCollection services)
        {
            //This will read the connection string we put inside appsettings.json and initialize our web Application to use the SQL Server Database
            string connectionString = Configuration.GetConnectionString("default");
            services.AddDbContext<AppDbContext>(c => c.UseSqlServer(connectionString));
            //This will allow us to use Identity functionality. This class is bound with ASP.NET Core AppDBContext Class, which handles our Database
            services.AddIdentityCore<ApplicationUser>(options => options.User.RequireUniqueEmail = true)
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddDefaultTokenProviders();

            //todo: fully unuderstand why this is used and not the SecurityStamp property in application user
            services.Configure<SecurityStampValidatorOptions>(options => options.ValidationInterval = TimeSpan.FromSeconds(10));

            services.Configure<JwtSettings>(Configuration.GetSection("Jwt"));

            // Adding Authentication  
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)

            // Adding Jwt Bearer  
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["Jwt:Audience"],
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Secret"])),
                    ClockSkew = TimeSpan.Zero
                };
            });
        }
    }
}
