// using DataLayer;
// using DataLayer.Models;
// using Microsoft.AspNetCore.Mvc;
// using System.Collections.Generic;

// namespace WebApi.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     public class TitleCharacterController : ControllerBase
//     {
//         private readonly IDataService _dataService;

//         public TitleCharacterController(IDataService dataService)
//         {
//             _dataService = dataService;
//         }

//         // GET: api/TitleCharacter/{nconst}
// [HttpGet("{nconst}")]
// public ActionResult<IList<TitleCharacter>> GetTitleCharactersByName(string nconst)
// {
//     return Ok(_dataService.GetTitleCharactersByName(nconst));
// }


//     }
// }


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
    public class TitleCharacterController : BaseController
    {
        private readonly IDataService _dataService;


        public TitleCharacterController(IDataService dataService, LinkGenerator linkGenerator)
        : base(linkGenerator)
        {
            _dataService = dataService;
        }
        [HttpGet("{nconst}", Name = "GetTitleCharactersByName")]
        public ActionResult<IList<TitleCharacterDto>> GetTitleCharactersByName(string nconst, int page = 1, int pageSize = DefaultPageSize)
        {
            var titleCharacters = _dataService.GetTitleCharactersByName(nconst);

            var titleCharacterDtos = titleCharacters.Select(tc => new TitleCharacterDto
            {
                NConst = tc.NConst,
                TConst = tc.TConst,
                Character = tc.Character,
                PrimaryTitle = tc.TitleBasic?.PrimaryTitle,
                Poster = tc.TitleBasic?.Poster

            }).ToList();

            var result = CreatePagingNConst("GetTitleCharactersByName", nconst, page, pageSize, titleCharacterDtos.Count, titleCharacterDtos.Skip((page - 1) * pageSize).Take(pageSize));
            return Ok(result);
        }
    }
}