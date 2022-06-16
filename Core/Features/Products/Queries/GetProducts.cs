using AutoMapper;
using AutoMapper.QueryableExtensions;

using Core.Domain.Entities;
using Core.Interfaces;
using Core.Security;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

using System.Security.Claims;

namespace Core.Features.Products.Queries
{
  public class GetProducts : IEndpoint
  {
    #region Ruteo
    public void AddRoutes(IEndpointRouteBuilder app)
    {
      app.MapGet(ApiRoutes.Products.GetAll, GetAllProducts).WithTags("Products");
      //.RequireAuthorization("buyers:user");
    }
    [TokenCheck(new BuyersRoles[] { BuyersRoles.Admin, BuyersRoles.User })]
    internal async Task<IResult> GetAllProducts(HttpContext context, IMediator mediator, ILogger<GetProducts> logger)
    {
      var tokenData = ((IEnumerable<Claim>)context.Items["TokenData"]).ToList();
      
      var data = await mediator.Send(new GetAllProductsQuery());
      return Results.Ok(data);
    }

    #endregion

    #region mediator
    public class GetAllProductsQuery : IRequest<IEnumerable<GetAllProductsResponse>> { }

    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<GetAllProductsResponse>>
    {
      private readonly IMapper _mapper;
      private readonly IProductRepository<ProductEntity> _repo;

      public GetAllProductsQueryHandler(IMapper mapper, IProductRepository<ProductEntity> repo)
      {
        _mapper = mapper;
        _repo = repo;
      }
      public async Task<IEnumerable<GetAllProductsResponse>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
      {

        var data = await _repo.GetAllProducts();
        var response = data.AsQueryable().ProjectTo<GetAllProductsResponse>(_mapper.ConfigurationProvider).ToList();
        return response;
      }
    }
    #endregion

    #region Modelo
    public record GetAllProductsResponse
    {
      public int Id { get; set; }
      public string Name { get; set; } = "";
      public string FullName { get => $"{Id} - {Name}"; }
    }
    #endregion

    #region Mapeo
    public class GetProductsMappingProfile : Profile
    {
      public GetProductsMappingProfile() =>
        CreateMap<ProductEntity, GetAllProductsResponse>()
          .ForMember(dest => dest.Id, opts => opts.MapFrom(nameof(ProductEntity.ProductId)))
          .ForMember(dest => dest.Name, opts => opts.MapFrom(nameof(ProductEntity.ProductName)));
    }
    #endregion
  }
}
