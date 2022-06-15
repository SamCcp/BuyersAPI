using Core.Interfaces;
using Core.Security;

using Microsoft.AspNetCore.Builder;
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
      app.MapPost("login", (JwtConfiguration jwtConfig, LoginModel login) =>
      {
        var tokenManager = new TokenManager(jwtConfig);
        tokenManager.AddClaim("user", login.UserName);
        var token = tokenManager.CreateToken();
        return token;
      }).AllowAnonymous();

      app.MapPost("login/validate/{token}", (JwtConfiguration jwtConfig, string token) => {
        var tokenManager = new TokenManager(jwtConfig);
        var isValid = tokenManager.IsValid(token);
        var tokenData = tokenManager.GetToken();
        var samData = tokenData.Claims.First(c => c.Type == "sam").Value;
        return isValid;
      }).AllowAnonymous();
    }
  }
}
