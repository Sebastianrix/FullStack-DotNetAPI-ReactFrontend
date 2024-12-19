using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;
using DataLayer;
using Mapster;
using System;
using DataLayer.Models;
using Microsoft.AspNetCore.Components.Web;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SimilarMoviesController : BaseController
    {
        private readonly IDataService _dataService;

        public SimilarMoviesController(IDataService dataService, LinkGenerator linkGenerator)
          : base(linkGenerator)
        {
            _dataService = dataService;
        }


        // --  --
        [HttpGet("{tConst}")]
        public ActionResult<PagedResponse<SimilarMovie>> GetSimilarMovies(string tConst, int pageNumber = 1, int pageSize = DefaultPageSize)
        {
            var similarMovies = _dataService.GetSimilarMovies(tConst);
            if (similarMovies == null || !similarMovies.Any())
            {
                return NotFound();
            }

            var totalItems = similarMovies.Count();
            var pagedSimilarMovies = similarMovies
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            foreach (var title in pagedSimilarMovies)
            {
                if (title.TConst != null)
                {
                    title.TConst = new Uri($"{Request.Scheme}://{Request.Host}/api/Title/{title.TConst}").ToString();
                }
            }

            var response = CreatePagedResponse(pagedSimilarMovies, pageNumber, pageSize, totalItems, "GetSimilarMovies");

            return Ok(response);
        }
    }
}