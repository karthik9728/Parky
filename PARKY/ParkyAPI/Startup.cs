using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ParkyAPI.Data;
using ParkyAPI.Repository;
using ParkyAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ParkyAPI.Mapper;
using System.Reflection;
using System.IO;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using ParkyAPI.SwaggerOptions;

namespace ParkyAPI
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
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            #region
            services.AddScoped<INationalParkRepository, NationalParkRepository>();
            services.AddScoped<ITrailRepository, TrailRepository>();
            #endregion

            services.AddAutoMapper(typeof(AutoMappings));

            #region Version Control

            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
            });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigSwaggerOptions>();
            services.AddSwaggerGen();
            #endregion

            #region Swagger
            //services.AddSwaggerGen(options =>
            //{
            //    options.SwaggerDoc("ParkyOpenAPISpec", new Microsoft.OpenApi.Models.OpenApiInfo()
            //    {
            //        Title = "Parky API",
            //        Version = "1",
            //        Description = "Project Parky API",
            //        Contact = new Microsoft.OpenApi.Models.OpenApiContact()
            //        {
            //            Email = "itzmekarthik97@gmail.com",
            //            Name = "Karthik",
            //            Url = new Uri("https://www.google.com/")
            //        },
            //        License = new Microsoft.OpenApi.Models.OpenApiLicense()
            //        {
            //            Name = "MIT License",
            //            Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
            //        }

            //    });

            //    #region Multiple Swagger Doc
            //    //options.SwaggerDoc("ParkyOpenAPISpecTrail", new Microsoft.OpenApi.Models.OpenApiInfo()
            //    //{
            //    //    Title = "Parky API (Trail)",
            //    //    Version = "1",
            //    //    Description = "Project Parky API Trail",
            //    //    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
            //    //    {
            //    //        Email = "itzmekarthik97@gmail.com",
            //    //        Name = "Karthik",
            //    //        Url = new Uri("https://www.google.com/")
            //    //    },
            //    //    License = new Microsoft.OpenApi.Models.OpenApiLicense()
            //    //    {
            //    //        Name = "MIT License",
            //    //        Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
            //    //    }

            //    //});
            //    #endregion

            //    var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            //    var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
            //    options.IncludeXmlComments(xmlCommentsFullPath);
            //});

            #endregion
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                foreach (var desc in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json",desc.GroupName.ToUpperInvariant());
                }
                options.RoutePrefix = "";
            });

            //app.UseSwaggerUI(options =>
            //{
            //    options.SwaggerEndpoint("/swagger/ParkyOpenAPISpec/swagger.json", "Parky API NP");
            //    //options.SwaggerEndpoint("/swagger/ParkyOpenAPISpecTrail/swagger.json", "Parky API Trail");
            //    options.RoutePrefix = "";
            //});

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
