using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;

namespace Core.Security
{
  public class JwtMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly JwtConfiguration _config;

    public JwtMiddleware(RequestDelegate next, JwtConfiguration config)
    {
      _next = next;
      _config = config;
    }

    public async Task Invoke(HttpContext context)
    {
      var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
      var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
      var shouldValidate = endpoint?.Metadata.GetMetadata<TokenCheckAttribute>() != null;

      if (!shouldValidate)
      {
        await _next(context);
        return;
      }

      if (shouldValidate && token == null)
      {
        await UnauthorizedRequest(context);
        return;
      }

      var tokenData = ValidateToken(token, out bool isValid);
      //TokenInformation.UserName = tokenData.Claims.FirstOrDefault(c => c.Type == "user").Value;
      //try
      //{
      //  int.TryParse(tokenData.Claims.FirstOrDefault(c => c.Type == "customerid").Value, out int cid);
      //  int.TryParse(tokenData.Claims.FirstOrDefault(c => c.Type == "customercontactid").Value, out int ccid);
      //  TokenInformation.CustomerId = cid;
      //  TokenInformation.CustomerContactId = ccid;
      //}
      //catch (Exception ex) {
      //  Console.WriteLine(ex.Message);
      //}


      context.Items["TokenData"] = tokenData.Claims;
      if (shouldValidate && tokenData == null)
      {
        await UnauthorizedRequest(context);
        return;
      }

      await _next(context);
    }

    /// <summary>
    /// Verifica si el token es valido
    /// </summary>
    private JwtSecurityToken ValidateToken(string token, out bool isValid)
    {
      var tokenManager = new TokenManager(_config);
      isValid = tokenManager.IsValid(token);
      var tokenData = tokenManager.GetToken();
      return tokenData;
    }

    private async Task UnauthorizedRequest(HttpContext context)
    {
      context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
      var responseContent = new
      {
        StatusCode = context.Response.StatusCode,
        Message = "Invalid Request"
      };
      await context.Response.WriteAsJsonAsync(responseContent);
    }

  }
}
