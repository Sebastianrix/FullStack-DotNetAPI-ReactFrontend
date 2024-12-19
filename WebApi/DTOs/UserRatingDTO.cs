using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs
{
  public class UserRatingDto
  {
    [Required]
    public int Id { get; set; }
    [Required]
    public int UserId { get; set; }

    [Required]
    public string TConst { get; set; }

    [Range(1, 10, ErrorMessage = "Rating must be between 1 and 10.")]
    public int Rating { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? SelfLink { get; set; }
  }
}