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
    public class Top10Controller : BaseController
    {
        private readonly IDataService _dataService;

        public Top10Controller(IDataService dataService, LinkGenerator linkGenerator)
          : base(linkGenerator)
        {
            _dataService = dataService;
        }

        [HttpGet("movies")]
        public ActionResult<IList<Top10Movies>> GetTop10Movies()
        {
            var top10 = _dataService.GetTop10Movies();
            if (top10 == null || !top10.Any())
            {
                return NotFound();
            }

            return Ok(top10);
        }

        [HttpGet("series")]
        public ActionResult<IList<Top10Series>> GetTop10Series()
        {
            var top10 = _dataService.GetTop10Series();
            if (top10 == null || !top10.Any())
            {
                return NotFound();
            }

            return Ok(top10);
        }

        [HttpGet("actors")]
        public ActionResult<IList<Top10Actors>> GetTop10Actors()
        {
            var top10 = _dataService.GetTop10Actors();
            if (top10 == null || !top10.Any())
            {
                return NotFound();
            }

            return Ok(top10);
        }
    }
}