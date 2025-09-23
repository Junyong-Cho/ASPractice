using System.ComponentModel.DataAnnotations;

namespace DbApi.Models;

//public record User(int Id, string Username, string Email);

public class User
{
    [Key]       // 기본키 속성
    public int Id { get; set; }
    [Required]  // 반드시 필요한 속성 == Not null
    public string? Username { get; set; }
    public string? Email { get; set; }
}