using System.Text.Json.Serialization;

namespace ProjectOnsMagasin;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string City { get; set; } = null!;
    public RoleEnum Role { get; set; } = RoleEnum.User;
    public ICollection<Order>? Orders { get; set; }
}
