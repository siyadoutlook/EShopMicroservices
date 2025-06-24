using BuildingBlocks.CQRS;
using Catalog.API.Models;
using Marten;

namespace Catalog.API.Products.GetProductByCategory;

public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;

public record GetProductByCategoryResult(IEnumerable<Product> Products);

internal class GetProductByCategoryHandler(IDocumentSession session, ILogger<GetProductByCategoryHandler> logger)
    : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
{
    public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("GetProductByCategoryHandler called with {@Query}", request);

        var products = await session.Query<Product>()
            .Where(p => p.Categories.Contains(request.Category))
            .ToListAsync(cancellationToken);

        return new GetProductByCategoryResult(products);
        ;
    }
}