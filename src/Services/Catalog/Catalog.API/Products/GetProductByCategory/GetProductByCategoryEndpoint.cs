using Catalog.API.Models;

namespace Catalog.API.Products.GetProductByCategory;

public record GetProductByCategoryResponse(IEnumerable<Product> products);

public class GetProductByCategoryEndpoint:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/category/{category}", async (string category, ISender sender) =>
        {
            var products = await sender.Send(new GetProductByCategoryQuery(category));
            var productsResponse = products.Adapt<GetProductByCategoryResponse>();
            return Results.Ok(productsResponse);
        })
        .WithName("GetProductByCategory")
        .Produces<GetProductByCategoryResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithSummary("Get Product By Category")
        .WithDescription("Get Product By Category");
    }
}