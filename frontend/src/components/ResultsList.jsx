import React, { useState } from "react";
import { Link } from "react-router-dom";

const ResultsList = ({ title, items, type, onBookmarkClick }) => {
  const [showMore, setShowMore] = useState(false);
  const displayedItems = showMore ? items : items.slice(0, 3);

  return (
    <div>
      <h3>{title}</h3>
      {items.length === 0 && <p>No results found</p>}
      <div className="search-results-container">
        {displayedItems.map((item, index) => (
          <div key={index} className="search-item">
            <Link
              to={`/${type}/${item.id || item.nConst}`}
              className="search-item-link"
            >
              {item.poster || item.imageUrl ? (
                <img
                  src={item.poster || item.imageUrl}
                  alt={item.primaryTitle || item.primaryName}
                  className="search-item-poster"
                />
              ) : (
                <div className="placeholder-image"></div>
              )}
              <div className="search-item-title">
                {item.primaryTitle || item.primaryName}
              </div>
              {item.rating && (
                <div className="search-item-rating">
                  <span className="star">⭐</span>
                  <p className="title-rating">{item.rating}</p>
                </div>
              )}
            </Link>
            <button
              className="add-to-bookmarks-button"
              onClick={onBookmarkClick}
            >
              + Add to Bookmarks
            </button>
          </div>
        ))}
      </div>
      {items.length > 3 && (
        <p
          onClick={() => setShowMore(!showMore)}
          className="see-more-text"
        >
          {showMore ? "See less..." : "See more..."}
        </p>
      )}
    </div>
  );
};

export default ResultsList;