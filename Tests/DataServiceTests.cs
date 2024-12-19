using DataLayer;
using DataLayer.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Xunit;

namespace DataLayer.Tests
{
  public class DataServiceTests
  {
    private IConfiguration configuration;
    private DataService service;

    public DataServiceTests()
    {
      var inMemorySettings = new Dictionary<string, string?>
            {
                {"ConnectionStrings:imdbDatabase", "Host=localhost;Database=imdb;Username=postgres;Password=postgres"}
            };

      configuration = new ConfigurationBuilder()
          .AddInMemoryCollection(inMemorySettings)
          .Build();

      service = new DataService(configuration);
    }

    /* User Tests */

    [Fact]
    public void AddUser_ValidData_ReturnsCreatedUser()
    {
      var newUser = service.AddUser("newUser", "password", "newUser@example.com");
      Assert.True(newUser.Id > 0);
      Assert.Equal("newUser", newUser.Username);

      var user = service.GetUser(newUser.Id);
      Assert.NotNull(user);
      Assert.Equal(newUser.Username, user.Username);
      Assert.Equal(newUser.Id, user.Id);

      // cleanup
      service.DeleteUser(newUser.Id);

      var deletedUser = service.GetUser(newUser.Id);
      Assert.Null(deletedUser);
    }

    [Fact]
    public void GetUser_InvalidId_ReturnsNull()
    {
      var user = service.GetUser(-1);
      Assert.Null(user);
    }

    /* UserRating Tests */

    [Fact]
    public void AddUserRating_ValidData_ReturnsCreatedRating()
    {
            var _newUser = new
            {
                Username = "RatingUser",
                Password = "password123",
                Email = "normal@example.com",
                Role = "user",
                Name = "Rate Rater",

            };
            // Ensure the user exists
          var newUser = service.CreateUser(_newUser.Name, _newUser.Username, _newUser.Password, _newUser.Email, "salt", _newUser.Role);

            Assert.True(newUser.Id > 0);

             service.rate("tt26919084", 5, newUser.Id);
     

            var ratings = service.GetUserRatings(newUser.Id);
            Assert.NotEmpty(ratings);
            Assert.Equal(5, ratings.First().Rating);

            //// Cleanup
            //service.DeleteUserRating(rating.Id);
            //service.DeleteUser(newUser.Id);

            //var deletedRating = service.GetUserRating(rating.Id);
            //Assert.Null(deletedRating);

            //service.DeleteUser(newUser.Id);
        }

    /* SearchHistory Tests */

    [Fact]
    public void AddSearchHistory_ValidQuery_ReturnsSearchHistory()
    {
      var newUser = service.AddUser("historyUser", "password", "historyUser@example.com");
      Assert.True(newUser.Id > 0);

      var history = service.AddSearchHistory(newUser.Id, "testQuery");
      Assert.Equal("testQuery", history.SearchQuery);

      var historyList = service.GetSearchHistoriesByUser(newUser.Id);

      Assert.NotEmpty(historyList);
      Assert.Equal("testQuery", historyList.First().SearchQuery);

      // cleanup
      service.DeleteSearchHistory(history.Id);

      var deletedHistory = service.GetSearchHistory(history.Id);
      Assert.Null(deletedHistory);

      service.DeleteUser(newUser.Id);
    }

    /* UserBookmark Tests */

    [Fact]
    public void AddUserBookmark_ValidData_CreatesAndReturnsBookmark()
    {

      var newUser = service.AddUser("bookmarkUser", "password", "bookmarkUser@example.com");
      Assert.True(newUser.Id > 0);

      var bookmark = service.AddBookmark(newUser.Id, "tt26919084", null, "Test note");
      Assert.True(bookmark.Id > 0);
      Assert.Equal("Test note", bookmark.Note);

      var retrievedBookmark = service.GetBookmark(newUser.Id, bookmark.Id);
      Assert.Equal(bookmark.Id, retrievedBookmark.Id);
      Assert.Equal("Test note", retrievedBookmark.Note);

      // Cleanup
      service.DeleteBookmark(bookmark.Id);

      var deletedBookmark = service.GetBookmark(newUser.Id, bookmark.Id);
      Assert.Null(deletedBookmark);

      var bookmark1 = service.AddBookmark(newUser.Id, null, "nm0000045", "Note 1");
      var bookmark2 = service.AddBookmark(newUser.Id, "tt26919084", null, "Note 2");

      var bookmarks = service.GetBookmarks(newUser.Id);

      Assert.Contains(bookmarks, b => b.Note == "Note 1");
      Assert.Contains(bookmarks, b => b.Note == "Note 2");

      // Cleanup
      service.DeleteBookmark(bookmark1.Id);
      service.DeleteBookmark(bookmark2.Id);

      service.DeleteUser(newUser.Id);
    }
    /* NameBasic Tests */

    [Fact]
    public void GetName_ValidID()
    {
        var name = service.GetNameByNConst("nm0000138");
        Assert.NotNull(name);
        Assert.Equal("Leonardo DiCaprio", name.ActualName);


    }
        [Fact]
        public void GetName_List_Valid()
        {
            var names = service.GetAllNames();
            Assert.NotEmpty(names);
        }
        /* TitleCharacter Tests*/
    [Fact]
    public void GetTitleCharacter_ValidID()
    {
        var characters = service.GetTitleCharactersByName("nm0000138");
        Assert.NotEmpty(characters);
        }
        /* TitlePrincipal Tests*/
    [Fact]
        public void GetPrincipal_ValidID()
        {
            var principals = service.GetTitlePrincipals("nm0000138");
            Assert.NotEmpty(principals);
        }
        /* KnownForTitle Tests */
    [Fact]
        public void GetKownForTitle_ValidID()
        {
            var knownForTitles = service.GetKnownForTitlesByName("nm0000138");
            Assert.NotEmpty(knownForTitles);
        }
    }
}