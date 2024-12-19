using WebApi.DTOs;
using System.Linq;
using DataLayer;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KnownForTitleController : BaseController
    {
        private readonly IDataService _dataService;

        public KnownForTitleController(IDataService dataService, LinkGenerator linkGenerator)
            : base(linkGenerator)
        {
            _dataService = dataService;
        }

        // GET: api/KnownForTitle/{nconst}
        [HttpGet("{nconst}", Name = "GetKnownForTitlesByName")]
        public ActionResult<IList<KnownForTitleDto>> GetKnownForTitlesByName(string nconst, int page = 1, int pageSize = DefaultPageSize)
        {
            var knownForTitles = _dataService.GetKnownForTitlesByName(nconst);

            var knownForTitleDtos = knownForTitles.Select(kft => new KnownForTitleDto
            {
                NConst = kft.NConst,
                KnownForTitles = kft.KnownForTitles,
                PrimaryTitle = kft.TitleBasic?.PrimaryTitle,
                Poster = kft.TitleBasic?.Poster
            }).ToList();

            var result = CreatePagingNConst("GetKnownForTitlesByName", nconst, page, pageSize, knownForTitleDtos.Count, knownForTitleDtos.Skip((page - 1) * pageSize).Take(pageSize));
            return Ok(result);
        }
    }
}