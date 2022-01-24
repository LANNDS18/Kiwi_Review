using Kiwi_review.Interfaces.IServices;
using Kiwi_review.Interfaces.IUnitOfWork;
using Kiwi_review.Models;
using Kiwi_review.Models.Jwt;
using Kiwi_review.Repository;
using Kiwi_review.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Kiwi_review
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<KiwiReviewContext>(options => options.UseMySQL(Configuration["DBInfo:ConnectionString"]));
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddSession();

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));
            var jwtConfig = Configuration.GetSection(nameof(JwtConfig));
            services.Configure<JwtConfig>(Configuration.GetSection("JwtConfig"));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(configureOptions =>
                {
                    configureOptions.ClaimsIssuer = jwtConfig[nameof(JwtConfig.Issuer)];
                    configureOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtConfig[nameof(JwtConfig.Issuer)],
                        ValidateAudience = true,
                        ValidAudience = jwtConfig[nameof(JwtConfig.Audience)],
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            System.Text.Encoding.UTF8.GetBytes(jwtConfig[nameof(JwtConfig.IssuerSigningKey)]))
                    };
                }
            );
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<ICheckService, CheckService>();
            services.AddTransient<IHighlightService, HighlightService>();
            services.AddTransient<IReviewService, ReviewService>();
            services.AddTransient<ITopicService, TopicService>();
            services.AddTransient<IMovieService, MovieService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAnonymousUserService, AnonymousUserService>();

            services.AddTransient<IUnitOfWorkWrapper, UnitOfWorkWrapper>();

            services.AddSwaggerGen(swagger =>  
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo  
                {  
                    Version = "v1",  
                    Title = "Kiwi Web API",  
                    Description = "Authentication and Authorization in ASP.NET 6 with JWT and Swagger"  
                });
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()  
                {  
                    Name = "Authorization",  
                    Type = SecuritySchemeType.ApiKey,  
                    Scheme = "Bearer",  
                    BearerFormat = "JWT",  
                    In = ParameterLocation.Header,  
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6\"",  
                });  
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement  
                {  
                    {  
                        new OpenApiSecurityScheme  
                        {  
                            Reference = new OpenApiReference  
                            {  
                                Type = ReferenceType.SecurityScheme,  
                                Id = "Bearer"  
                            }  
                        },  
                        new string[] {}
                    }  
                });  
            });  
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseAuthentication();
            app.UseCors("MyPolicy");
            app.UseSwagger();  
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Kiwi Web API v1"));  
            app.UseCors("AllowAll");
            app.UseMvc();
        }
    }
}