using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs
{
    public class ForgotPasswordDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
