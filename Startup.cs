using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OAuthApp.Models;

using Microsoft.EntityFrameworkCore;
using OAuthApp.OAuth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using IdentityServer4.Validation;
using IdentityServer4.Services;

namespace OAuthApp
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDbContext<OAuthDBContext>(options =>
                   options.UseSqlServer(Configuration.GetConnectionString("AuthDB")));


            // CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    });
            });

            // add oauth2
            services.AddIdentityServer()
                   .AddInMemoryApiResources(Config.GetApiResources())
                   .AddInMemoryClients(Config.GetClients())
                   .AddProfileService<ProfileService>()                          
                   .AddDeveloperSigningCredential(true, @"E:\Projects\Udemy\ASPNETCoreAPI\bootcamp\OAuthApp\OAuthApp\tempkey.rsa");


            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                                           JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme =
                                           JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.Authority = "http://localhost:29254";
                o.Audience = "OAuth2Demo.ReadAccess";               
                o.RequireHttpsMetadata = false;
            });

            services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
            services.AddTransient<IProfileService, ProfileService>();

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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseIdentityServer();
            
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
