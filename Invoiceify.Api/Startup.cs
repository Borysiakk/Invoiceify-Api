using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Invoiceify.Infrastructure;
using Invoiceify.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace Invoiceify.Api
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
            services.AddPersistence();
            services.AddAuthorization();
            services.AddAuthentication(Configuration);
            services.AddInfrastructure(Configuration);
            services.AddControllers();
            services.AddControllersWithViews().AddNewtonsoftJson(option =>
            {
                option.SerializerSettings.DateFormatString = "yyyy-MM-dd";
                option.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            
            services.AddSwaggerGen(swagger =>
            {
                //This is to generate the Default UI of Swagger Documentation  
                swagger.SwaggerDoc("v1", new OpenApiInfo  
                {   
                    Version= "v1",   
                    Title = "JWT Token Authentication API",  
                    Description="ASP.NET Core 3.1 Web API" });  
                // To Enable authorization using Swagger (JWT)  
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()  
                {  
                    Name = "Authorization",  
                    Type = SecuritySchemeType.ApiKey,  
                    Scheme = "Bearer",  
                    BearerFormat = "JWT",  
                    In = ParameterLocation.Header,  
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",  
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
            app.UseCors(x=>x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials());
            
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Invoiceify.Api v1"));
            

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}