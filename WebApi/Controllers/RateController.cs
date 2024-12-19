using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebApi.DTOs;
using DataLayer;
using Mapster;
using System;
using DataLayer.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowReactApp")]
    public class RateController : BaseController
    {
        private readonly IDataService _dataService;

        public RateController(IDataService dataService, LinkGenerator linkGenerator)
          : base(linkGenerator)
        {
            _dataService = dataService;
        }


        // --ADD RATING--
        [HttpGet("{tConst}/{rating}")]
        [Authorize]
       public ActionResult rate(string tConst, int rating)
        {
            var userId = int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value, out var userIdInt);


            if (userIdInt == null) return Unauthorized();

            if (!_dataService.UserExists(userIdInt))
            {
                return NotFound(new { message = "User does not exist." });
            }
            else if (_dataService.GetTitleByTConst(tConst) == null)
            {
                return NotFound(new { message = "Title does not exist." });
            }
            else if (rating < 1 || rating > 10)
            {
                return BadRequest(new { message = "Rating must be between 1 and 10." });
            }
            _dataService.rate(tConst, rating, userIdInt);
            return Ok();
        }

        [HttpGet("Delete/{userRatingId}")]
        public ActionResult rateDelete(int userRatingId)
        {
            var userId = int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value, out var userIdInt);

            if (userIdInt == null) return Unauthorized();

            _dataService.rateDelete(userRatingId, userIdInt);
            return Ok();
        }
    }
}