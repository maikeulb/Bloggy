using AutoMapper;
using Bloggy.API.Features.Profiles;
using Bloggy.API.Data
using Bloggy.API.Infrastructure;
using Bloggy.API.Infrastructure.Interfaces;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Bloggy.API
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
            services.AddMediatR();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

            services
                .AddEntityFrameworkSqlite()
                .AddDbContext<BloggyContext>();
            
            services.AddCors();

            services.AddMvc(opt =>
                {
                    opt.Conventions.Add(new GroupByApiRootConvention());
                    opt.Filters.Add(typeof(ValidatorActionFilter));
                })
                .AddJsonOptions(opt =>
                {
                    opt.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                })
                .AddFluentValidation(cfg => { cfg.RegisterValidatorsFromAssemblyContaining<Startup>(); });

            services.AddAutoMapper(GetType().Assembly);

            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();
            services.AddScoped<IProfileReader, ProfileReader>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            services.AddOptions();

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("somethinglongerforthisdumbalgorithmisrequired"));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var issuer = "issuer";
            var audience = "audience";

            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = issuer;
                options.Audience = audience;
                options.SigningCredentials = signingCredentials;
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingCredentials.Key,
                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = issuer,
                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = audience,
                // Validate the token expiry
                ValidateLifetime = true,
                // If you want to allow a certain amount of clock drift, set that here:
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => { options.TokenValidationParameters = tokenValidationParameters; })
                .AddJwtBearer("Token", options => { options.TokenValidationParameters = tokenValidationParameters; });
        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseCors(builder =>
                builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());

            app.UseMvc();
        }
    }
}
