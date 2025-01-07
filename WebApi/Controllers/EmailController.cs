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


    }
}
