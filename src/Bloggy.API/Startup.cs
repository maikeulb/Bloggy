using System;
using System.Text;
using AutoMapper;
using Bloggy.API.Data;
using Bloggy.API.Infrastructure;
using Bloggy.API.Services;
using Bloggy.API.Services.Interfaces;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;

namespace Bloggy
{
    public class Startup
    {
        public Startup (IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices (IServiceCollection services)
        {
            services.AddDbContext<BloggyContext> (options =>
                options.UseNpgsql (Configuration.GetConnectionString ("BloggyApi")));

            services.AddSwaggerGen (options =>
            {
                options.SwaggerDoc ("v1", new Info { Title = "Bloggy API", Version = "v1" });
                options.CustomSchemaIds(y => y.FullName);
                options.DocInclusionPredicate((version, apiDescription) => true);
                options.TagActionsBy(y => y.GroupName);
            });

            services.AddMvc (opt =>
                {
                    opt.Conventions.Add (new GroupByApiRootConvention ());
                    opt.Filters.Add (typeof (ValidatorActionFilter));
                })
                .AddJsonOptions (opt =>
                {
                    opt.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                })
                .AddFluentValidation (cfg => { cfg.RegisterValidatorsFromAssemblyContaining<Startup> (); });

            services.AddAutoMapper (GetType ().Assembly);

            services.AddScoped<IPasswordHasher, PasswordHasher> ();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator> ();
            services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor> ();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor> ();

            services.AddOptions ();

            var signingKey = new SymmetricSecurityKey (Encoding.ASCII.GetBytes ("somethinglongerforthisdumbalgorithmisrequired"));
            var signingCredentials = new SigningCredentials (signingKey, SecurityAlgorithms.HmacSha256);
            var issuer = "issuer";
            var audience = "audience";

            services.Configure<JwtIssuerOptions> (options =>
            {
                options.Issuer = issuer;
                options.Audience = audience;
                options.SigningCredentials = signingCredentials;
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = false,
                IssuerSigningKey = signingCredentials.Key,
                ValidateIssuer = false,
                ValidIssuer = issuer,
                ValidateAudience = false,
                ValidAudience = audience,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication (JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer (options => { options.TokenValidationParameters = tokenValidationParameters; })
                .AddJwtBearer ("Token", options => { options.TokenValidationParameters = tokenValidationParameters; });

            /* services.AddCors (); */
            services.AddMediatR ();
            services.AddTransient (typeof (IPipelineBehavior<,>), typeof (ValidationPipelineBehavior<,>));
        }

        public void Configure (IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();

            /* app.UseCors (builder => */
            /*     builder */
            /*     .AllowAnyOrigin () */
            /*     .AllowAnyHeader () */
            /*     .AllowAnyMethod ()); */

            app.UseMvc ();

            app.UseSwagger(c =>
            {
                c.RouteTemplate = "swagger/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "Bloggy API V1");
            });
        }
    }
}
