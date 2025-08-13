using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.SwaggerGen;

public class EmptyStringSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema.Type == "string" && schema.Format != "date-time")
        {
            schema.Example = new OpenApiString("");
        }
       
        if (schema.Properties != null)
        {
            foreach (var property in schema.Properties.Values)
            {
                if (property.Type == "string" && property.Format != "date-time")
                {
                    property.Example = new OpenApiString("");
                }
            }
        }
    }
}