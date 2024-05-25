using System.ComponentModel.DataAnnotations;

namespace ProjectOnsMagasin;

public class UserRegisterRequestModel
{
    [Required]
    [StringLength(50, MinimumLength = 5)]
    public string UserName { get; set; } = null!;
    [EmailAddress]
    public string Email { get; set; } = null!;
    [Required]
    [StringLength(50, MinimumLength = 5)]
    public string Password { get; set; } = null!;
    [Required]
    [StringLength(25, MinimumLength = 8)]
    public string PhoneNumber { get; set; } = null!;
    [Required]
    [StringLength(150, MinimumLength = 25)]
    public string Address { get; set; } = null!;
    [Required]
    [StringLength(150, MinimumLength = 5)]
    public string City { get; set; } = null!;
}
public class UserLoginRequestModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
    [Required]
    [StringLength(50, MinimumLength = 5)]
    public string Password { get; set; } = null!;
}