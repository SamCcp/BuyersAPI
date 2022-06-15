namespace Core.Interfaces
{
  public interface IProductRepository<T>
  {
    /// <summary>
    /// Obtiene todos los items del catalogo
    /// </summary>
    Task<IEnumerable<T>> GetAllProducts();
    /// <summary>
    /// Obtiene los items que coincidan con el parametro de busqueda
    /// </summary>
    Task<IEnumerable<T>> GetFilteredProducts(T product);
    /// <summary>
    /// Crea un nuevo item en el catalogo
    /// </summary>
    Task<T> CreateProduct(T product);
  }
}
