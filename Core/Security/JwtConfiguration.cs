using System.Text;

namespace Core.Security
{
  public record JwtConfiguration
  {
    /// <summary>
    /// Quien emite el token
    /// </summary>
    public string Issuer { get; set; }
    /// <summary>
    /// Para que dominio es valido el token
    /// </summary>
    public string Audience { get; set; }
    /// <summary>
    /// Llave de encriptacion del token
    /// </summary>
    public string Key { get; set; }
    /// <summary>
    /// Duracion del token antes de expirar ( en horas )
    /// </summary>
    public int TokenDuration { get; set; }
    /// <summary>
    /// Obtiene la llave de encriptacion en formato de bytes
    /// </summary>
    public byte[] GetKey()
    {
      if (string.IsNullOrEmpty(Key))
      {
        return new byte[] {0};
      }
      return Encoding.UTF8.GetBytes(Key);
    }
  }
}
