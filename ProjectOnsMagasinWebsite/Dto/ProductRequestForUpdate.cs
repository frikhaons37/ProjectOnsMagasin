using System.ComponentModel.DataAnnotations;

namespace ProjectOnsMagasin;

public class ProductRequestForUpdate
{
    [Required]
    [Range(1, int.MaxValue)]
    public int Id { get; set; }
    [StringLength(50, MinimumLength = 5)]
    public string Name { get; set; } = null!;
    [StringLength(250, MinimumLength = 25)]
    public string Description { get; set; } = null!;
    [StringLength(50, MinimumLength = 5)]
    public string Brand { get; set; } = null!;
    [Range(1, int.MaxValue)]
    public double Price { get; set; }
    [Range(1, int.MaxValue)]
    public int Qunatity { get; set; }
    public IFormFile Image { get; set; } = null!;
    [Range(1, int.MaxValue)]
    public int CategoryId { get; set; }
}
