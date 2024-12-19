using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Xunit;
using cit13_portfolioproject2.Tests;

namespace cit13_portfolioproject2.WebApiTests
{
  public class SearchHistoryControllerTests
  {
    private const string SearchHistoryApi = "http://localhost:5002/api/searchhistory";
    private static readonly HttpClient client = new HttpClient();

    /* /api/searchhistory */

    [Fact]
    public async Task ApiSearchHistory_GetWithValidSearchId_OkAndSearchHistory()
    {
      int searchId = 1;
      var (searchHistory, statusCode) = await HelperTest.GetObject($"{SearchHistoryApi}/{searchId}");

      Assert.Equal(HttpStatusCode.OK, statusCode);
      Assert.Equal(searchId, searchHistory?.ValueInt("id"));
    }

    [Fact]
    public async Task ApiSearchHistory_GetWithInvalidSearchId_NotFound()
    {
      int searchId = 999;
      var (_, statusCode) = await HelperTest.GetObject($"{SearchHistoryApi}/{searchId}");

      Assert.Equal(HttpStatusCode.NotFound, statusCode);
    }

    [Fact]
    public async Task ApiSearchHistory_GetWithValidUserId_OkAndPaginatedSearchHistories()
    {
      int userId = 1;
      int pageNumber = 1;
      int pageSize = 10;

      var (data, statusCode) = await HelperTest.GetArray($"{SearchHistoryApi}/user/{userId}?pageNumber={pageNumber}&pageSize={pageSize}");

      Assert.Equal(HttpStatusCode.OK, statusCode);
      Assert.NotNull(data);
      Assert.True(data?.Count > 0);
    }

    [Fact]
    public async Task ApiSearchHistory_GetWithInvalidUserId_NotFound()
    {
      int userId = 999;
      var (_, statusCode) = await HelperTest.GetArray($"{SearchHistoryApi}/user/{userId}");

      Assert.Equal(HttpStatusCode.NotFound, statusCode);
    }

    [Fact]
    public async Task ApiSearchHistory_PostWithValidData_Created()
    {
      var newSearchHistory = new
      {
        UserId = 1,
        SearchQuery = "sample query"
      };

      var (searchHistory, statusCode) = await HelperTest.PostData(SearchHistoryApi, newSearchHistory);

      Assert.Equal(HttpStatusCode.Created, statusCode);
      Assert.NotNull(searchHistory);
      Assert.Equal(newSearchHistory.UserId, searchHistory?.ValueInt("userId"));
      Assert.Equal(newSearchHistory.SearchQuery, searchHistory?.Value("searchQuery"));

      // Clean up after test
      string? id = searchHistory?["id"]?.ToString();
      if (id != null)
      {
        await HelperTest.DeleteData($"{SearchHistoryApi}/{id}");
      }
    }

    [Fact]
    public async Task ApiSearchHistory_PostWithInvalidData_BadRequest()
    {
      var invalidSearchHistory = new
      {
        UserId = 1,
        SearchQuery = ""
      };

      var (response, statusCode) = await HelperTest.PostData(SearchHistoryApi, invalidSearchHistory);

      Assert.Equal(HttpStatusCode.BadRequest, statusCode);
    }

    [Fact]
    public async Task ApiSearchHistory_DeleteWithValidSearchId_NoContent()
    {
      var newSearchHistory = new
      {
        UserId = 1,
        SearchQuery = "sample query"
      };

      var (searchHistory, _) = await HelperTest.PostData(SearchHistoryApi, newSearchHistory);

      // Extract selfLink from the searchHistory for deletion
      string? deleteUrl = searchHistory?["selfLink"]?.ToString();

      var statusCode = await HelperTest.DeleteData(deleteUrl);

      Assert.Equal(HttpStatusCode.NoContent, statusCode);
    }
  }
}