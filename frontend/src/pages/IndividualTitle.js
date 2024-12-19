import React, { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import "./IndividualTitle.css";
import {
  fetchImages,
  fetchTitleData,
  fetchSimilarTitles,
  fetchTitlePrincipals,
  isRated,
} from "../services/apiService";
import Bookmark from "../components/Bookmark";
import Rate from "../components/Rate";
import PaginationButtons from "../components/PaginationButtons";
import CardList from "../components/CardList";
import { useBookmarks } from "../context/BookmarkContext";

const ITEMS_PER_PAGE = 5;

const IndividualTitle = () => {
  const { tConst } = useParams();
  const [titleData, setTitleData] = useState(null);
  const [similarTitles, setSimilarTitles] = useState([]);
  const [principals, setPrincipals] = useState([]);
  const [castWithImages, setCastWithImages] = useState([]);
  const [crewPage, setCrewPage] = useState(0);
  const [similarTitlesPage, setSimilarTitlesPage] = useState(0);
  const [showBookmarkModal, setShowBookmarkModal] = useState(false);
  const [showRateModal, setShowRateModal] = useState(false);
  const [error, setError] = useState(null);
  const [loading, setLoading] = useState(true);
  const [isBookmarkedStatus, setIsBookmarkedStatus] = useState(null);
  const [isRatedStatus, setIsRatedStatus] = useState(null);

  // Fetch main title data, similar titles, and cast/crew
  useEffect(() => {
    const fetchData = async () => {
      setLoading(true);
      try {
        const [title, similar, principals] = await Promise.all([
          fetchTitleData(tConst),
          fetchSimilarTitles(tConst, 1, ITEMS_PER_PAGE * 5),
          fetchTitlePrincipals(tConst),
        ]);

        setTitleData(title);
        setSimilarTitles(similar.items || []);
        setPrincipals(principals || []);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };
    fetchData();
    setSimilarTitlesPage(0);
    setCrewPage(0);
    window.scrollTo(0, 0);
  }, [tConst]);

  // Fetch cast images
  useEffect(() => {
    const fetchCastImages = async () => {
      if (!principals.length) return;
      const updatedPrincipals = await Promise.all(
        principals.map(async (principal) => {
          const imageUrl = await fetchImages(principal.nConst);
          return { ...principal, imageUrl };
        })
      );
      setCastWithImages(updatedPrincipals);
    };
    fetchCastImages();
  }, [principals]);

  const formatTitleType = (type) => {
    const typesMap = {
      tvseries: "TV Show",
      tvepisode: "TV Episode",
      tvshort: "TV Short",
      movie: "Movie",
      videogame: "Video Game",
      short: "Short",
      tvmini: "TV Mini-Series",
      video: "Video",
    };
    return typesMap[type?.toLowerCase()] || type;
  };

  const handlePageChange = (setter, value) => {
    setter((prev) => Math.max(0, prev + value));
  };

  const { isBookmarked } = useBookmarks();

  // Check bookmark status
  useEffect(() => {
    const checkBookmarkStatus = async () => {
      if (titleData?.tConst) {
        const result = await isBookmarked(titleData.tConst);
        setIsBookmarkedStatus(result);
      }
    };
    checkBookmarkStatus();
  }, [titleData, isBookmarked]);

  // Check rating status
  useEffect(() => {
    const checkRatingStatus = async () => {
      if (titleData?.tConst) {
        const result = await isRated(titleData.tConst);
        setIsRatedStatus(result);
      }
    };
    checkRatingStatus();
  }, [titleData]);

  if (loading) return <p>Loading...</p>;
  if (error) return <p>Error: {error}</p>;

  return (
    <div className="individual-title-container">
      {/* Top Section */}
      <div className="title-header">
        <div className="title-data">
          <h1>{titleData.primaryTitle}</h1>
          <div className="meta-data">
            <span>{formatTitleType(titleData.titleType)}</span>
            {titleData.startYear && <span>{titleData.startYear}</span>}
            {titleData.genres && <span>{titleData.genres.join(", ")}</span>}
            {titleData.runTimeMinutes && (
              <span>{titleData.runTimeMinutes} min</span>
            )}
          </div>
        </div>
        <div className="title-actions">
          {titleData.averageRating && (
            <div className="rating-container">
              <span className="rating">⭐ {titleData.averageRating}</span>
              <span className="meta-data">
                {titleData.numVotes.toLocaleString()} rates
              </span>
            </div>
          )}
          {isRatedStatus ? (
            <span className="bookmark-style disabled" disabled>
              Rated
            </span>
          ) : (
            <button
              className="bookmark-style"
              onClick={() => setShowRateModal(true)}
            >
              + Add Rating
            </button>
          )}
          {isBookmarkedStatus ? (
            <span className="bookmark-style disabled" disabled>
              Bookmarked
            </span>
          ) : (
            <button
              className="bookmark-style"
              onClick={() => setShowBookmarkModal(true)}
            >
              + Add to Bookmarks
            </button>
          )}
        </div>
      </div>

      {/* Poster and Plot */}
      <div className="poster-plot-container">
        {titleData.poster && (
          <img
            src={titleData.poster}
            alt={`${titleData.primaryTitle} poster`}
            className="poster"
          />
        )}
        <p className="plot">{titleData.plot}</p>
      </div>

      {/* Cast & Crew */}
      <section className="cast-crew-similar-titles">
        <h2>Cast & Crew</h2>
        <CardList
          items={castWithImages.slice(
            crewPage * ITEMS_PER_PAGE,
            (crewPage + 1) * ITEMS_PER_PAGE
          )}
          renderItem={(principal) => (
            <Link
              to={`/name/${principal.nConst}`}
              key={principal.nConst}
              className="search-item-link"
            >
              <div className="card">
                {principal.imageUrl ? (
                  <img
                    src={principal.imageUrl}
                    alt={principal.name}
                    className="card-img"
                  />
                ) : (
                  <div className="card-img placeholder"></div>
                )}
                <div className="card-details">
                  <p>{principal.name}</p>
                  <p>{principal.roles}</p>
                </div>
              </div>
            </Link>
          )}
        />
        <PaginationButtons
          currentPage={crewPage}
          totalItems={castWithImages.length}
          itemsPerPage={ITEMS_PER_PAGE}
          onNext={() => handlePageChange(setCrewPage, 1)}
          onPrevious={() => handlePageChange(setCrewPage, -1)}
        />
      </section>

      {/* Similar Titles */}
      <section className="cast-crew-similar-titles">
        <h2>Similar Titles</h2>
        <CardList
          items={similarTitles.slice(
            similarTitlesPage * ITEMS_PER_PAGE,
            (similarTitlesPage + 1) * ITEMS_PER_PAGE
          )}
          renderItem={(title) => (
            <Link
              to={`/title/${title.tConst.split("/").pop()}`}
              key={title.tConst}
              className="search-item-link"
            >
              <div className="card">
                {title.poster ? (
                  <img
                    src={title.poster}
                    alt={title.primaryTitle}
                    className="card-img"
                  />
                ) : (
                  <div className="card-img placeholder"></div>
                )}
                <div className="card-details">
                  <p>{title.primaryTitle}</p>
                </div>
              </div>
            </Link>
          )}
        />
        <PaginationButtons
          currentPage={similarTitlesPage}
          totalItems={similarTitles.length}
          itemsPerPage={ITEMS_PER_PAGE}
          onNext={() => handlePageChange(setSimilarTitlesPage, 1)}
          onPrevious={() => handlePageChange(setSimilarTitlesPage, -1)}
        />
      </section>

      <Bookmark
        show={showBookmarkModal}
        onClose={() => setShowBookmarkModal(false)}
        onBookmarkSuccess={() => setIsBookmarkedStatus(true)}
        identifier={tConst}
      />
      <Rate
        show={showRateModal}
        onClose={() => setShowRateModal(false)}
        onRateSuccess={() => setIsRatedStatus(true)}
      />
    </div>
  );
};

export default IndividualTitle;
