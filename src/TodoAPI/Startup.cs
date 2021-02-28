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
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using TodoApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace TodoAPI
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
            var allowedOrigins = Configuration.GetValue<string>("AllowedOrigins")?.Split(",") ?? new string[0];
            // services.AddCors(opt => opt.AddPolicy("AllowEverything", builder => builder.AllowAnyOrigin()
            //                                                                            .AllowAnyMethod()
            //                                                                            .AllowAnyHeader()));
            services.AddCors(opt =>
            {
                opt.AddPolicy("nagInternal", builder => builder.WithOrigins(allowedOrigins)
                                                                .AllowAnyHeader()
                                                                .SetPreflightMaxAge(TimeSpan.FromMinutes(1)));
                opt.AddPolicy("PublicApi", builder => builder.AllowAnyOrigin().WithMethods("Get").WithHeaders("Content-Type"));
            });

            services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoList"));
            services.AddAutoMapper(Assembly.GetExecutingAssembly());


            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TodoAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TodoAPI v1"));
            }

            // app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("nagInternal"); // between Routing and UseAuthorization
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
