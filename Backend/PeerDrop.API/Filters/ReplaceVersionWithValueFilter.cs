using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PeerDrop.API.Filters;

public class ReplaceVersionWithValueFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var paths = new OpenApiPaths();
        
        foreach (var path in swaggerDoc.Paths)
        {
            var newKey = path.Key.Replace("{version}", "1");
            paths.Add(newKey, path.Value);
        }
        
        swaggerDoc.Paths = paths;
    }
}
