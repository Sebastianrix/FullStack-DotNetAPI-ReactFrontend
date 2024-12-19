import React, { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import "./IndividualTitle.css";
import {
  fetchImages,
  fetchNameData,
  fetchKnownForTitles,
  fetchCoPlayers,
  fetchPrincipalsByName,
  fetchBiography,
} from "../services/apiService";
import Bookmark from "../components/Bookmark";
import PaginationButtons from "../components/PaginationButtons";
import CardList from "../components/CardList";
import { useBookmarks } from "../context/BookmarkContext";

const ITEMS_PER_PAGE = 5;

const IndividualName = () => {
  const { nConst } = useParams();
  const [nameData, setNameData] = useState(null);
  const [knownFor, setKnownFor] = useState([]);
  const [coPlayers, setCoPlayers] = useState([]);
  const [principals, setPrincipals] = useState([]);
  const [personImage, setPersonImage] = useState(null);
  const [coPlayersWithImages, setCoPlayersWithImages] = useState([]);
  const [showBookmarkModal, setShowBookmarkModal] = useState(false);
  const [pagination, setPagination] = useState({
    knownForPage: 0,
    coPlayersPage: 0,
    principalsPage: 0,
  });
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [isBookmarkedStatus, setIsBookmarkedStatus] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      setLoading(true);
      try {
        const [name, known, coPlayersData, principalsData] = await Promise.all([
          fetchNameData(nConst),
          fetchKnownForTitles(nConst, 1, ITEMS_PER_PAGE * 5),
          fetchCoPlayers(nConst, 1, ITEMS_PER_PAGE * 5),
          fetchPrincipalsByName(nConst, 1, ITEMS_PER_PAGE * 5),
        ]);

        const imageUrl = await fetchImages(name.nConst);
        const bio = await fetchBiography(name.actualName);

        setNameData({ ...name, bio });
        setPersonImage(imageUrl);
        setKnownFor(known.items || []);
        setCoPlayers(coPlayersData || []);
        setPrincipals(principalsData.items || []);
      } catch (err) {
        console.error(err);
        setError("Failed to load data. Please try again later.");
      } finally {
        setLoading(false);
      }
    };

    fetchData();
    setPagination({ knownForPage: 0, coPlayersPage: 0, principalsPage: 0 });
    window.scrollTo(0, 0);
  }, [nConst]);

  // Fetch images for co-stars
  useEffect(() => {
    const fetchCostarImages = async () => {
      if (!coPlayers.items || !coPlayers.items.length) return;

      try {
        const updatedCoPlayers = await Promise.all(
          coPlayers.items.map(async (coPlayer) => {
            const nConst = coPlayer.nConst.split("/").pop();
            const imageUrl = await fetchImages(nConst);
            return { ...coPlayer, imageUrl };
          })
        );
        setCoPlayersWithImages(updatedCoPlayers);
      } catch (err) {
        console.error("Error fetching coPlayer images:", err);
      }
    };

    fetchCostarImages();
  }, [coPlayers]);

  const handlePageChange = (field, value) => {
    setPagination((prev) => ({
      ...prev,
      [field]: Math.max(0, prev[field] + value),
    }));
  };

  const { isBookmarked } = useBookmarks();

  useEffect(() => {
    const checkBookmarkedStatus = async () => {
      if (!nConst) return;
      const status = await isBookmarked(nConst);
      setIsBookmarkedStatus(status);
    };

    checkBookmarkedStatus();
  }, [nConst, isBookmarked]);

  return (
    <div className="individual-title-container">
      {loading ? (
        <p>Loading...</p>
      ) : error ? (
        <p>{error}</p>
      ) : (
        <>
          {/* Header Section */}
          <div className="title-header">
            <div className="title-data">
              <h1>{nameData.actualName}</h1>
              <div className="meta-data">
                {nameData.birthYear && <span>Born: {nameData.birthYear}</span>}
                {nameData.deathYear && <span>Died: {nameData.deathYear}</span>}
              </div>
            </div>
            <div className="title-actions">
              {nameData.nRating && (
                <span className="rating">⭐ {nameData.nRating}</span>
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
          <div className="poster-plot-container">
            {/* Person Image */}
            {personImage && (
              <img
                src={personImage}
                alt={nameData.actualName}
                className="poster"
              />
            )}
            {/* Biography */}
            <p className="plot">{nameData.bio || "Biography not available."}</p>
          </div>

          {/* Known For Section */}
          {knownFor && knownFor.length > 0 && (
            <section className="cast-crew-similar-titles">
              <h2>Known For</h2>
              <CardList
                items={knownFor.slice(
                  pagination.knownForPage * ITEMS_PER_PAGE,
                  (pagination.knownForPage + 1) * ITEMS_PER_PAGE
                )}
                renderItem={(title) => (
                  <Link
                    to={`/title/${title.knownForTitles.split("/").pop()}`}
                    key={title.knownForTitles}
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
            </section>
          )}
          {/* Co-stars Section */}
          <section className="cast-crew-similar-titles">
            <h2>Co-stars</h2>
            <CardList
              items={coPlayersWithImages.slice(
                pagination.coPlayersPage * ITEMS_PER_PAGE,
                (pagination.coPlayersPage + 1) * ITEMS_PER_PAGE
              )}
              renderItem={(coPlayer) => (
                <Link
                  to={`/name/${coPlayer.nConst.split("/").pop()}`}
                  key={coPlayer.primaryName}
                  className="search-item-link"
                >
                  <div className="card">
                    {coPlayer.imageUrl ? (
                      <img
                        src={coPlayer.imageUrl}
                        alt={coPlayer.primaryName}
                        className="card-img"
                      />
                    ) : (
                      <div className="card-img placeholder"></div>
                    )}
                    <div className="card-details">
                      <p>{coPlayer.primaryName}</p>
                      <p>{coPlayer.frequency} times</p>
                    </div>
                  </div>
                </Link>
              )}
            />
            <PaginationButtons
              currentPage={pagination.coPlayersPage}
              totalItems={coPlayersWithImages.length}
              itemsPerPage={ITEMS_PER_PAGE}
              onNext={() => handlePageChange("coPlayersPage", 1)}
              onPrevious={() => handlePageChange("coPlayersPage", -1)}
            />
          </section>

          {/* Roles Section */}
          <section className="cast-crew-similar-titles">
            <h2>Recently Seen In</h2>
            <CardList
              items={principals.slice(
                pagination.principalsPage * ITEMS_PER_PAGE,
                (pagination.principalsPage + 1) * ITEMS_PER_PAGE
              )}
              renderItem={(principal) => (
                <Link
                  to={`/title/${principal.tConst.split("/").pop()}`}
                  key={principal.tConst}
                  className="search-item-link"
                >
                  <div className="card">
                    {principal.poster ? (
                      <img
                        src={principal.poster}
                        alt={principal.title}
                        className="card-img"
                      />
                    ) : (
                      <div className="card-img placeholder"></div>
                    )}
                    <div className="card-details">
                      <p>{principal.title}</p>
                      <p>{principal.roles}</p>
                    </div>
                  </div>
                </Link>
              )}
            />
            <PaginationButtons
              currentPage={pagination.principalsPage}
              totalItems={principals.length}
              itemsPerPage={ITEMS_PER_PAGE}
              onNext={() => handlePageChange("principalsPage", 1)}
              onPrevious={() => handlePageChange("principalsPage", -1)}
            />
          </section>

          {/* Bookmark Modal */}
          <Bookmark
            show={showBookmarkModal}
            onClose={() => setShowBookmarkModal(false)}
            onBookmarkSuccess={() => setIsBookmarkedStatus(true)}
            identifier={nConst}
          />
        </>
      )}
    </div>
  );
};

export default IndividualName;
