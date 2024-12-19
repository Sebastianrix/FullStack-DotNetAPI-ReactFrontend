using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DataLayer.Models;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace DataLayer
{
  public class DataService : IDataService
  {
    private readonly MovieDbContext _context;

    public DataService(IConfiguration configuration)
    {
      var options = new DbContextOptionsBuilder<MovieDbContext>()
          .UseNpgsql(configuration.GetConnectionString("imdbDatabase"))
          .Options;

      _context = new MovieDbContext(options);
    }

    // -- Helper Methods --

    private IList<T> GetPagedResults<T>(IQueryable<T> query, int pageNumber, int pageSize) where T : class =>
        query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

    private bool SaveChanges() => _context.SaveChanges() > 0;

    private bool Exists<T>(int id) where T : class =>
        _context.Set<T>().Find(id) != null;

    private T FindById<T>(int id) where T : class =>
        _context.Set<T>().Find(id);


    //Create user for authentication part
    public User CreateUser(string name, string username, string password, string email, string salt, string role)
    {
      var user = new User
      {
        //     Id = _context.Users.Max(x => x.Id) + 1,
        Name = name,
        Username = username,
        Password = password,
        Email = email,
        Salt = salt,
        Role = role
      };
      _context.Users.Add(user);
      SaveChanges();
      return user;
    }
    // -- USER --

    public User AddUser(string username, string password, string email)
    {


      var user = new User { Username = username, Password = password, Email = email };
      _context.Users.Add(user);
      SaveChanges();
      return user;
    }

    public User GetUser(string username) =>
        _context.Users.FirstOrDefault(u => u.Username == username);

    public User GetUser(int userId) =>
        FindById<User>(userId);

    public User GetUserByEmail(string email) =>
        _context.Users.FirstOrDefault(u => u.Email == email);

    public void DeleteUser(int userId)
    {
      var user = FindById<User>(userId);
      if (user != null)
      {
        _context.Users.Remove(user);
        SaveChanges();
      }
    }

    public bool UserExists(int userId) =>
        Exists<User>(userId);

    // -- BOOKMARK --

    public IList<Bookmark> GetBookmarks(int userId, int pageNumber = 1, int pageSize = 10)
    {
      var query = _context.Bookmarks.Where(b => b.UserId == userId).OrderBy(b => b.Id);
      return GetPagedResults(query, pageNumber, pageSize);
    }

    public int GetBookmarkCountByUser(int userId) =>
        _context.Bookmarks.Count(b => b.UserId == userId);

    public Bookmark GetBookmarkById(int bookmarkId) =>
        _context.Bookmarks.FirstOrDefault(b => b.Id == bookmarkId);

    public Bookmark GetBookmark(int userId, int bookmarkId) =>
        _context.Bookmarks.FirstOrDefault(b => b.UserId == userId && b.Id == bookmarkId);

    public Bookmark AddBookmark(int userId, string? tconst, string? nconst, string note)
    {
      ValidateUserExists(userId);

      if ((string.IsNullOrEmpty(tconst) && string.IsNullOrEmpty(nconst)) || (!string.IsNullOrEmpty(tconst) && !string.IsNullOrEmpty(nconst)))
        throw new ArgumentException("Specify either tconst or nconst, not both.");

      var bookmark = new Bookmark
      {
        UserId = userId,
        TConst = tconst,
        NConst = nconst,
        Note = note,
        CreatedAt = DateTime.UtcNow
      };
      _context.Bookmarks.Add(bookmark);
      SaveChanges();
      return bookmark;
    }

    public void UpdateBookmark(int userId, int bookmarkId, string tconst, string nconst, string note)
    {
      var bookmark = GetBookmark(userId, bookmarkId);
      if (bookmark != null)
      {
        bookmark.TConst = tconst;
        bookmark.NConst = nconst;
        bookmark.Note = note;
        bookmark.CreatedAt = DateTime.UtcNow;
        SaveChanges();
      }
    }

    public void DeleteBookmark(int bookmarkId)
    {
      var bookmark = FindById<Bookmark>(bookmarkId);
      if (bookmark != null)
      {
        _context.Bookmarks.Remove(bookmark);
        SaveChanges();
      }
    }

    // -- SEARCH HISTORY --

    public IList<SearchHistory> GetSearchHistory(int userId, int pageNumber = 1, int pageSize = 10)
    {
      var query = _context.SearchHistories.Where(sh => sh.UserId == userId).OrderBy(sh => sh.CreatedAt);
      return GetPagedResults(query, pageNumber, pageSize);
    }

    public SearchHistory GetSearchHistory(int searchId) =>
        FindById<SearchHistory>(searchId);

    public IList<SearchHistory> GetSearchHistoriesByUser(int userId, int pageNumber = 1, int pageSize = 10)
    {
      var query = _context.SearchHistories.Where(sh => sh.UserId == userId).OrderBy(sh => sh.CreatedAt);
      return GetPagedResults(query, pageNumber, pageSize);
    }

    public int GetSearchHistoryCountByUser(int userId) =>
        _context.SearchHistories.Count(sh => sh.UserId == userId);

    public SearchHistory AddSearchHistory(int userId, string searchQuery)
    {
      var searchHistory = new SearchHistory
      {
        UserId = userId,
        SearchQuery = searchQuery,
        CreatedAt = DateTime.UtcNow
      };
      _context.SearchHistories.Add(searchHistory);
      SaveChanges();
      return searchHistory;
    }

    public void DeleteSearchHistory(int searchId)
    {
      var searchHistory = FindById<SearchHistory>(searchId);
      if (searchHistory != null)
      {
        _context.SearchHistories.Remove(searchHistory);
        SaveChanges();
      }
    }

    // -- USER RATING --

    public IList<UserRating> GetUserRatings(int userId, int pageNumber = 1, int pageSize = 10)
    {
      var query = _context.UserRatings.Where(ur => ur.UserId == userId).OrderBy(ur => ur.Id);
      return GetPagedResults(query, pageNumber, pageSize);
    }

    public UserRating GetUserRating(int ratingId) =>
        FindById<UserRating>(ratingId);

    public UserRating GetUserRatingByUserAndTConst(int userId, string tconst) =>
        _context.UserRatings.FirstOrDefault(ur => ur.UserId == userId && ur.TConst == tconst);

    public int GetUserRatingCount(int userId) =>
        _context.UserRatings.Count(ur => ur.UserId == userId);

    public UserRating AddUserRating(int userId, string tconst, int rating)
    {
      var userRating = new UserRating
      {
        UserId = userId,
        TConst = tconst,
        Rating = rating,
        CreatedAt = DateTime.UtcNow
      };
      _context.UserRatings.Add(userRating);
      SaveChanges();
      return userRating;
    }

    public void UpdateUserRating(int ratingId, int rating)
    {
      var userRating = FindById<UserRating>(ratingId);
      if (userRating != null)
      {
        userRating.Rating = rating;
        userRating.CreatedAt = DateTime.UtcNow;
        SaveChanges();
      }
    }

    public void UpdateUserRating(int userId, int ratingId, int rating)
    {
      var userRating = GetUserRating(ratingId);
      if (userRating != null)
      {
        userRating.Rating = rating;
        userRating.CreatedAt = DateTime.UtcNow;
        SaveChanges();
      }
    }

    public void DeleteUserRating(int ratingId)
    {
      var userRating = FindById<UserRating>(ratingId);
      if (userRating != null)
      {
        _context.UserRatings.Remove(userRating);
        SaveChanges();
      }
    }

    // -- Private Helper Methods --

    private void ValidateUserExists(int userId)
    {
      if (!Exists<User>(userId))
        throw new ArgumentException("User with specified ID does not exist.");
    }
    // title 
    public TitleBasic GetTitleByTConst(string tConst)
    {
      return _context.TitleBasics.FirstOrDefault(tb => tb.TConst == tConst);
    }

    public IEnumerable<TitleGenre> GetGenresByTConst(string tConst)
    {
      return _context.TitleGenres.Where(tg => tg.TConst == tConst).ToList();
    }

    public IEnumerable<TitleCountry> GetCountriesByTConst(string tConst)
    {
      return _context.TitleCountries.Where(tc => tc.TConst == tConst).ToList();
    }

    public TitleRating GetRatingByTConst(string tConst)
    {
      return _context.TitleRatings.FirstOrDefault(tr => tr.TConst == tConst);
    }
    // coplayers
    public IList<CoPlayer> GetCoPlayers(string nConst)
    {
      return _context.CoPlayers.FromSqlInterpolated($"select * from coplayers({nConst})").ToList();
    }

    public IList<RatingActor> GetRatingActors(string tConst)
    {
      return _context.RatingActors.FromSqlInterpolated($"select * from ratingactors({tConst})").ToList();
    }
    public IList<RatingCoPlayer> GetRatingCoPlayers(string nConst)
    {
      return _context.RatingCoPlayers.FromSqlInterpolated($"select * from ratingcoplayers({nConst})").ToList();
    }
    public IList<RatingCrew> GetRatingCrew(string tConst)
    {
      return _context._RatingCrew.FromSqlInterpolated($"select * from ratingcrew({tConst})").ToList();
    }
    public IList<SimilarMovie> GetSimilarMovies(string tConst)
    {
      var queryResult = _context.SimilarMovies.FromSqlInterpolated($"select * from similarmovies({tConst})").ToList();
      foreach (var movie in queryResult)
      {
        if (movie.Poster == null)
        {
          movie.Poster = "null";
        }
      }
      return queryResult;
    }

    public IList<Top10Series> GetTop10Series()
    {
      var queryResult = _context.Top10Series.FromSqlInterpolated($"select * from top10series()").ToList();
      foreach (var title in queryResult)
      {
        if (title.Poster == null)
        {
          title.Poster = "null";
        }
      }
      return queryResult;
    }
    public IList<Top10Movies> GetTop10Movies()
    {
      var queryResult = _context.Top10Movies.FromSqlInterpolated($"select * from top10movies()").ToList();
      foreach (var title in queryResult)
      {
        if (title.Poster == null)
        {
          title.Poster = "null";
        }
      }
      return queryResult;
    }
    public IList<Top10Actors> GetTop10Actors()
    {
      return _context.Top10Actors.FromSqlInterpolated($"select * from top10actors()").ToList();
    }
    public IList<SearchName> GetSearchNames(string searchTerm)
    {
      return _context.SearchNames.FromSqlInterpolated($"select * from search_names_by_text_sorted({searchTerm})").ToList();
    }
    public IList<SearchTitle> GetSearchTitles(string searchTerm)
    {
      return _context.SearchTitles.FromSqlInterpolated($"select * from string_search({searchTerm})").ToList();
    }
    public IList<SearchTitleNumvote> GetSearchTitlesNumvote(string? searchTerm = "null", string? searchTitleType = "null", string? searchGenre = "null", int? searchYear = -1)
    {
      return _context.SearchTitleNumvotes.FromSqlInterpolated($"select * from filtered_search_numvotes({searchTerm},{searchTitleType},{searchGenre},{searchYear})").ToList();
    }
    public IList<SearchTitleRating> GetSearchTitlesRating(string? searchTerm = "null", string? searchTitleType = "null", string? searchGenre = "null", int? searchYear = -1)
    {
      return _context.SearchTitleRatings.FromSqlInterpolated($"select * from filtered_search_avgrating({searchTerm},{searchTitleType},{searchGenre},{searchYear})").ToList();
    }
    public IList<SearchTitleYear> GetSearchTitlesYear(string? searchTerm = "null", string? searchTitleType = "null", string? searchGenre = "null", int? searchYear = -1)
    {
      return _context.SearchTitleYears.FromSqlInterpolated($"select * from filtered_search_years({searchTerm},{searchTitleType},{searchGenre},{searchYear})").ToList();
    }
    public void rate(string tConst, int rating, int userId)
    {
      _context.Database.ExecuteSqlInterpolated($"CALL rate({tConst}, {rating}, {userId})");
    }
    public void rateDelete(int userRatingId, int userId)
        {
            _context.Database.ExecuteSqlInterpolated($"CALL rateDelete({userRatingId}, {userId})");
        }
    public IEnumerable<TitlePrincipal> GetTitlePrincipals(string tConst)
    {
      var results = new List<TitlePrincipal>();

      using (var command = _context.Database.GetDbConnection().CreateCommand())
      {
        command.CommandText = $"SELECT * FROM get_title_principals('{tConst}')";
        _context.Database.OpenConnection();

        using (var reader = command.ExecuteReader())
        {
          while (reader.Read())
          {
            var principal = new TitlePrincipal
            {
              TConst = reader.GetString(0),
              NConst = reader.GetString(1),
              Name = reader.GetString(2),
              Ordering = reader.GetInt32(3),
              Roles = reader.IsDBNull(4) ? null : reader.GetString(4)
            };
            results.Add(principal);
          }
        }
      }
      return results;
    }

    public IList<TitlePrincipal> GetTitlePrincipalsName(string nconst)
    {
      return _context.TitlePrincipals
                     .FromSqlInterpolated($"SELECT * FROM get_title_principals_name({nconst})")
                     .ToList();
    }

    // --Name-- (Actors, Directors, Writers)
    public NameBasic GetNameByNConst(string nconst)
    {
      return _context.NameBasics.FirstOrDefault(p => p.NConst == nconst);
    }

    public IList<NameBasic> GetAllNames(int pageNumber = 1, int pageSize = 10)
    {
      var query = _context.NameBasics.AsQueryable();
      return GetPagedResults(query, pageNumber, pageSize);
    }
    public int GetAllNamesCount()
    {
      return _context.NameBasics.Count();
    }


    public IList<TitleCharacter> GetTitleCharactersByName(string nconst)
    {
      return _context.TitleCharacters
                           .Include(tc => tc.TitleBasic)
                           .Where(tc => tc.NConst == nconst)
                           .ToList();
    }

    // --KNOWN FOR TITLES--
    public IList<KnownForTitle> GetKnownForTitlesByName(string nconst)
    {
      return _context.KnownForTitles
                    .Include(k => k.TitleBasic)
                     .Where(k => k.NConst == nconst)
                     .ToList();
    }
    public IList<GetGenreData> GetGenreData()
    {
      return _context.GetGenreData.FromSqlInterpolated($"select * from get_distinct_genres()").ToList();
    }
    public IList<GetYearData> GetYearData()
    {
      return _context.GetYearData.FromSqlInterpolated($"select * from get_distinct_start_years()").ToList();
    }
    public IList<GetTitleTypeData> GetTitleTypeData()
    {
      return _context.GetTitleTypeData.FromSqlInterpolated($"select * from get_distinct_title_types()").ToList();
    }
    public IList<SearchName> GetSearchNamesSorted(string searchTerm, string sortType)
    {
      return _context.SearchNames.FromSqlInterpolated($"select * from search_names_by_text_sorted({searchTerm},{sortType})").ToList();
    }
  }
}
