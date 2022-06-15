using Core.Domain.Entities;
using Core.Interfaces;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Core.Features.Products.Queries
{
  public class GetProductByFilter : IEndpoint
  {
    public void AddRoutes(IEndpointRouteBuilder app)
    {
      app.MapPost(ApiRoutes.Products.GetFilteredProducts, GetFilteredProducts)
        .WithTags("Products")
        .Produces<List<ProductEntity>>()
        .AllowAnonymous()
        //.Produces(statusCode: StatusCodes.Status200OK)
        //.Produces(statusCode: StatusCodes.Status404NotFound)
        ;
    }

    internal async Task<IResult> GetFilteredProducts(
      HttpContext context,
      IMediator mediator,
      IHttpClientFactory HttpClientFactory,
      ProductEntity product)
    {
      var cliente = HttpClientFactory.CreateClient("API:Classic");

      var data = await mediator.Send(new GetFilteredProductsQuery(product));
      return Results.Ok(data);
      //return data;
    }

    #region mediator
    public class GetFilteredProductsQuery : IRequest<IEnumerable<ProductEntity>>
    {
      public GetFilteredProductsQuery(ProductEntity product)
      {
        Product = product;
      }

      public ProductEntity Product { get; }
    }

    //public class GetFilteredProductsQueryHandler : IRequestHandler<GetFilteredProductsQuery, IEnumerable<ProductEntity>>
    //{
    //  public async Task<IEnumerable<ProductEntity>> Handle(GetFilteredProductsQuery request, CancellationToken cancellationToken)
    //  {
    //    using var cliente = new HttpClient();

    //    var productNameFilter = $"or ProductName like '%{request.Product.ProductName}%'";
    //    if (string.IsNullOrEmpty(request.Product.ProductName))
    //    {
    //      productNameFilter = "";
    //    }
    //    var requestData = new
    //    {
    //      SP = $"select top 10 ProductId, ProductName from products where ProductId = {request.Product.ProductId} {productNameFilter}",
    //      Params = ""
    //    };

    //    var response = await cliente.PostAsync("https://demo.azzule.com/api/servicios.asmx/ExecuteSP", new StringContent( JsonSerializer.Serialize(requestData) ));
    //    var data = await response.Content.ReadAsStringAsync();
    //    var products = JsonSerializer.Deserialize<ClassicApiResponse<ProductEntity>>(data);
    //    return products.Data.Table;
    //  }
    //}


    public class GetFilteredProductsFromRepositoryQueryHandler : IRequestHandler<GetFilteredProductsQuery, IEnumerable<ProductEntity>>
    {
      private readonly IProductRepository<ProductEntity> _repo;

      public GetFilteredProductsFromRepositoryQueryHandler(IProductRepository<ProductEntity> repo)
      {
        _repo = repo;
      }
      public async Task<IEnumerable<ProductEntity>> Handle(GetFilteredProductsQuery request, CancellationToken cancellationToken)
      {
        var data = await _repo.GetFilteredProducts(request.Product);
        return data;
      }
    }
    #endregion

    #region Modelo

    #endregion

    #region Mapeo
    #endregion
  }
}
