using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DeliveryAgreagatorApplication.Common.Schemas
{
    public class EnumSchemaFilter : ISchemaFilter
    {
		public void Apply(OpenApiSchema schema, SchemaFilterContext context)
		{
			if (context.Type.IsEnum)
			{
				schema.Extensions.Add(
					"x-ms-enum",
					new OpenApiObject
					{
						["name"] = new OpenApiString(context.Type.Name),
						["modelAsString"] = new OpenApiBoolean(true)
					}
				);
				schema.Enum.Clear();
				Enum.GetNames(context.Type).ToList().ForEach(name => schema.Enum.Add(new OpenApiString(name)));
				schema.Type = "string";
			}
		}
	}
    
}
