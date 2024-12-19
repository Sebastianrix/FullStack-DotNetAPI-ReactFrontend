using DataLayer;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TitlePrincipalController : BaseController
    {
        private readonly IDataService _dataService;

        public TitlePrincipalController(IDataService dataService, LinkGenerator linkGenerator)
            : base(linkGenerator)
        {
            _dataService = dataService;
        }

        [HttpGet("{tConst}/principals")]
        public ActionResult<IList<TitlePrincipal>> GetTitlePrincipals(string tConst)
        {
            try
            {
                var results = _dataService.GetTitlePrincipals(tConst);
                return Ok(results);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Controller: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("{nconst}/principals-name")]
        public ActionResult<PagedResponse<TitlePrincipal>> GetTitlePrincipalsName(string nconst, int pageNumber = 1, int pageSize = DefaultPageSize)
        {
            var titles = _dataService.GetTitlePrincipalsName(nconst);
            if (titles == null || !titles.Any())
            {
                return NotFound();
            }
            var totalItems = titles.Count();
            var pagedTitles = titles
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            foreach (var title in pagedTitles)
            {
                if (title.TConst != null)
                {
                    title.TConst = new Uri($"{Request.Scheme}://{Request.Host}/api/TitleBasic/{title.TConst}").ToString();
                }
            }
            var response = CreatePagedResponse(pagedTitles, pageNumber, pageSize, totalItems, "GetTitlePrincipalsName");
            return Ok(response);
        }
    }
}
