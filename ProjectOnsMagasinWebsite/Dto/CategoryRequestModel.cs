using System.ComponentModel.DataAnnotations;

namespace ProjectOnsMagasin;

public class CategoryRequestModel
{
    [Required]
    [StringLength(50, MinimumLength = 5)]
    public string Name { get; set; } = null!;
}
