using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs
{
  public class SearchHistoryDTO
  {
    [Required]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required(ErrorMessage = "Search query cannot be empty.")]
    [MinLength(1, ErrorMessage = "Search query must have at least 1 character.")]
    public string SearchQuery { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? SelfLink { get; set; }
  }
}