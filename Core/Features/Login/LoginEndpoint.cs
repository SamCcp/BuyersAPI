using Core.Interfaces;
using Core.Security;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Core.Features.Login
{
  public class LoginEndpoint : IEndpoint
  {

    public record LoginModel
    {
      public string UserName { get; set; }
      public string Password { get; set; }
    }

    public void AddRoutes(IEndpointRouteBuilder app)
    {
      app.MapPost(ApiRoutes.Login.LoginMethod, (JwtConfiguration jwtConfig, LoginModel login) =>
      {
        var tokenManager = new TokenManager(jwtConfig);
        tokenManager.AddClaim("user", login.UserName);
        var token = tokenManager.CreateToken();
        return token;
      })
      .Produces(200)
      .Produces(200, typeof(string))
      .WithTags(new string[] { "Login" });

      app.MapPost(ApiRoutes.Login.TokenValidate, (JwtConfiguration jwtConfig, string token) => {
        var tokenManager = new TokenManager(jwtConfig);
        var isValid = tokenManager.IsValid(token);
        var tokenData = tokenManager.GetToken();
        var samData = tokenData.Claims.First(c => c.Type == "user").Value;
        return isValid;
      })
      .WithTags(new string[] {"Login" });
    }
  }
}
