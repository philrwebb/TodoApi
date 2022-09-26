using TodoApi.Models;
using  Microsoft.AspNetCore.Identity;
using  Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
namespace TodoApi.Services;

public class UserService: IUserService 
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IConfiguration _configuration;
    public UserService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }
    public async Task<UserManagerResponse> RegisterUserAsync(RegisterViewModelDto userDto)
    {
        if (userDto == null)
        {
            throw new NullReferenceException("Register Model is null");
        }
        var user = new IdentityUser
        {
            Email = userDto.Email,
            UserName = userDto.Email
        };
        var result = await _userManager.CreateAsync(user, userDto.Password);
        if(result.Succeeded)
        {
            return new UserManagerResponse
            {
                Message = "User created successfully",
                IsSuccess = true
            };
        }

        return new UserManagerResponse
        {
            Message = "User did not create",
            IsSuccess = false,
            Errors = result.Errors.Select(e => e.Description)
        };
    }

    public async Task<UserManagerResponse> LoginUserAsync(LoginViewModelDto model) {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return new UserManagerResponse
            {
                Message = "There is no user with that email address",
                IsSuccess = false
            };
        }
        var result = await _userManager.CheckPasswordAsync(user, model.Password);
        if (!result)
        {
            return new UserManagerResponse
            {
                Message = "Invalid password",
                IsSuccess = false
            };
        }
        var claims = new[]
        {
            new Claim("Email", model.Email),
            new Claim(ClaimTypes.Name, user.Id)
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretForKey"]));
        var token = new  JwtSecurityToken(
            issuer: _configuration["Authentication:Issuer"],
            audience: _configuration["Authentication:Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(30),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );
        string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);
        return new UserManagerResponse 
        {
            Message = tokenAsString,
            IsSuccess = true,
            ExpireDate = token.ValidTo
        };
    }
}
