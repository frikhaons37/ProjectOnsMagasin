using System.ComponentModel.DataAnnotations;

namespace ProjectOnsMagasin;

public class CategoryRequestModelForUpdate
{
    [Required]
    [Range(1, int.MaxValue)]
    public int Id { get; set; }
    [StringLength(50, MinimumLength = 5)]
    public string Name { get; set; } = null!;
}