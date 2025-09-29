using System.ComponentModel.DataAnnotations;

namespace Authentication.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(30)]
    public string UserId { get; set; } = null!;
    [Required]
    [MaxLength(256)]
    public string HashPassword { get; set; } = null!;
    [Required]
    [MaxLength(50)]
    public string Email { get; set; } = null!;
    [Required]
    [MaxLength(10)]
    public string Username { get; set; } = null!;
}
