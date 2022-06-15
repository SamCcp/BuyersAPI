namespace Core.Security
{
  public class TokenCheckAttribute : Attribute
  {
    private readonly BuyersRoles[] _roles;
    //private readonly string[] _roles;

    //public TokenCheckAttribute(string[] roles = null)
    //{
    //  _roles = roles;
    //}

    public TokenCheckAttribute(BuyersRoles[] role = null)
    {
      _roles = role;
    }

  }

  public enum BuyersRoles {
    User,
    Admin,
    Manager
  };
}
