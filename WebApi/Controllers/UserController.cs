using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.DTOs;
using WebApi.Services;
using Mapster;

namespace WebApi.Controllers
{
  [ApiController]
  [Route("api/v3/user")]
  [EnableCors("AllowReactApp")]
  public class UsersController : BaseController
  {
    private readonly IDataService _dataService;
    private readonly Hashing _hashing;
    private readonly IConfiguration _configuration;

    public UsersController(IDataService dataService, IConfiguration configuration, LinkGenerator linkGenerator, Hashing hashing)
        : base(linkGenerator)
    {
      _configuration = configuration;
      _dataService = dataService;
      _hashing = hashing;
    }

    // -- GET USER by ID --
    [HttpGet("profile")]
    [Authorize]
    public IActionResult GetUser()
    {
      var userId = int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value, out var userIdInt);


      if (userIdInt == null) return Unauthorized();

      var user = _dataService.GetUser(userIdInt);

      if (user == null) return NotFound();

      var userDto = user.Adapt<UserDTO>();
      userDto.SelfLink = GenerateSelfLink(nameof(GetUser), new { userIdInt });

      return Ok(userDto);
    }

    // -- GET USER by USERNAME --
    [HttpGet("username/{username}")]
    public IActionResult GetUserByUsername(string username)
    {
      var user = _dataService.GetUser(username);
      if (user == null) return NotFound();

      var userDto = user.Adapt<UserDTO>();
      userDto.SelfLink = GenerateSelfLink(nameof(GetUserByUsername), new { username });

      return Ok(userDto);
    }

    // -- GET USER by Email --
    [HttpGet("email/{email}")]
    public IActionResult GetUserByEmail(string email)
    {
       var user = _dataService.GetUserByEmail(email);
       if (user == null) return NotFound();

       var userDto = user.Adapt<UserDTO>();
       userDto.SelfLink = GenerateSelfLink(nameof(GetUserByEmail), new { email });

       return Ok(userDto);
    }

    // -- REGISTER USER / CREATE USER --
    [HttpPost("register")]
    public IActionResult RegisterUser([FromBody] UserRegisterDTO dto)
    {
      if (!ModelState.IsValid ||
          string.IsNullOrWhiteSpace(dto.Username) ||
          string.IsNullOrWhiteSpace(dto.Password) ||
          string.IsNullOrWhiteSpace(dto.Email))
      {
        return BadRequest(ModelState);
      }
                                                                           
            if (_dataService.GetUser(dto.Username) != null)
            {
                return BadRequest(new { message = "User already exists"});
            }
            if (_dataService.GetUserByEmail(dto.Email) != null)
            {
                return BadRequest(new { message = "Email already exists"});
            }
            if (string.IsNullOrEmpty(dto.Password))
            {
                return BadRequest(new { message = "No password"});
            }
            if (dto.Password.Length < 8)
            {
                return BadRequest(new { message = "Password must be at least 8 characters long."});
            }

            if (!dto.Password.Any(char.IsUpper))
            {
                return BadRequest(new { message = "Password must contain at least one uppercase letter."});
            }

            if (!dto.Password.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                return BadRequest(new { message = "Password must contain at least one special character."});
            }

            if (!dto.Password.Any(char.IsDigit))
            {
                return BadRequest(new { message = "Password must contain at least one digit."});
            }

            if (string.IsNullOrEmpty(dto.Email))
            {
                return BadRequest(new { message = "No email"});
            }





            (var hashedPwd, var salt) = _hashing.Hash(dto.Password);

      var user = _dataService.CreateUser(dto.Name, dto.Username, hashedPwd, dto.Email, salt, dto.Role);

      var userDto = user.Adapt<UserDTO>();
      userDto.SelfLink = GenerateSelfLink(nameof(GetUser), new { userId = user.Id });

      return CreatedAtAction(nameof(GetUser), new { userId = user.Id }, userDto);
    }

    // -- LOGIN USER --
    [HttpPut("login")]
    public IActionResult Login(LoginUserModel model)
    {
      var user = _dataService.GetUser(model.UserName);

      if (user == null)
      {
        return BadRequest("Can't login. Password or Username is wrong.");
      }

      if (!_hashing.Verify(model.Password, user.Password, user.Salt))
      {
        return BadRequest("Couldn't verify.");
      }

      var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("Id", user.Id.ToString())
            };

      var secret = _configuration.GetSection("Auth:Secret").Value;
      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

      // Token generated
      var token = new JwtSecurityToken(
          claims: claims,
          expires: DateTime.Now.AddDays(7),
          signingCredentials: creds
      );

      var Jwt = new JwtSecurityTokenHandler().WriteToken(token);

       // Set token as cookie for authentication
      Response.Cookies.Append("auth_token_cookie", Jwt, new CookieOptions
    {
        HttpOnly = true,
        Secure = true, // Toggle HTTPS
        SameSite = SameSiteMode.Lax, // can be 'Lax', 'none' or 'strict' ( depents on same site policy, AKA same port or not )
        Expires = System.DateTimeOffset.UtcNow.AddHours(1)
    });
      return Ok(new { username = user.Username, token = Jwt});
    }

    // -- LOGOUT USER --
    [HttpPost("logout")]
    [Authorize]
    public IActionResult Logout()
    {
            Response.Cookies.Delete("auth_token_cookie");


            return Ok(new { message = "Logout successful" });
    }

    // -- DELETE USER --
    [HttpDelete("{userId}")]
    [Authorize(Roles = "admin")]
    public IActionResult DeleteUser(int userId)
    {
      if (_dataService.GetUser(userId) == null) return NotFound("Coundn't delete, because ID not found");

      _dataService.DeleteUser(userId);
      return NoContent();
    }
  }
}