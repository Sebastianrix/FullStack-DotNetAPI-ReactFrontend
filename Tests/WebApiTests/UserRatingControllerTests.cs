using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Xunit;
using cit13_portfolioproject2.Tests;


namespace cit13_portfolioproject2.WebApiTests.UserRatingControllerTests
{
  public class UserRatingControllerTests
  {
    private const string UserRatingsApi = "http://localhost:5002/api/userrating";
    private static readonly HttpClient client = new HttpClient();

    [Fact]
    public async Task ApiUserRatings_GetUserRatingsWithValidUserId_OkAndRatings()
    {
      int userId = 1;
      var (ratings, statusCode) = await HelperTest.GetObject($"{UserRatingsApi}/{userId}");

      Assert.Equal(HttpStatusCode.OK, statusCode);
      Assert.NotNull(ratings);
    }

    [Fact]
    public async Task ApiUserRatings_GetUserRatingsWithInvalidUserId_NotFound()
    {
      int userId = 999;
      var (_, statusCode) = await HelperTest.GetObject($"{UserRatingsApi}/{userId}");

      Assert.Equal(HttpStatusCode.NotFound, statusCode);
    }

    [Fact]
    public async Task ApiUserRatings_GetUserRatingByIdWithValidIds_OkAndRating()
    {
      int userId = 1;
      int ratingId = 1;
      var (rating, statusCode) = await HelperTest.GetObject($"{UserRatingsApi}/{userId}/{ratingId}");

      Assert.Equal(HttpStatusCode.OK, statusCode);
      Assert.NotNull(rating);
    }

    [Fact]
    public async Task ApiUserRatings_GetUserRatingByIdWithInvalidIds_NotFound()
    {
      int userId = 1;
      int ratingId = 999;
      var (_, statusCode) = await HelperTest.GetObject($"{UserRatingsApi}/{userId}/{ratingId}");

      Assert.Equal(HttpStatusCode.NotFound, statusCode);
    }

    [Fact]
    public async Task ApiUserRatings_AddUserRating_ValidData_Created()
    {
      var newRating = new
      {
        UserId = 1,
        TConst = "tt16120138",
        Rating = 8,
        Name = "test"
      };
      var (rating, statusCode) = await HelperTest.PostData($"{UserRatingsApi}", newRating);

      Assert.Equal(HttpStatusCode.Created, statusCode);
      Assert.NotNull(rating);
      Assert.Equal(newRating.UserId, rating?.ValueInt("userId"));
      Assert.Equal(newRating.TConst, rating?.Value("tConst"));
      Assert.Equal(newRating.Rating, rating?.ValueInt("rating"));

      // Clean up after test
      string? id = rating?["id"]?.ToString();
      if (id != null)
      {
        await HelperTest.DeleteData($"{UserRatingsApi}/{newRating.UserId}/{id}");
      }
    }

    [Fact]
    public async Task ApiUserRatings_AddUserRating_InvalidData_BadRequest()
    {
      var invalidRating = new
      {
        UserId = 1,
        TConst = "",
        Rating = 11
      };
      var (_, statusCode) = await HelperTest.PostData($"{UserRatingsApi}", invalidRating);

      Assert.Equal(HttpStatusCode.BadRequest, statusCode);
    }

    [Fact]
    public async Task ApiUserRatings_DeleteUserRatingWithValidIds_NoContent()
    {
      var newRating = new
      {
        UserId = 1,
        TConst = "tt16120138",
        Rating = 8
      };
      var (rating, _) = await HelperTest.PostData($"{UserRatingsApi}", newRating);

      string? deleteUrl = $"{UserRatingsApi}/{newRating.UserId}/{rating?["id"]?.ToString()}";
      var statusCode = await HelperTest.DeleteData(deleteUrl);

      Assert.Equal(HttpStatusCode.NoContent, statusCode);
    }

    [Fact]
    public async Task ApiUserRatings_DeleteUserRatingWithInvalidIds_NotFound()
    {
      var statusCode = await HelperTest.DeleteData($"{UserRatingsApi}/1/999");

      Assert.Equal(HttpStatusCode.NotFound, statusCode);
    }

    [Fact]
    public async Task ApiUserRatings_UpdateUserRating_ValidData_NoContent()
    {
      var newRating = new
      {
        UserId = 1,
        TConst = "tt16120138",
        Rating = 8
      };
      var (rating, _) = await HelperTest.PostData($"{UserRatingsApi}", newRating);

      var updatedRating = new
      {
        UserId = 1,
        TConst = "tt16120138",
        Rating = 9
      };
      string? updateUrl = $"{UserRatingsApi}/{newRating.UserId}/{rating?["id"]?.ToString()}";
      var statusCode = await HelperTest.PutDataStatusOnly(updateUrl, updatedRating);

      Assert.Equal(HttpStatusCode.NoContent, statusCode);

      // Clean up after test
      await HelperTest.DeleteData(updateUrl);
    }
  }
}