using Authentication.DbContexts;
using Authentication.Dtos.AuthDtos;
using FluentValidation;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Authentication.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Authentication.Apis;

static public class AuthRequest
{
    static public void AddAuthRequest(this WebApplication application)
    {
        var app = application.MapGroup("/auth");

        app.MapPost("/signup", async (UserDbContext db, IMapper mapper, IValidator<SignupUserDto> validator, SignupUserDto signupUser) =>
        {
            var results = validator.Validate(signupUser);

            if (!results.IsValid) return Results.ValidationProblem(results.ToDictionary());

            if (await db.Users.AnyAsync(u => u.UserId == signupUser.UserId)) return Results.Conflict();

            User user = mapper.Map<User>(signupUser);

            db.Users.Add(user);

            user.HashPassword = BCrypt.Net.BCrypt.HashPassword(signupUser.Password);

            await db.SaveChangesAsync();

            return Results.Ok();
        });

        app.MapPost("/signin", async (HttpContext context, UserDbContext db, IValidator<SigninUserDto> validator, IConfiguration config, SigninUserDto signinUser) =>
        {
            var results = validator.Validate(signinUser);

            if (!results.IsValid) return Results.Unauthorized();

            User? user = await db.Users.FirstOrDefaultAsync(u => u.UserId==signinUser.UserId);

            if (user == null) return Results.Unauthorized();

            if (!BCrypt.Net.BCrypt.Verify(signinUser.Password, user.HashPassword)) return Results.Unauthorized();

            SigningCredentials credential = new(new SymmetricSecurityKey(Convert.FromBase64String(config["Jwt:Key"]!)),
                SecurityAlgorithms.HmacSha256);

            Claim[] claims_ =
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.Username),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            JwtSecurityToken token = new(               // 토큰 생성
                issuer: config["Issuer"],               // 토큰 발급자 (현재 null)
                audience: config["Audience"],           // 토큰을 받는 클라이언트 (현재 null)
                claims: claims_,                        // 사용자 정보
                signingCredentials: credential,         // 인증서
                expires: DateTime.Now.AddMinutes(60));  // 토큰 유효시간 60분

            string stringToken = new JwtSecurityTokenHandler().WriteToken(token);

            CookieOptions cookieOptions = new()
            {
                HttpOnly = true,                        // 웹브라우저에서 자동으로 쿠키 처리하기
                Secure = true,                          // https 프로토콜만 허용
                Expires = DateTime.Now.AddMinutes(60)   // 토큰 유효 기간 60분
            };

            context.Response.Cookies.Append("jwt-token", stringToken, cookieOptions);
            // HttpContext 쿠키에 토큰 추가

            return Results.Ok(new { Message = "SUCCESS!" });
        });

        app.MapPost("/signout", (HttpContext context) =>
        {
            context.Response.Cookies.Delete("jwt-token");
            return Results.Ok(new { Message = "SUCCESS" });
        }).RequireAuthorization();
    }
}
