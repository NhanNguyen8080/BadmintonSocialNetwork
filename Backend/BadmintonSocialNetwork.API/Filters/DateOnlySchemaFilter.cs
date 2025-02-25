using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BadmintonSocialNetwork.API.Filters
{
    public class DateOnlySchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(DateOnly))
            {
                schema.Type = "string";
                schema.Format = "date"; // ISO 8601 format: "yyyy-MM-dd"
                schema.Example = new OpenApiString("2002-04-13"); // Example value
            }
        }
    }
}
