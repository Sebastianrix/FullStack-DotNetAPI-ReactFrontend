using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebApi.DTOs;
using DataLayer;
using DataLayer.Models;
using System.Collections.Generic;
using System.Linq;

namespace WebApi.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  [EnableCors("AllowReactApp")]
  public class BookmarkController : BaseController
  {
    private readonly IDataService _dataService;

    public BookmarkController(IDataService dataService, LinkGenerator linkGenerator)
      : base(linkGenerator)
    {
      _dataService = dataService;
    }
    // -- ADD BOOKMARK --
    [HttpPost]
    [Authorize]
    public IActionResult AddBookmark([FromBody] CreateBookmarkDto dto)
    {
      // Extract userId from token claims
      var userIdClaim = User.FindFirst("Id");
      if (userIdClaim == null)
      {
        return Unauthorized("User ID not found in token.");
      }

      if (!int.TryParse(userIdClaim.Value, out var userId))
      {
        return Unauthorized("Invalid user ID in token.");
      }


      // Check if bookmark already exists
      var existingBookmark = _dataService.GetBookmarks(userId)
          .FirstOrDefault(b => b.TConst == dto.TConst && b.NConst == dto.NConst);

      if (existingBookmark != null)
      {
        return Conflict(new { message = "Bookmark already exists for this title and user." });
      }

      if (!_dataService.UserExists(userId))
      {
        return NotFound(new { message = "User does not exist." });
      }

      var bookmark = _dataService.AddBookmark(userId, dto.TConst, dto.NConst, dto.Note);


      var bookmarkDto = new BookmarkDto
      {
        Id = bookmark.Id,
        UserId = bookmark.UserId,
        TConst = bookmark.TConst,
        NConst = bookmark.NConst,
        Note = bookmark.Note,
        CreatedAt = bookmark.CreatedAt,
        SelfLink = GenerateSelfLink(nameof(GetBookmark), new { userId = bookmark.UserId, bookmarkId = bookmark.Id })
      };

      return CreatedAtAction(nameof(GetBookmark), new { userId = bookmark.UserId, bookmarkId = bookmark.Id }, bookmarkDto);
    }

    // Get all Bookmarks for a User with Pagination
    [HttpGet("user", Name = nameof(GetBookmarks))]
    [Authorize]
    public IActionResult GetBookmarks(int pageNumber = 1, int pageSize = 10)
    {
      var userId = int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value, out var userIdInt);

      if (userIdInt == null) return Unauthorized();

      var bookmarks = _dataService.GetBookmarks(userIdInt, pageNumber, pageSize);
      if (bookmarks == null || !bookmarks.Any())
        return NotFound();

      var totalItems = _dataService.GetBookmarkCountByUser(userIdInt);
      var bookmarkDtos = bookmarks.Select(MapToBookmarkDto).ToList();
      var paginatedResult = CreatePagingUser(nameof(GetBookmarks), userIdInt, pageNumber, pageSize, totalItems, bookmarkDtos);

      return Ok(paginatedResult);
    }

    // Get a specific Bookmark by ID with self-reference
    [HttpGet("{bookmarkId}", Name = nameof(GetBookmark))]
    public IActionResult GetBookmark(int userId, int bookmarkId)
    {
      var bookmark = _dataService.GetBookmark(userId, bookmarkId);
      return bookmark == null ? NotFound() : Ok(MapToBookmarkDto(bookmark));
    }

    // Update Bookmark with Existence Check
    [HttpPut("{bookmarkId}")]
    public IActionResult UpdateBookmark(int userId, int bookmarkId, BookmarkDto dto)
    {
      // Check if bookmark exists
      if (_dataService.GetBookmark(userId, bookmarkId) == null)
      {
        return NotFound();
      }

      _dataService.UpdateBookmark(userId, bookmarkId, dto.TConst, dto.NConst, dto.Note);
      return NoContent();
    }

    // Delete Bookmark with Existence Check
    [HttpDelete("{bookmarkId}")]
    public IActionResult DeleteBookmark(int bookmarkId)
    {
      var existingBookmark = _dataService.GetBookmarkById(bookmarkId);
      if (existingBookmark == null)
      {
        return NotFound();
      }

      _dataService.DeleteBookmark(bookmarkId);
      return NoContent();
    }

    // Private Helper Method to Map Bookmark to BookmarkDto
    private BookmarkDto MapToBookmarkDto(Bookmark bookmark)
    {
      return new BookmarkDto
      {
        Id = bookmark.Id,
        UserId = bookmark.UserId,
        TConst = bookmark.TConst,
        NConst = bookmark.NConst,
        Note = bookmark.Note,
        CreatedAt = bookmark.CreatedAt,
        SelfLink = GetUrl(nameof(GetBookmark), new { userId = bookmark.UserId, bookmarkId = bookmark.Id })
      };
    }
  }
}