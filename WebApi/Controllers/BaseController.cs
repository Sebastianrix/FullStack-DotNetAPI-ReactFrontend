using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using static WebApi.Controllers.SearchController;

namespace WebApi.Controllers
{
  [ApiController]
  public class BaseController : ControllerBase
  {
    private readonly LinkGenerator _linkGenerator;
    protected const int DefaultPageSize = 10;
    private const int MaxPageSize = 25;

    public BaseController(LinkGenerator linkGenerator)
    {
      _linkGenerator = linkGenerator;
    }

    protected string? GetUrl(string linkName, object args)
    {
      var uri = _linkGenerator.GetUriByName(HttpContext, linkName, args);
      return uri;
    }

    protected string? GetLinkUser(string linkName, int userId, int page, int pageSize)
    {
      return GetUrl(linkName, new { userId, page, pageSize });
    }

    protected string? GetLinkNConst(string linkName, string nconst, int page, int pageSize)
    {
      return GetUrl(linkName, new { nconst, page, pageSize });
    }

    protected object CreatePagingUser<T>(string linkName, int userId, int page, int pageSize, int total, IEnumerable<T?> items)
    {
      const int MaxPageSize = 25;
      pageSize = pageSize > MaxPageSize ? MaxPageSize : pageSize;
      var numberOfPages = (int)Math.Ceiling(total / (double)pageSize);

      var curPage = GetLinkUser(linkName, userId, page, pageSize);
      var nextPage = page < numberOfPages - 1 ? GetLinkUser(linkName, userId, page + 1, pageSize) : null;
      var prevPage = page > 1 ? GetLinkUser(linkName, userId, page - 1, pageSize) : null;

      var result = new
      {
        CurPage = curPage,
        NextPage = nextPage,
        PrevPage = prevPage,
        NumberOfItems = total,
        NumberPages = numberOfPages,
        Items = items
      };

      return result;
    }

    protected object CreatePagingNConst<T>(string linkName, string nconst, int page, int pageSize, int total, IEnumerable<T?> items)
    {
      pageSize = pageSize > MaxPageSize ? MaxPageSize : pageSize;
      var numberOfPages = (int)Math.Ceiling(total / (double)pageSize);

      // Current page link
      var curPage = GetLinkNConst(linkName, nconst, page, pageSize);

      // Next page link (only if there is a next page)
      var nextPage = page < numberOfPages ? GetLinkNConst(linkName, nconst, page + 1, pageSize) : null;

      // Previous page link (only if there is a previous page)
      var prevPage = page > 1 ? GetLinkNConst(linkName, nconst, page - 1, pageSize) : null;

      var result = new
      {
        CurPage = curPage,
        NextPage = nextPage,
        PrevPage = prevPage,
        NumberOfItems = total,
        NumberPages = numberOfPages,
        Items = items
      };

      return result;
    }

    protected string? GetLink(string linkName, int page, int pageSize)
    {
      return GetUrl(linkName, new { page, pageSize });
    }

    protected object CreatePaging<T>(string linkName, int page, int pageSize, int total, IEnumerable<T?> items)
    {
      // Ensure the pageSize doesn't exceed the maximum
      pageSize = pageSize > MaxPageSize ? MaxPageSize : pageSize;
      var numberOfPages = (int)Math.Ceiling(total / (double)pageSize);

      // Generate current, next, and previous page links
      var curPage = GetLink(linkName, page, pageSize);
      var nextPage = page < numberOfPages ? GetLink(linkName, page + 1, pageSize) : null;
      var prevPage = page > 1 ? GetLink(linkName, page - 1, pageSize) : null;

      var result = new
      {
        CurPage = curPage,
        NextPage = nextPage,
        PrevPage = prevPage,
        NumberOfItems = total,
        NumberPages = numberOfPages,
        Items = items
      };

      return result;
    }
    protected string? GenerateSelfLink(string actionName, object routeValues)
    {
      return Url.Action(actionName, routeValues);
    }


        //funcion paging

        protected PagedResponse<T> CreatePagedResponse<T>(IEnumerable<T> items, int pageNumber, int pageSize, int totalItems, string actionName)
        {
            pageSize = pageSize > MaxPageSize ? MaxPageSize : pageSize;
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var selfLink = GenerateFullLink(actionName, new { pageNumber, pageSize });
            var nextPageLink = pageNumber < totalPages ? GenerateFullLink(actionName, new { pageNumber = pageNumber + 1, pageSize }) : null;
            var prevPageLink = pageNumber > 1 ? GenerateFullLink(actionName, new { pageNumber = pageNumber - 1, pageSize }) : null;

            return new PagedResponse<T>
            {
                numberOfItems = totalItems,
                numberPages = totalPages,
                curPage = selfLink,
                nextPage = nextPageLink,
                prevPage = prevPageLink,
                Items = items
            };
        }
        protected string GenerateFullLink(string actionName, object routeValues)
        {
            var uri = new Uri($"{Request.Scheme}://{Request.Host}{Url.Action(actionName, routeValues)}");
            return uri.ToString();
        }
        public class PagedResponse<T>
        {
            public string? curPage { get; set; }
            public string? nextPage { get; set; }
            public string? prevPage { get; set; }
            public int numberOfItems { get; set; }
            public int numberPages { get; set; }
            public IEnumerable<T>? Items { get; set; }
        }
    }
}