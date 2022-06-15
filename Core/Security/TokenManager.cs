using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Core.Security
{
  /// <summary>
  /// Clase encargada de generar y validar tokens
  /// </summary>
  public class TokenManager
  {
    private readonly JwtConfiguration _jwtConfig;
    private IList<Claim> _claims;
    private JwtSecurityToken _token;

    public TokenManager(JwtConfiguration jwtConfig)
    {
      _jwtConfig = jwtConfig;
      _claims = new List<Claim>();
    }
    /// <summary>
    /// Agrega propiedades al token
    /// </summary>
    public void AddClaim(string name, string value)
    { 
      _claims.Add(new Claim(name, value));
    }
    /// <summary>
    /// Crea el token de acceso a la aplicacion
    /// </summary>
    public string CreateToken()
    {
      var jwtTokenHandler = new JwtSecurityTokenHandler();
      var _key = _jwtConfig.GetKey();

      var securityKey = new SymmetricSecurityKey(_key);
      var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

      _claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

      var tokenDescriptor = CreateTokenDescriptor(credentials);
      var rawToken = jwtTokenHandler.CreateToken(tokenDescriptor);
      var token = jwtTokenHandler.WriteToken(rawToken);
      return token;
    }
    /// <summary>
    /// Crea el objeto del token
    /// </summary>
    private SecurityTokenDescriptor CreateTokenDescriptor(SigningCredentials credentials)
    {
      return new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(_claims),
        Issuer = _jwtConfig.Issuer,
        Audience = _jwtConfig.Audience,
        Expires = DateTime.UtcNow.AddHours(_jwtConfig.TokenDuration),
        SigningCredentials = credentials
      };
    }
    /// <summary>
    /// Determina si el token ingresado es valido
    /// </summary>
    public bool IsValid(string token)
    {
      if (string.IsNullOrEmpty(token)) return false;

      var jwtTokenHandler = new JwtSecurityTokenHandler();
      var key = _jwtConfig.GetKey();
      try
      {
        var validatedToken = ValidateToken(token, jwtTokenHandler, key);

        var jwtToken = (JwtSecurityToken)validatedToken;
        _token = jwtToken;
        return true;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        throw;
      }

    }
    /// <summary>
    /// Valida si el token es valido
    /// </summary>
    private static SecurityToken ValidateToken(string token, JwtSecurityTokenHandler jwtTokenHandler, byte[] key)
    {
      jwtTokenHandler.ValidateToken(token, new TokenValidationParameters
      {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
      }, out SecurityToken validatedToken);
      return validatedToken;
    }

    /// <summary>
    /// Obtiene el token representado en un objeto
    /// </summary>
    public JwtSecurityToken GetToken() => _token;
    
  }
}
