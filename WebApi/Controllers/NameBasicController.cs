using DataLayer;
using DataLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NameBasicController : BaseController
    {
        private readonly IDataService _dataService;

        public NameBasicController(IDataService dataService, LinkGenerator linkGenerator)
            : base(linkGenerator)
        {
            _dataService = dataService;
        }

        // GET: api/NameBasic/{nconst}
        [HttpGet("{nconst}")]
        public ActionResult<NameBasic> GetNameBasicByNConst(string nconst)
        {
            var nameBasic = _dataService.GetNameByNConst(nconst);
            if (nameBasic == null)
            {
                return NotFound();
            }
            return Ok(nameBasic);
        }

        // GET: api/NameBasic
        [HttpGet(Name = "GetAllNameBasics")]
        public ActionResult<IList<NameBasic>> GetAllNameBasics(int pageNumber = 1, int pageSize = DefaultPageSize)
        {
            var allNames = _dataService.GetAllNames();
            var totalItems = _dataService.GetAllNamesCount();
            foreach (var name in allNames)
            {
                name.NConst = new Uri($"{Request.Scheme}://{Request.Host}/api/namebasic/{name.NConst}").ToString();
            }

            var pagedResponse = CreatePagedResponse(allNames, pageNumber, pageSize, totalItems, "GetAllNameBasics");
            return Ok(pagedResponse);
        }
    }
}
