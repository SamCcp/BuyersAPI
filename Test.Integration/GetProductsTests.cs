
using Core.Domain.Entities;
using Core.Features;

using FluentAssertions;

using System.Text.Json;

namespace Test.Integration
{
  public class GetProductsTests : BuyersIntegrationTest
  { 
    [Fact]
    public async Task GetAll_Products_ReturnsList()
    {
      //Arrange
      await Authenticate();
      //Act
      var response = await _httpClient.GetAsync(ApiRoutes.Products.GetAll);
      //Assert
      response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
      var jsonData = await response.Content.ReadAsStringAsync();
      jsonData.Should().NotBeNull();
      var products = JsonSerializer.Deserialize<List<ProductEntity>>(jsonData);
      products.Should().NotBeNull();
      products.Count().Should().Be(10);
    }

    [Fact]
    public async Task GetAll_Products_ShouldFailAuthentication()
    {
      //Arrange

      //Act
      var response = await _httpClient.GetAsync(ApiRoutes.Products.GetAll);
      //Assert
      response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }
  }
}
