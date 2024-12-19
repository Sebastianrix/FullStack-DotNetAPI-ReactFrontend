using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs
{
    public class CreateBookmarkDto
    {
        public string? TConst { get; set; }
        public string? NConst { get; set; }

        [StringLength(500, ErrorMessage = "Note cannot exceed 500 characters.")]
        public string? Note { get; set; }
    }
}