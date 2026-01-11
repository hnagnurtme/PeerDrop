using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PeerDrop.API.Filters;

public class RemoveVersionParameterFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var versionParameter = operation.Parameters.FirstOrDefault(p => 
            p.Name is "version" or "api-version");
        
        if (versionParameter != null)
        {
            operation.Parameters.Remove(versionParameter);
        }
    }
}
