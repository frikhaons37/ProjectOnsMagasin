using System.ComponentModel.DataAnnotations;

namespace ProjectOnsMagasin;

public class ProductRequest
{

    [Required]
    [StringLength(50,MinimumLength =5)]
    public string Name { get; set; } = null!;
    [Required]
    [StringLength(250, MinimumLength = 25)]
    public string Description { get; set; } = null!;
    [Required]
    [StringLength(50, MinimumLength = 5)]
    public string Brand { get; set; } = null!;
    [Required]
    [Range(1, int.MaxValue)]
    public double Price { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public int Qunatity { get; set; }
    [Required]
    public IFormFile Image { get; set; } = null!;
    [Required]
    [Range(1,int.MaxValue)]
    public int CategoryId { get; set; }
}
