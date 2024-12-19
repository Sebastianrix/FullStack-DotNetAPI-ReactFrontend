using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;
using DataLayer;
using Mapster;
using System;
using DataLayer.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingCrewController : BaseController
    {
        private readonly IDataService _dataService;

        public RatingCrewController(IDataService dataService, LinkGenerator linkGenerator)
          : base(linkGenerator)
        {
            _dataService = dataService;
        }


        // --  --
        [HttpGet("{tConst}")]
        public ActionResult<PagedResponse<RatingCrew>> GetRatingCrew(string tConst, int pageNumber = 1, int pageSize = DefaultPageSize)
        {
            var ratingCrew = _dataService.GetRatingCrew(tConst);
            if (ratingCrew == null || !ratingCrew.Any())
            {
                return NotFound();
            }

            var totalItems = ratingCrew.Count();
            var pagedRatingCrew = ratingCrew
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            foreach (var name in pagedRatingCrew)
            {
                if (name.NConst != null)
                {
                    name.NConst = new Uri($"{Request.Scheme}://{Request.Host}/api/NameBasic/{name.NConst}").ToString();
                }
            }

            var response = CreatePagedResponse(pagedRatingCrew, pageNumber, pageSize, totalItems, "GetRatingCrew");

            return Ok(response);
        }
    }
}