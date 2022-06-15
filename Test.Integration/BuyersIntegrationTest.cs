using API.ApiConfiguration;

using Microsoft.AspNetCore.Mvc.Testing;

using System.Net.Http.Headers;

namespace Test.Integration
{
  public class BuyersIntegrationTest
  { 
    protected readonly HttpClient _httpClient;
    private readonly string _token;

    protected BuyersIntegrationTest(string token)
    {
      var appFactory = new WebApplicationFactory<APIHAndler>();
      _httpClient = appFactory.CreateClient();
      _token = token;
    }
    protected async Task Authenticate()
    {
      await Task.FromResult(
        _httpClient.DefaultRequestHeaders.Authorization =new AuthenticationHeaderValue("Bearer", _token)
      );
    }
  }
}
