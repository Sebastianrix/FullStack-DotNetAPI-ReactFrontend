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
    public class DataController : BaseController
    {
        private readonly IDataService _dataService;

        public DataController(IDataService dataService, LinkGenerator linkGenerator)
          : base(linkGenerator)
        {
            _dataService = dataService;
        }


        // --  --
        [HttpGet("genre")]
        public ActionResult<GetGenreData> GetGenreData()
        {
            var data = _dataService.GetGenreData();
            if (data == null || !data.Any())
            {
                return NotFound();
            }

            return Ok(data);
        }
        [HttpGet("titletype")]
        public ActionResult<GetTitleTypeData> GetTitleTypeData()
        {
            var data = _dataService.GetTitleTypeData();
            if (data == null || !data.Any())
            {
                return NotFound();
            }

            return Ok(data);
        }
        [HttpGet("startyear")]
        public ActionResult<GetYearData> GetYearData()
        {
            var data = _dataService.GetYearData();
            if (data == null || !data.Any())
            {
                return NotFound();
            }

            return Ok(data);
        }
    }
}