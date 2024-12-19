using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Models;

namespace DataLayer;

public class MovieDbContext : DbContext
{
  public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options)
  {
  }

  public DbSet<User> Users { get; set; }
  public DbSet<Bookmark> Bookmarks { get; set; }
  public DbSet<UserRating> UserRatings { get; set; }
  public DbSet<SearchHistory> SearchHistories { get; set; }
  public DbSet<CoPlayer> CoPlayers { get; set; }
  public DbSet<RatingActor> RatingActors { get; set; }
  public DbSet<RatingCoPlayer> RatingCoPlayers { get; set; }
  public DbSet<RatingCrew> _RatingCrew { get; set; }
  public DbSet<SimilarMovie> SimilarMovies { get; set; }
  public DbSet<NameBasic> NameBasics { get; set; }
  public DbSet<KnownForTitle> KnownForTitles { get; set; }
  public DbSet<TitleCharacter> TitleCharacters { get; set; }
  public DbSet<TitlePrincipal> TitlePrincipals { get; set; }
  public DbSet<TitleBasic> TitleBasics { get; set; }
  public DbSet<TitleCountry> TitleCountries { get; set; }
  public DbSet<TitleEpisode> TitleEpisodes { get; set; }
  public DbSet<TitleGenre> TitleGenres { get; set; }
  public DbSet<TitleRating> TitleRatings { get; set; }
  public DbSet<TitleLanguage> TitleLanguages { get; set; }
  public DbSet<TitleAka> TitleAkas { get; set; }
  public DbSet<SearchName> SearchNames { get; set; }
  public DbSet<SearchTitle> SearchTitles { get; set; }
  public DbSet<Top10Actors> Top10Actors { get; set; }
  public DbSet<Top10Movies> Top10Movies { get; set; }
  public DbSet<Top10Series> Top10Series { get; set; }
  public DbSet<SearchTitleRating> SearchTitleRatings { get; set; }
  public DbSet<SearchTitleNumvote> SearchTitleNumvotes { get; set; }
  public DbSet<GetGenreData> GetGenreData { get; set; }
  public DbSet<GetTitleTypeData> GetTitleTypeData { get; set; }
  public DbSet<GetYearData> GetYearData { get; set; }
  public DbSet<SearchTitleYear> SearchTitleYears { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    MapUsers(modelBuilder);
    MapBookmarks(modelBuilder);
    MapUserRatings(modelBuilder);
    MapSearchHistories(modelBuilder);
    MapTitleBasic(modelBuilder);
    MapCoPlayer(modelBuilder);
    MapRatingActor(modelBuilder);
    MapRatingCoPlayers(modelBuilder);
    MapRatingCrew(modelBuilder);
    MapSimilarMovies(modelBuilder);
    MapNameBasic(modelBuilder);
    MapKnownForTitles(modelBuilder);
    MapTitleCharacters(modelBuilder);
    MapTitlePrincipals(modelBuilder);
    MapTitleBasic(modelBuilder);
    MapTitleCountries(modelBuilder);
    MapTitleEpisodes(modelBuilder);
    MapTitleGenres(modelBuilder);
    MapTitleRating(modelBuilder);
    MapTitleLanguages(modelBuilder);
    MapTitleAkas(modelBuilder);
    MapSearchNames(modelBuilder);
    MapSearchTitles(modelBuilder);
    MapTop10Actors(modelBuilder);
    MapTop10Movies(modelBuilder);
    MapTop10Series(modelBuilder);
    MapSearchTitleNumvotes(modelBuilder);
    MapSearchTitleRatings(modelBuilder);
    MapGetGenreData(modelBuilder);
    MapGetTitleTypeData(modelBuilder);
    MapGetYearData(modelBuilder);
    MapSearchTitleYears(modelBuilder);
    }
  //User Table Mapping
  private static void MapUsers(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<User>().ToTable("users");
    modelBuilder.Entity<User>().HasKey(u => u.Id);
    modelBuilder.Entity<User>().Property(u => u.Id).HasColumnName("userid");
    modelBuilder.Entity<User>().Property(u => u.Username).HasColumnName("username");
    modelBuilder.Entity<User>().Property(u => u.Email).HasColumnName("email");
    modelBuilder.Entity<User>().Property(u => u.Password).HasColumnName("password");
    modelBuilder.Entity<User>().Property(u => u.Salt).HasColumnName("salt");
    modelBuilder.Entity<User>().Property(u => u.Role).HasColumnName("role");
    modelBuilder.Entity<User>().Property(u => u.Name).HasColumnName("name");
  }

  //Bookmark Table Mapping
  private static void MapBookmarks(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Bookmark>().ToTable("userbookmarks");
    modelBuilder.Entity<Bookmark>().HasKey(b => b.Id);
    modelBuilder.Entity<Bookmark>().Property(b => b.Id).HasColumnName("bookmarkid");
    modelBuilder.Entity<Bookmark>().Property(b => b.UserId).HasColumnName("userid");
    modelBuilder.Entity<Bookmark>().Property(b => b.TConst).HasColumnName("tconst");
    modelBuilder.Entity<Bookmark>().Property(b => b.NConst).HasColumnName("nconst");
    modelBuilder.Entity<Bookmark>().Property(b => b.Note).HasColumnName("note");
    modelBuilder.Entity<Bookmark>().Property(b => b.CreatedAt).HasColumnName("bookmarkdate");
  }

  //User Ratings Table Mapping
  private static void MapUserRatings(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<UserRating>().ToTable("userratings");
    modelBuilder.Entity<Bookmark>().HasKey(ur => ur.Id);
    modelBuilder.Entity<UserRating>().Property(u => u.Id).HasColumnName("userratingid");
    modelBuilder.Entity<UserRating>().Property(u => u.UserId).HasColumnName("userid");
    modelBuilder.Entity<UserRating>().Property(u => u.TConst).HasColumnName("tconst");
    modelBuilder.Entity<UserRating>().Property(u => u.Rating).HasColumnName("rating");
    modelBuilder.Entity<UserRating>().Property(u => u.CreatedAt).HasColumnName("ratingdate");
  }

  //Search History Table Mapping
  private static void MapSearchHistories(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<SearchHistory>().ToTable("usersearchhistory");
    modelBuilder.Entity<SearchHistory>().HasKey(sh => sh.Id);
    modelBuilder.Entity<SearchHistory>().Property(s => s.Id).HasColumnName("searchid");
    modelBuilder.Entity<SearchHistory>().Property(s => s.UserId).HasColumnName("userid");
    modelBuilder.Entity<SearchHistory>().Property(s => s.SearchQuery).HasColumnName("searchquery");
    modelBuilder.Entity<SearchHistory>().Property(s => s.CreatedAt).HasColumnName("searchdate");
  }

  // Name Table Mapping
  private static void MapNameBasic(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<NameBasic>().ToTable("namebasic");
    modelBuilder.Entity<NameBasic>().Property(p => p.NConst).HasColumnName("nconst");
    modelBuilder.Entity<NameBasic>().Property(p => p.BirthYear).HasColumnName("birthyear");
    modelBuilder.Entity<NameBasic>().Property(p => p.DeathYear).HasColumnName("deathyear");
    modelBuilder.Entity<NameBasic>().Property(p => p.ActualName).HasColumnName("primaryname");
    modelBuilder.Entity<NameBasic>().Property(p => p.NRating).HasColumnName("nrating");
  }

  // MapTitleCharacters method
  private static void MapTitleCharacters(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<TitleCharacter>().ToTable("titlecharacters");
    modelBuilder.Entity<TitleCharacter>().HasKey(tc => new { tc.NConst, tc.TConst });
    modelBuilder.Entity<TitleCharacter>().Property(tc => tc.NConst).HasColumnName("nconst");
    modelBuilder.Entity<TitleCharacter>().Property(tc => tc.TConst).HasColumnName("tconst");
    modelBuilder.Entity<TitleCharacter>().Property(tc => tc.Character).HasColumnName("character");
    modelBuilder.Entity<TitleCharacter>().Property(tc => tc.Ordering).HasColumnName("ordering");

    modelBuilder.Entity<TitleCharacter>()
      .HasOne(tc => tc.TitleBasic)
      .WithMany()
      .HasForeignKey(tc => tc.TConst)
      .HasPrincipalKey(tb => tb.TConst);
  }


  // MapKnownForTitles method
  private static void MapKnownForTitles(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<KnownForTitle>().ToTable("nameknownfor");
    modelBuilder.Entity<KnownForTitle>().HasKey(k => new { k.NConst, k.KnownForTitles });
    modelBuilder.Entity<KnownForTitle>().Property(k => k.NConst).HasColumnName("nconst");
    modelBuilder.Entity<KnownForTitle>().Property(k => k.KnownForTitles).HasColumnName("knownfortitles");
    modelBuilder.Entity<KnownForTitle>()
  .HasOne(k => k.TitleBasic)
  .WithMany()
  .HasForeignKey(k => k.KnownForTitles)
  .HasPrincipalKey(tb => tb.TConst);

  }

  // MapTitlePrincipals method
  private static void MapTitlePrincipals(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<TitlePrincipal>().ToTable("titleprincipals");
    modelBuilder.Entity<TitlePrincipal>().HasKey(tp => new { tp.TConst, tp.NConst, tp.Ordering });

    modelBuilder.Entity<TitlePrincipal>()
        .Property(tp => tp.TConst)
        .HasColumnName("tconst");

    modelBuilder.Entity<TitlePrincipal>()
        .Property(tp => tp.NConst)
        .HasColumnName("nconst");

    modelBuilder.Entity<TitlePrincipal>()
        .Property(tp => tp.Ordering)
        .HasColumnName("ordering");

    // Define relationship with TitleBasic
    modelBuilder.Entity<TitlePrincipal>()
        .HasOne<TitleBasic>()
        .WithMany()
        .HasForeignKey(tp => tp.TConst)
        .HasPrincipalKey(tb => tb.TConst);
  }

  // MapTitleBasic method
  private static void MapTitleBasic(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<TitleBasic>().ToTable("titlebasic");
    modelBuilder.Entity<TitleBasic>().HasKey(tb => tb.TConst);
    modelBuilder.Entity<TitleBasic>().Property(tb => tb.TConst).HasColumnName("tconst");
    modelBuilder.Entity<TitleBasic>().Property(tb => tb.TitleType).HasColumnName("titletype");
    modelBuilder.Entity<TitleBasic>().Property(tb => tb.PrimaryTitle).HasColumnName("primarytitle");
    modelBuilder.Entity<TitleBasic>().Property(tb => tb.OriginalTitle).HasColumnName("originaltitle");
    modelBuilder.Entity<TitleBasic>().Property(tb => tb.StartYear).HasColumnName("startyear");
    modelBuilder.Entity<TitleBasic>().Property(tb => tb.EndYear).HasColumnName("endyear");
    modelBuilder.Entity<TitleBasic>().Property(tb => tb.RunTimeMinutes).HasColumnName("runtimeminutes");
    modelBuilder.Entity<TitleBasic>().Property(tb => tb.Awards).HasColumnName("awards");
    modelBuilder.Entity<TitleBasic>().Property(tb => tb.Plot).HasColumnName("plot");
    modelBuilder.Entity<TitleBasic>().Property(tb => tb.Rated).HasColumnName("rated");
    modelBuilder.Entity<TitleBasic>().Property(tb => tb.ReleaseDate).HasColumnName("releasedate");
    modelBuilder.Entity<TitleBasic>().Property(tb => tb.ProductionCompany).HasColumnName("productioncompany");
    modelBuilder.Entity<TitleBasic>().Property(tb => tb.Poster).HasColumnName("poster");
    modelBuilder.Entity<TitleBasic>().Property(tb => tb.BoxOffice).HasColumnName("boxoffice");

    // Relationships
    modelBuilder.Entity<TitleBasic>()
        .HasOne(tb => tb.TitleRating)
        .WithOne()
        .HasForeignKey<TitleRating>(tr => tr.TConst);

    modelBuilder.Entity<TitleBasic>()
        .HasMany(tb => tb.TitleGenres)
        .WithOne()
        .HasForeignKey(tg => tg.TConst);

    modelBuilder.Entity<TitleBasic>()
        .HasMany(tb => tb.TitleCountries)
        .WithOne()
        .HasForeignKey(tc => tc.TConst);
  }
  private static void MapCoPlayer(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<CoPlayer>().HasNoKey();
    modelBuilder.Entity<CoPlayer>().Property(c => c.NConst).HasColumnName("nconst");
    modelBuilder.Entity<CoPlayer>().Property(c => c.PrimaryName).HasColumnName("primaryname");
    modelBuilder.Entity<CoPlayer>().Property(c => c.Frequency).HasColumnName("frequency");
  }
  private static void MapRatingActor(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<RatingActor>().HasNoKey();
    modelBuilder.Entity<RatingActor>().Property(c => c.NConst).HasColumnName("nconst");
    modelBuilder.Entity<RatingActor>().Property(c => c.NRating).HasColumnName("nrating");
  }
  private static void MapRatingCoPlayers(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<RatingCoPlayer>().HasNoKey();
    modelBuilder.Entity<RatingCoPlayer>().Property(c => c.NConst).HasColumnName("nconst");
    modelBuilder.Entity<RatingCoPlayer>().Property(c => c.PrimaryName).HasColumnName("primaryname");
    modelBuilder.Entity<RatingCoPlayer>().Property(c => c.NRating).HasColumnName("nrating");
  }
  private static void MapRatingCrew(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<RatingCrew>().HasNoKey();
    modelBuilder.Entity<RatingCrew>().Property(c => c.NConst).HasColumnName("nconst");
    modelBuilder.Entity<RatingCrew>().Property(c => c.NRating).HasColumnName("nrating");
  }
  private static void MapSimilarMovies(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<SimilarMovie>().HasNoKey();
    modelBuilder.Entity<SimilarMovie>().Property(c => c.TConst).HasColumnName("tconst");
    modelBuilder.Entity<SimilarMovie>().Property(c => c.PrimaryTitle).HasColumnName("primarytitle");
    modelBuilder.Entity<SimilarMovie>().Property(c => c.NumVotes).HasColumnName("numvotes");
    modelBuilder.Entity<SimilarMovie>().Property(c => c.MatchingLanguages).HasColumnName("matching_languages");
    modelBuilder.Entity<SimilarMovie>().Property(c => c.Poster).HasColumnName("poster");
  }
  public static void MapSearchNames(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<SearchName>().HasNoKey();
    modelBuilder.Entity<SearchName>().Property(sn => sn.NConst).HasColumnName("nconst");
    modelBuilder.Entity<SearchName>().Property(sn => sn.PrimaryName).HasColumnName("primaryname");
  }
  public static void MapSearchTitles(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<SearchTitle>().HasNoKey();
    modelBuilder.Entity<SearchTitle>().Property(st => st.TConst).HasColumnName("tconst");
    modelBuilder.Entity<SearchTitle>().Property(st => st.PrimaryTitle).HasColumnName("title");
  }

  private static void MapSearchTitleNumvotes(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<SearchTitleNumvote>().HasNoKey();
    modelBuilder.Entity<SearchTitleNumvote>().Property(st => st.TConst).HasColumnName("tconst");
    modelBuilder.Entity<SearchTitleNumvote>().Property(st => st.NumVotes).HasColumnName("numvotes");
    modelBuilder.Entity<SearchTitleNumvote>().Property(st => st.PrimaryTitle).HasColumnName("primarytitle");
    modelBuilder.Entity<SearchTitleNumvote>().Property(st => st.Poster).HasColumnName("poster");
    modelBuilder.Entity<SearchTitleNumvote>().Property(st => st.StartYear).HasColumnName("startyear");
    modelBuilder.Entity<SearchTitleNumvote>().Property(st => st.Genre).HasColumnName("genre");
    modelBuilder.Entity<SearchTitleNumvote>().Property(st => st.Rating).HasColumnName("rating");
  }
  private static void MapSearchTitleRatings(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<SearchTitleRating>().HasNoKey();
    modelBuilder.Entity<SearchTitleRating>().Property(str => str.TConst).HasColumnName("tconst");
    modelBuilder.Entity<SearchTitleRating>().Property(str => str.PrimaryTitle).HasColumnName("primarytitle");
    modelBuilder.Entity<SearchTitleRating>().Property(str => str.Poster).HasColumnName("poster");
    modelBuilder.Entity<SearchTitleRating>().Property(str => str.StartYear).HasColumnName("startyear");
    modelBuilder.Entity<SearchTitleRating>().Property(str => str.Genre).HasColumnName("genre");
    modelBuilder.Entity<SearchTitleRating>().Property(str => str.Rating).HasColumnName("rating");
    modelBuilder.Entity<SearchTitleRating>().Property(str => str.NumVotes).HasColumnName("numvotes");
  }
  private static void MapSearchTitleYears(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<SearchTitleYear>().HasNoKey();
    modelBuilder.Entity<SearchTitleYear>().Property(str => str.TConst).HasColumnName("tconst");
    modelBuilder.Entity<SearchTitleYear>().Property(str => str.PrimaryTitle).HasColumnName("primarytitle");
    modelBuilder.Entity<SearchTitleYear>().Property(str => str.Poster).HasColumnName("poster");
    modelBuilder.Entity<SearchTitleYear>().Property(str => str.StartYear).HasColumnName("startyear");
    modelBuilder.Entity<SearchTitleYear>().Property(str => str.Genre).HasColumnName("genre");
    modelBuilder.Entity<SearchTitleYear>().Property(str => str.Rating).HasColumnName("rating");
    modelBuilder.Entity<SearchTitleYear>().Property(str => str.NumVotes).HasColumnName("numvotes");
  }

  private static void MapTop10Actors(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Top10Actors>().ToTable("top10actors");
    modelBuilder.Entity<Top10Actors>().HasKey(ta => new { ta.NConst, ta.PrimaryName });
    modelBuilder.Entity<Top10Actors>().Property(ta => ta.NConst).HasColumnName("nconst");
    modelBuilder.Entity<Top10Actors>().Property(ta => ta.PrimaryName).HasColumnName("primaryname");
    modelBuilder.Entity<Top10Actors>().Property(ta => ta.NumVotes).HasColumnName("total_numvotes");
  }
  private static void MapTop10Movies(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Top10Movies>().ToTable("top10movies");
    modelBuilder.Entity<Top10Movies>().HasKey(tm => new { tm.TConst, tm.PrimaryTitle });
    modelBuilder.Entity<Top10Movies>().Property(tm => tm.TConst).HasColumnName("tconst");
    modelBuilder.Entity<Top10Movies>().Property(tm => tm.PrimaryTitle).HasColumnName("primarytitle");
    modelBuilder.Entity<Top10Movies>().Property(tm => tm.TitleType).HasColumnName("titletype");
    modelBuilder.Entity<Top10Movies>().Property(tm => tm.Poster).HasColumnName("poster");
  }
  private static void MapTop10Series(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Top10Series>().ToTable("top10series");
    modelBuilder.Entity<Top10Series>().HasKey(ts => new { ts.TConst, ts.PrimaryTitle });
    modelBuilder.Entity<Top10Series>().Property(ts => ts.TConst).HasColumnName("tconst");
    modelBuilder.Entity<Top10Series>().Property(ts => ts.PrimaryTitle).HasColumnName("primarytitle");
    modelBuilder.Entity<Top10Series>().Property(ts => ts.TitleType).HasColumnName("titletype");
    modelBuilder.Entity<Top10Series>().Property(ts => ts.Poster).HasColumnName("poster");
  }
  private static void MapTitleCountries(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<TitleCountry>().ToTable("titlecountry");
    modelBuilder.Entity<TitleCountry>().HasKey(tc => new { tc.TConst, tc.Country });
    modelBuilder.Entity<TitleCountry>().Property(tc => tc.TConst).HasColumnName("tconst");
    modelBuilder.Entity<TitleCountry>().Property(tc => tc.Country).HasColumnName("country");
  }

  private static void MapTitleEpisodes(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<TitleEpisode>().ToTable("titleepisodes");
    modelBuilder.Entity<TitleEpisode>().HasKey(te => new { te.TConst, te.ParentTConst });
    modelBuilder.Entity<TitleEpisode>().Property(te => te.TConst).HasColumnName("tconst");
    modelBuilder.Entity<TitleEpisode>().Property(te => te.ParentTConst).HasColumnName("parenttconst");
    modelBuilder.Entity<TitleEpisode>().Property(te => te.SeasonNumber).HasColumnName("seasonnumber");
    modelBuilder.Entity<TitleEpisode>().Property(te => te.EpisodeNumber).HasColumnName("episodenumber");
  }

  private static void MapTitleGenres(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<TitleGenre>().ToTable("titlegenre");
    modelBuilder.Entity<TitleGenre>().HasKey(tg => new { tg.TConst, tg.Genre });
    modelBuilder.Entity<TitleGenre>().Property(tg => tg.TConst).HasColumnName("tconst");
    modelBuilder.Entity<TitleGenre>().Property(tg => tg.Genre).HasColumnName("genre");
  }

  private static void MapTitleRating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<TitleRating>().ToTable("titleratings");
    modelBuilder.Entity<TitleRating>().HasKey(tr => tr.TConst);
    modelBuilder.Entity<TitleRating>().Property(tr => tr.TConst).HasColumnName("tconst");
    modelBuilder.Entity<TitleRating>().Property(tr => tr.AverageRating).HasColumnName("averagerating");
    modelBuilder.Entity<TitleRating>().Property(tr => tr.NumVotes).HasColumnName("numvotes");
  }

  private static void MapTitleLanguages(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<TitleLanguage>().ToTable("titlelanguages");
    modelBuilder.Entity<TitleLanguage>().HasKey(tl => new { tl.TConst, tl.Language });
    modelBuilder.Entity<TitleLanguage>().Property(tl => tl.TConst).HasColumnName("tconst");
    modelBuilder.Entity<TitleLanguage>().Property(tl => tl.Language).HasColumnName("language");
  }

  private static void MapTitleAkas(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<TitleAka>().ToTable("titleakas");
    modelBuilder.Entity<TitleAka>().HasKey(ta => new { ta.TConst, ta.Ordering });
    modelBuilder.Entity<TitleAka>().Property(ta => ta.TConst).HasColumnName("tconst");
    modelBuilder.Entity<TitleAka>().Property(ta => ta.Ordering).HasColumnName("ordering");
    modelBuilder.Entity<TitleAka>().Property(ta => ta.Title).HasColumnName("title");
    modelBuilder.Entity<TitleAka>().Property(ta => ta.Region).HasColumnName("region");
    modelBuilder.Entity<TitleAka>().Property(ta => ta.Language).HasColumnName("language");
    modelBuilder.Entity<TitleAka>().Property(ta => ta.Types).HasColumnName("types");
    modelBuilder.Entity<TitleAka>().Property(ta => ta.Attributes).HasColumnName("attributes");
    modelBuilder.Entity<TitleAka>().Property(ta => ta.IsOriginalTitle).HasColumnName("isoriginaltitle");
  }
  private static void MapGetGenreData(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<GetGenreData>().HasNoKey();
    modelBuilder.Entity<GetGenreData>().Property(gd => gd.Genre).HasColumnName("genre");
  }
  private static void MapGetTitleTypeData(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<GetTitleTypeData>().HasNoKey();
    modelBuilder.Entity<GetTitleTypeData>().Property(rd => rd.TitleType).HasColumnName("titletype");
  }
  private static void MapGetYearData(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<GetYearData>().HasNoKey();
    modelBuilder.Entity<GetYearData>().Property(yd => yd.StartYear).HasColumnName("startyear");
  }
}


