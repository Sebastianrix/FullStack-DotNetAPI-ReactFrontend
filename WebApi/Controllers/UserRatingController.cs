using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DataLayer;
using WebApi.DTOs;
using Mapster;
using System.Linq;

namespace WebApi.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  [EnableCors("AllowReactApp")]
  public class UserRatingController : BaseController
  {
    private readonly IDataService _dataService;

    public UserRatingController(IDataService dataService, LinkGenerator linkGenerator)
      : base(linkGenerator)
    {
      _dataService = dataService;
    }

    // Get paginated ratings for a user
    [HttpGet( Name = nameof(GetUserRatings))]
    [Authorize]
    public IActionResult GetUserRatings(int pageNumber = 1, int pageSize = DefaultPageSize)
    {
      var userId = int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value, out var userIdInt);


      if (userIdInt == null) return Unauthorized();

      var totalRatings = _dataService.GetUserRatingCount(userIdInt);
      if (totalRatings == 0) return NotFound();

      var userRatings = _dataService.GetUserRatings(userIdInt, pageNumber, pageSize);
      var userRatingDtos = userRatings.Select(r => r.Adapt<UserRatingDto>().WithSelfLink(GetUrl(nameof(GetUserRatingById), new { userIdInt, ratingId = r.Id }))).ToList();

      var paginatedResult = CreatePagingUser(nameof(GetUserRatings), userIdInt, pageNumber, pageSize, totalRatings, userRatingDtos);
      return Ok(paginatedResult);
    }

    // Get a specific rating by userId and ratingId
    [HttpGet("{ratingId}", Name = nameof(GetUserRatingById))]
    [Authorize]
    public IActionResult GetUserRatingById(int ratingId)
    {
      var userId = int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value, out var userIdInt);


      if (userIdInt == null) return Unauthorized();

      var userRating = _dataService.GetUserRating(ratingId);
      if (userRating == null) return NotFound();

      var userRatingDto = userRating.Adapt<UserRatingDto>().WithSelfLink(GetUrl(nameof(GetUserRatingById), new { userIdInt, ratingId }));
      return Ok(userRatingDto);
    }

    // Add a new rating
    [HttpPost]
    public IActionResult AddUserRating([FromBody] UserRatingDto userRatingDto)
    {
      if (!ModelState.IsValid) return BadRequest(ModelState);

      var existingRating = _dataService.GetUserRatingByUserAndTConst(userRatingDto.UserId, userRatingDto.TConst);
      if (existingRating != null)
      {
        return Conflict("User has already rated this title.");
      }
      if (!_dataService.UserExists(userRatingDto.UserId))
      {
        return NotFound(new { message = "User does not exist." });
      }
      //Add a check to ensure title exists in database here too
      try
      {
        var userRating = _dataService.AddUserRating(userRatingDto.UserId, userRatingDto.TConst, userRatingDto.Rating);
        var resultDto = userRating.Adapt<UserRatingDto>().WithSelfLink(GetUrl(nameof(GetUserRatingById), new { userId = userRatingDto.UserId, ratingId = userRating.Id }));

        return CreatedAtAction(nameof(GetUserRatingById), new { userId = userRatingDto.UserId, ratingId = userRating.Id }, resultDto);
      }
      catch (ArgumentException ex)
      {
        return BadRequest(ex.Message);
      }
    }

    // Delete a rating
    [HttpDelete("{userId}/{ratingId}")]
    public IActionResult DeleteUserRating(int userId, int ratingId)
    {
      if (_dataService.GetUserRating(ratingId) == null) return NotFound();

      _dataService.DeleteUserRating(ratingId);
      return NoContent();
    }

    // Update a rating by ratingId
    [HttpPut("{ratingId}")]
    [Authorize]
    public IActionResult UpdateUserRating(int ratingId, [FromBody] UserRatingDto userRatingDto)
    {
      var userId = int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value, out var userIdInt);


      if (userIdInt == null) return Unauthorized();

      var existingRating = _dataService.GetUserRating(ratingId);
      if (existingRating == null || existingRating.UserId != userIdInt)
      {
        return Forbid("User does not have permission to modify this rating.");
      }

      if (!ModelState.IsValid) return BadRequest(ModelState);

      if (_dataService.GetUserRating(ratingId) == null) return NotFound();

      _dataService.UpdateUserRating(userIdInt, ratingId, userRatingDto.Rating);
      return NoContent();
    }
  }
}

public static class DtoExtensions
{
  public static T WithSelfLink<T>(this T dto, string selfLink) where T : class
  {
    if (dto is UserRatingDto ratingDto) ratingDto.SelfLink = selfLink;
    return dto;
  }
}