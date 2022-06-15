using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities
{
  [Table("Products")]
  public record ProductEntity
  {
    [Key]
    public int ProductId { get; set; }
    public string ProductName { get; set; } = "";
  }
}
