using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Categories;

public static class CategoryEndpoints
{
    public static void MapEndpoints(IEndpointRouteBuilder app)
    {
        ArchiveCategory.MapEndpoint(app);
        CreateCategory.MapEndpoint(app);
        GetCategories.MapEndpoint(app);
        GetCategories.MapEndpoint(app);
        UpdateCategory.MapEndpoint(app);
    }
}
