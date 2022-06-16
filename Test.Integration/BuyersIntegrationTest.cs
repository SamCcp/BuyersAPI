using API.ApiConfiguration;

using Core.Features;

using Microsoft.AspNetCore.Mvc.Testing;

using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

using static Core.Features.Login.LoginEndpoint;

namespace Test.Integration
{
  public class BuyersIntegrationTest
  { 
    protected readonly HttpClient _httpClient;

    protected BuyersIntegrationTest()
    {
      var appFactory = new WebApplicationFactory<APIHAndler>();
      _httpClient = appFactory.CreateClient();
    }
    protected async Task Authenticate()
    {
      var token = await GetNewToken();

      await Task.FromResult(
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token)
      );
    }
    /// <summary>
    /// Realiza el metodo de login para obtener un nuevo token
    /// </summary>
    private async Task<string> GetNewToken()
    {
      var login = new LoginModel
      {
        UserName = "sam",
        Password = "el tucan"
      };

      var postData = new StringContent(JsonSerializer.Serialize(login), UnicodeEncoding.UTF8, "application/json");
      var tokenResponse = await _httpClient.PostAsync(ApiRoutes.Login.LoginMethod, postData);
      var token = await tokenResponse.Content.ReadAsStringAsync();
      return token;
    }
  }
}
