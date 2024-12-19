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
    public class TitleController : BaseController
    {
        private readonly IDataService _dataService;

        public TitleController(IDataService dataService, LinkGenerator linkGenerator)
          : base(linkGenerator)
        {
            _dataService = dataService;
        }


        [HttpGet("{tConst}")]
        public ActionResult<TitleDetailsDTO> GetTitleDetails(string tConst)
        {
            var titleBasic = _dataService.GetTitleByTConst(tConst);
            if (titleBasic == null)
            {
                return NotFound();
            }

            var genres = _dataService.GetGenresByTConst(tConst).Select(g => g.Genre).ToList();
            var countries = _dataService.GetCountriesByTConst(tConst).Select(c => c.Country).ToList();
            var rating = _dataService.GetRatingByTConst(tConst);

            var titleDetailsDTO = new TitleDetailsDTO
            {
                TConst = titleBasic.TConst,
                TitleType = titleBasic.TitleType,
                PrimaryTitle = titleBasic.PrimaryTitle,
                OriginalTitle = titleBasic.OriginalTitle,
                StartYear = titleBasic.StartYear,
                EndYear = titleBasic.EndYear,
                RunTimeMinutes = titleBasic.RunTimeMinutes,
                Awards = titleBasic.Awards,
                Plot = titleBasic.Plot,
                Rated = titleBasic.Rated,
                ReleaseDate = titleBasic.ReleaseDate,
                ProductionCompany = titleBasic.ProductionCompany,
                Poster = titleBasic.Poster,
                BoxOffice = titleBasic.BoxOffice,
                Genres = genres,
                Countries = countries,
                AverageRating = rating?.AverageRating,
                NumVotes = rating?.NumVotes
            };

            return Ok(titleDetailsDTO);
        }
    }
}