using Core.Domain.Entities;
using Core.Domain.Exceptions;
using Core.Interfaces;

using FluentValidation;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace Core.Features.Products.Commands
{
  public class AddNewProduct : IEndpoint
  {

    #region Ruteo
    public void AddRoutes(IEndpointRouteBuilder app)
    {
      app.MapPost(ApiRoutes.Products.AddProduct, CreateProduct)
        .WithTags("Products");
    }
    internal async Task<IResult> CreateProduct(
      HttpContext context,
      IMediator mediator,
      AddNewProductCommand product)
    {
      var data = await mediator.Send(product);
      return Results.Ok(data);
    }
    #endregion

    #region Mediator
    public class AddNewProductCommand : IRequest<ProductEntity>
    {
      public int ProductId { get; set; }
      public string ProductName { get; set; } = "";
    }

    public class AddNewProductCommandHandler : IRequestHandler<AddNewProductCommand, ProductEntity>
    {

      private readonly IProductRepository<ProductEntity> _repo;
      private readonly IValidator<AddNewProductCommand> _validator;
      private readonly ILogger<AddNewProductCommand> _logger;

      public AddNewProductCommandHandler(
        IProductRepository<ProductEntity> repo,
        IValidator<AddNewProductCommand> validator,
        ILogger<AddNewProductCommand> logger
      )
      {
        _repo = repo;
        _validator = validator;
        _logger = logger;
      }
      public async Task<ProductEntity> Handle(AddNewProductCommand request, CancellationToken cancellationToken)
      {
        var data = new ProductEntity();
        try
        {
          var validaciones = _validator.Validate(request);
          if (!validaciones.IsValid)
          {
            _logger.LogError("No se pudo crear el producto, {product}", request);
            throw new InvalidProductException("Pelas, el producto no se puede agregar");
          }
          data = await _repo.CreateProduct(new ProductEntity { ProductId = request.ProductId, ProductName = request.ProductName });

          //return data;
        }
        catch (InvalidProductException ex)
        {
          _logger.LogError(ex, "{msg}, {param}", ex.Message, request);
        }
        return data;
      }
    }
    #endregion

    #region modelo
    #endregion

    #region mapeo
    #endregion

    #region Validaciones
    public class AddNewProductValidator : AbstractValidator<AddNewProductCommand>
    {
      public AddNewProductValidator()
      {
        RuleFor(p =>
          p.ProductId
        ).Equals(0);
        RuleFor(p => p.ProductName).NotEmpty();
      }
    }
    #endregion
  }
}
