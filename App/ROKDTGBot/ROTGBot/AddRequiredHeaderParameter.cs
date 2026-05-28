using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ROTGBot
{
    public class AddRequiredHeaderParameter : IOperationFilter
    {
        private const string AuthorizationName = "Authorization";
        private const ParameterLocation ParameterLocationIn = ParameterLocation.Header;
        private const string AuthorizationDescription = "access token";
        private const string OpenApiSchemaType = "string";
        private const bool OpenApiParameterRequired = true;
        private const string OpenApiSchemaDefault = "Bearer ";

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= [];
            operation.Parameters.Add(CreateOpenApiParameter());
        }

        private static OpenApiParameter CreateOpenApiParameter() => new()
        {
            Name = AuthorizationName,
            In = ParameterLocationIn,
            Description = AuthorizationDescription,
            Required = OpenApiParameterRequired,
            Schema = CreateSchema()
        };

        private static OpenApiSchema CreateSchema() => new()
        {
            Type = OpenApiSchemaType,
            Default = new OpenApiString(OpenApiSchemaDefault)
        };
    }
}
