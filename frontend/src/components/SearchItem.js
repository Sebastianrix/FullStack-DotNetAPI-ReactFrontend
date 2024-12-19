import React, { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import Bookmark from "../components/Bookmark";
import { useBookmarks } from "../context/BookmarkContext";

const SearchItem = ({ item, type }) => {
  const [showBookmarkModal, setShowBookmarkModal] = useState(false);
  const [isBookmarkedState, setIsBookmarkedState] = useState(false);
  const [imageError, setImageError] = useState(false);

  const handleBookmarkClick = () => {
    setShowBookmarkModal(true);
  };

  const extractConstFromUrl = (url) => {
    const urlParts = url.split("/");
    return urlParts[urlParts.length - 1];
  };

  const constValue = extractConstFromUrl(
    type === "title" ? item.tConst : item.nConst
  );

  const { isBookmarked } = useBookmarks();

  useEffect(() => {
    const checkBookmarkStatus = async () => {
      const result = await isBookmarked(constValue);
      setIsBookmarkedState(result);
    };

    checkBookmarkStatus();
  }, [constValue, isBookmarked]);

  return (
    <div className="search-item">
      <Link
        to={`/${type === "title" ? "title" : "name"}/${item[
          type === "title" ? "tConst" : "nConst"
        ]
          .split("/")
          .pop()}`}
        className="search-item-link"
      >
        {item.poster && !imageError ? (
          <img
            src={item.poster}
            alt={item.primaryTitle || item.primaryName}
            className="search-item-img"
            onError={() => setImageError(true)}
          />
        ) : (
          <div className="search-item-img placeholder"></div>
        )}
        <div className="search-item-title">
          {item.primaryTitle || item.primaryName}
        </div>
        {type === "title" ? (
          <div className="search-item-details">
            <p className="search-item-year">{item.startYear}</p>
            <p className="search-item-genre">{item.genre}</p>
          </div>
        ) : (
          <div className="search-item-details">
            <p className="search-item-year">
              {item.birthYear} - {item.deathYear}
            </p>
          </div>
        )}
        {(item.rating || item.nRating) && (
          <div className="search-item-rating">
            <span className="star-search">⭐</span>
            <p className="title-rating">{item.rating || item.nRating}</p>
          </div>
        )}
      </Link>
      {isBookmarkedState ? (
        <span className="add-to-bookmarks-button">Bookmarked</span>
      ) : (
        <button
          className="add-to-bookmarks-button"
          onClick={handleBookmarkClick}
        >
          + Add to Bookmarks
        </button>
      )}
      <Bookmark
        show={showBookmarkModal}
        onClose={() => {
          setShowBookmarkModal(false);
          setIsBookmarkedState(true);
        }}
        identifier={constValue}
      />
    </div>
  );
};

export default SearchItem;
