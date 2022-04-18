using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Reflection;

namespace ParkyAPI.SwaggerOptions
{
    public class ConfigSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider Provider;

        public ConfigSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            Provider = provider;
        }
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var desc in Provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(desc.GroupName,new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = $"Parky API {desc.ApiVersion}",
                    Version = desc.ApiVersion.ToString(),
                });
            }

            var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
            options.IncludeXmlComments(xmlCommentsFullPath);
        }
    }
}
