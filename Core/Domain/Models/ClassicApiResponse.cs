namespace Core.Domain.Models
{
  public class ClassicApiResponse<T>
  {
    public bool Ok { get; set; }
    public string Message { get; set; } = "";
    public Data<T> Data { get; set; } = new Data<T>();
  }
  public class Data<T>
  {
    public IEnumerable<T> Table { get; set; } = new List<T>();
  }
}
