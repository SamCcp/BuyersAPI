using Core.Domain.Entities;

using MediatR;

using Microsoft.Extensions.Logging;

namespace Core.Features.Products.Events
{
  internal class ProductCreatedEvent : INotification
  {
    public ProductEntity Product { get; set; }  
  }

  internal class ProductCreatedEventHandler : INotificationHandler<ProductCreatedEvent>
  {
    private readonly ILogger<ProductCreatedEventHandler> _logger;

    public ProductCreatedEventHandler(ILogger<ProductCreatedEventHandler> logger)
    {
      _logger = logger;
    }

    public Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {

      var x = new Task(async () => {
        var duracion = new Random().Next(5000);
        await Task.Delay(duracion);
        _logger.LogWarning("ocurrio un error!");
      });
      x.Start();
      _logger.LogInformation("Se creo el siguiente producto : {producto}", notification.Product);
      return Task.CompletedTask;
    }
  }

  internal class NotificarAlmacenHandler : INotificationHandler<ProductCreatedEvent>
  {
    private readonly ILogger<NotificarAlmacenHandler> _logger;

    public NotificarAlmacenHandler(ILogger<NotificarAlmacenHandler> logger)
    {
      _logger = logger;
    }
    public Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
      _logger.LogInformation("Almacen recibio pedido de producto: {producto}", notification.Product);
      return Task.CompletedTask;
    }
  }
}
