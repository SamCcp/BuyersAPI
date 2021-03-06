namespace Core.Features
{
  public static class ApiRoutes
  {

    public static class Login
    {
      public const string LoginMethod = "login";
      public const string TokenValidate = "login/validate/{token}";
    }
    public static class Products
    {
      public const string GetAll = "products";
      public const string GetFilteredProducts = "products/filterby";
      public const string AddProduct = "products/add";
      public const string DeleteProduct = "products/delete";
    }
  }
}
