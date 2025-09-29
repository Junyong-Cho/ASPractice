namespace Authentication.Dtos.AuthDtos;

public class SignupUserDto
{
    public string UserId { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
}
