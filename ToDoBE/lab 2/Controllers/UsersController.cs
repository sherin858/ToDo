using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.Authentication.Dtos;

namespace WebAPI.Authentication.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<User> _userManager;

    public UsersController(IConfiguration configuration,
        UserManager<User> userManager)
    {
        _configuration = configuration;
        _userManager = userManager;
    }

    #region Login

    [HttpPost]
    [Route("Login")]
    public async Task<ActionResult<TokenDto>> Login(LoginDto credentials)
    {
        User? user = await _userManager.FindByNameAsync(credentials.UserName);
        if (user == null)
        {
            return BadRequest();
        }

        bool isPasswordCorrect = await _userManager.CheckPasswordAsync(user, credentials.Password);
        if (!isPasswordCorrect)
        {
            return BadRequest();
        }

        var claimsList = await _userManager.GetClaimsAsync(user);

        return GenerateToken(claimsList);
    }

    #endregion

    #region Register

    [HttpPost]
    [Route("Register")]
    public async Task<ActionResult> Register(RegisterDto registerDto)
    {
        var newUser = new User
        {
            UserName = registerDto.UserName,
            Email = registerDto.Email,
        };

        var creationResult = await _userManager.CreateAsync(newUser, 
            registerDto.Password);
        if (!creationResult.Succeeded)
        {
            return BadRequest(creationResult.Errors);
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, newUser.Id),
            new Claim(ClaimTypes.Role, "User")
        };

        await _userManager.AddClaimsAsync(newUser, claims);

        return NoContent();
    }

    #endregion

    #region Helpers

    private TokenDto GenerateToken(IList<Claim> claimsList)
    {
        string keyString = _configuration.GetValue<string>("SecretKey") ?? string.Empty;
        var keyInBytes = Encoding.ASCII.GetBytes(keyString);
        var key = new SymmetricSecurityKey(keyInBytes);


        var signingCredentials = new SigningCredentials(key,
            SecurityAlgorithms.HmacSha256Signature);


        var expiry = DateTime.Now.AddMinutes(15);

        var jwt = new JwtSecurityToken(
                expires: expiry,
                claims: claimsList,
                signingCredentials: signingCredentials);


        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenString = tokenHandler.WriteToken(jwt);

        return new TokenDto
        {
            Token = tokenString,
            Expiry = expiry
        };
    }

    #endregion
}
