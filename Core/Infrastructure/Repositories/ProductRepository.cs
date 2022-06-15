
using Core.Domain.Entities;
using Core.Domain.Models;
using Core.Interfaces;

using Dapper;

using System.Text.Json;

namespace Core.Infrastructure.Repositories
{
  public class ProductRepository : IProductRepository<ProductEntity>
  {
    private readonly IDBConnectionFactory _db;

    public ProductRepository(IDBConnectionFactory db)
    {
      _db = db;
    }

    //public async Task<IEnumerable<ProductEntity>> GetAllProducts()
    //{
    //  using var cliente = new HttpClient();
    //  var requestData = new
    //  {
    //    SP = $"select top 10 ProductId, ProductName from products ",
    //    Params = ""
    //  };
    //  var response = await cliente.PostAsync("https://demo.azzule.com/api/servicios.asmx/ExecuteSP", new StringContent(JsonSerializer.Serialize(requestData)));
    //  var data = await response.Content.ReadAsStringAsync();
    //  var products = JsonSerializer.Deserialize<ClassicApiResponse<ProductEntity>>(data)!;
    //  return products.Data.Table;

    //}

    //public async Task<IEnumerable<ProductEntity>> GetFilteredProducts(ProductEntity product)
    //{
    //  using var cnx = new SqlConnection("data source=10.10.50.30; uid=ApplicationUser_; pwd=D3$@rr0ll0; initial catalog=masterdb; application name=Buyers API; TrustServerCertificate=True");
    //  var parametrosBusqueda = new { pid = product.ProductId, pname = $"{product.ProductName}%" };
    //  var data = await cnx.QueryAsync<ProductEntity>("select top 10 productid, productname from products where productid = @pid or productname like @pname", parametrosBusqueda);
    //  return data;
    //}

    public async Task<IEnumerable<ProductEntity>> GetAllProducts()
    {
      using var cnx = await _db.CreateConnection(); //SQL | Posgre | Mongo | Oracle
      var products = await cnx.GetListPagedAsync<ProductEntity>(1, 10, "", "");
      return products;
    }

    public async Task<IEnumerable<ProductEntity>> GetFilteredProducts(ProductEntity product)
    { 
      using var cnx = await _db.CreateConnection();
      var parametrosBusqueda = $"where 1=1 and ";

      if (product.ProductId > 0)
        parametrosBusqueda += $"productid = {product.ProductId}";
      else
        parametrosBusqueda += $"productname like '{product.ProductName}%'";
      
      var data = await cnx.GetListAsync<ProductEntity>(parametrosBusqueda);
      return data;
    }

    public async Task<ProductEntity> CreateProduct(ProductEntity product)
    { 
      using var cnx = await _db.CreateConnection();
      var tran = cnx.BeginTransaction();
      var data = await cnx.InsertAsync(product, tran);
      product.ProductId = (int)data;
      tran.Rollback();
      return product;
    }
  }
}
