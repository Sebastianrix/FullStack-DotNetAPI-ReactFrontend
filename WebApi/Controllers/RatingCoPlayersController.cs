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
    public class RatingCoPlayersController : BaseController
    {
        private readonly IDataService _dataService;

        public RatingCoPlayersController(IDataService dataService, LinkGenerator linkGenerator)
          : base(linkGenerator)
        {
            _dataService = dataService;
        }


        // --  --
        [HttpGet("{nConst}")]
        public ActionResult<PagedResponse<RatingCoPlayer>> GetRatingCoPlayers(string nConst, int pageNumber = 1, int pageSize = DefaultPageSize)
        {
            var coplayers = _dataService.GetRatingCoPlayers(nConst);
            if (coplayers == null || !coplayers.Any())
            {
                return NotFound();
            }

            var totalItems = coplayers.Count();
            var pagedCoPlayers = coplayers
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            foreach (var name in pagedCoPlayers)
            {
                if (name.NConst != null)
                {
                    name.NConst = new Uri($"{Request.Scheme}://{Request.Host}/api/NameBasic/{name.NConst}").ToString();
                }
            }

            var response = CreatePagedResponse(pagedCoPlayers, pageNumber, pageSize, totalItems, "GetRatingCoPlayers");

            return Ok(response);
        }
    }
}