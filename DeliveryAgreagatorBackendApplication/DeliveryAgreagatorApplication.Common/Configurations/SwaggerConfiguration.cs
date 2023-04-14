using DeliveryAgreagatorApplication.Common.Schemas;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryAgreagatorApplication.Common.Configurations
{
    public static class SwaggerConfiguration
    {
        public static void AddSwagger(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                c.SchemaFilter<EnumSchemaFilter>();
                var basePath = AppContext.BaseDirectory;

                var xmlPath = Path.Combine(basePath, "DeliveryAgreagatorApplication.xml");
                c.IncludeXmlComments(xmlPath);               
            });

        }
    }
}
