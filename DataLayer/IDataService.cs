using DataLayer.Models;
using System.Collections.Generic;
using System.Security.Principal;

namespace DataLayer
{
    public interface IDataService
    {
        // --USER--
        User AddUser(string username, string password, string email);
        User GetUser(string username);
        User GetUser(int userId);
        User GetUserByEmail(string email);
        void DeleteUser(int userId);
        bool UserExists(int userId);


        // --USER for with security --
        User CreateUser(string name, string username, string password, string email, string salt, string role);

        // --BOOKMARK--
        IList<Bookmark> GetBookmarks(int userId, int pageNumber = 1, int pageSize = 10);
        Bookmark GetBookmark(int userId, int bookmarkId);
        Bookmark GetBookmarkById(int bookmarkId);
        Bookmark AddBookmark(int userId, string tconst, string nconst, string note);
        void UpdateBookmark(int userId, int bookmarkId, string tconst, string nconst, string note);
        void DeleteBookmark(int bookmarkId);
        int GetBookmarkCountByUser(int userId);

        // --SEARCH HISTORY--
        IList<SearchHistory> GetSearchHistory(int userId, int pageNumber = 1, int pageSize = 10);
        IList<SearchHistory> GetSearchHistoriesByUser(int userId, int pageNumber = 1, int pageSize = 10);
        SearchHistory GetSearchHistory(int searchId);
        SearchHistory AddSearchHistory(int userId, string searchQuery);
        void DeleteSearchHistory(int searchId);
        int GetSearchHistoryCountByUser(int userId);

        // --USER RATING--
        IList<UserRating> GetUserRatings(int userId, int pageNumber = 1, int pageSize = 10);
        UserRating GetUserRating(int ratingId);
        UserRating GetUserRatingByUserAndTConst(int userId, string tconst);
        UserRating AddUserRating(int userId, string tconst, int rating);
        void DeleteUserRating(int ratingId);
        void UpdateUserRating(int userId, int ratingId, int rating);
        int GetUserRatingCount(int userId);


        // --TITLE BASIC--
        TitleBasic GetTitleByTConst(string tConst);
        IEnumerable<TitleGenre> GetGenresByTConst(string tConst);
        IEnumerable<TitleCountry> GetCountriesByTConst(string tConst);
        TitleRating GetRatingByTConst(string tConst);

        // --COPLAYYERS--
        IList<CoPlayer> GetCoPlayers(string nConst);
        IList<RatingActor> GetRatingActors(string tConst);
        IList<RatingCoPlayer> GetRatingCoPlayers(string nConst);
        IList<RatingCrew> GetRatingCrew(string tConst);
        IList<SimilarMovie> GetSimilarMovies(string tConst);
        IList<SearchName> GetSearchNames(string searchTerm);
        IList<SearchTitle> GetSearchTitles(string searchTerm);
        IList<Top10Series> GetTop10Series();
        IList<Top10Movies> GetTop10Movies();
        IList<Top10Actors> GetTop10Actors();
        IList<SearchTitleNumvote> GetSearchTitlesNumvote(string? searchTerm = "null", string? searchTitleType = "null", string? searchGenre = "null", int? searchYear = -1);
        IList<SearchTitleRating> GetSearchTitlesRating(string? searchTerm = "null", string? searchTitleType = "null", string? searchGenre = "null", int? searchYear = -1);
        IList<SearchTitleYear> GetSearchTitlesYear(string? searchTerm = "null", string? searchTitleType = "null", string? searchGenre = "null", int? searchYear = -1);
        IList<GetGenreData> GetGenreData();
        IList<GetYearData> GetYearData();
        IList<GetTitleTypeData> GetTitleTypeData();
        void rate(string tConst, int rating, int userId);
        void rateDelete(int userRatingId, int userId);
        // --Name--
        NameBasic GetNameByNConst(string nconst);
        IList<NameBasic> GetAllNames(int pageNumber = 1, int pageSize = 10);
        int GetAllNamesCount();


        IList<KnownForTitle> GetKnownForTitlesByName(string nconst);


        // --TITLE CHARACTERS--
        IList<TitleCharacter> GetTitleCharactersByName(string nconst);


        // --TITLE PRINCIPALS--
        // IList<TitlePrincipal> GetTitlePrincipalsByName(string nconst);
        // IList<TitlePrincipal> GetTitlePrincipalsByTitle(string tconst);
        IEnumerable<TitlePrincipal> GetTitlePrincipals(string tConst);
        IList<TitlePrincipal> GetTitlePrincipalsName(string nconst);

        IList<SearchName> GetSearchNamesSorted(string searchTerm, string sortType);

    }
}