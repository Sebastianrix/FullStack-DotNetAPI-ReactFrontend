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
    [Route("api/email")]
    [EnableCors("AllowReactApp")]
    public class EmailController : Controller
    {
        private readonly IDataService _dataService;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;

        public EmailController(IDataService dataService, IConfiguration configuration, IEmailSender emailSender)
        {
            _configuration = configuration;
            _dataService = dataService;
            _emailSender = emailSender;
        }

        [HttpPost("send-test-email")]
        public async Task<IActionResult> SendTestEmail([FromBody] string email)
        {
            try
            {
                await _emailSender.SendEmailAsync(email,
                                                  "Hello from server!",
                                                  "This is a test mail");

                return Ok($"Test email sent to {email}!");
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"An error occurred while sending email: {ex.Message}");
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO dto)
        {
            var user = _dataService.GetUserByEmail(dto.Email);
            if (user == null)
                return NotFound("No user found with that email address.");


            var resetToken = Guid.NewGuid().ToString();


            var subject = "Password Reset";
            var message = $"Hello {user.Username},\n\nUse this token to reset your password:\n{resetToken}\n\nRegards,\nYour App Team";


            await _emailSender.SendEmailAsync(dto.Email, subject, message);

            return Ok("Password reset email sent.");
        }

        [HttpPost("send-welcome-email")]
        public async Task<IActionResult> SendWelcomeEmail([FromBody] string email)
        {
            try
            {
                var user = _dataService.GetUserByEmail(email);
                if (user == null)
                    return NotFound("No user found with that email address.");

                var subject = "Welcome to Our Service!";
                var message = $"Hello {user.Username},\n\n" +
                              "Welcome! We're happy to have you here.\n\n" +
                              "Here are some tips to get started:\n" +
                              "- Stay cool\n" +
                              "- Keep coding\n\n" +
                              "Feel free to explore.\n\n" +
                              "Best Regards,\nSebastian Rix";

                await _emailSender.SendEmailAsync(email, subject, message);

                return Ok($"Welcome email sent to {email}!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while sending the welcome email: {ex.Message}");
            }
        }


    }
}
