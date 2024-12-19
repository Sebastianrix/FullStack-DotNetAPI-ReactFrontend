using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs
{
  public class BookmarkDto
  {
    [Required]
    public int Id { get; set; }
    [Required]
    public int UserId { get; set; }

    public string? TConst { get; set; }

    public string? NConst { get; set; }

    [StringLength(500, ErrorMessage = "Note cannot exceed 500 characters.")]
    public string? Note { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string? SelfLink { get; set; }
  }
}