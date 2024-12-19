import React, { useState, useEffect } from "react";
import { Link } from "react-router-dom";

const Carousel = ({ title, items, itemType }) => {
  const [currentIndex, setCurrentIndex] = useState(0);

  useEffect(() => {
    const handleResize = () => {
      setItemsPerPage(getItemsPerPage());
    };

    window.addEventListener("resize", handleResize);
    return () => {
      window.removeEventListener("resize", handleResize);
    };
  }, []);

  const getItemsPerPage = () => {
    const width = window.innerWidth;
    if (width < 768) return 2;
    if (width < 1200) return 5;
    return 7;
  };

  const handleLeftClick = () => {
    setCurrentIndex((prevIndex) => {
      const newIndex = prevIndex - 1;
      return newIndex < 0 ? items.length - 1 : newIndex;
    });
  };

  const handleRightClick = () => {
    setCurrentIndex((prevIndex) => {
      const newIndex = prevIndex + 1;
      return newIndex >= items.length ? 0 : newIndex;
    });
  };

  const getVisibleItems = () => {
    const endIndex = currentIndex + itemsPerPage;
    if (endIndex <= items.length) {
      return items.slice(currentIndex, endIndex);
    } else {
      return items
        .slice(currentIndex)
        .concat(items.slice(0, endIndex - items.length));
    }
  };

  const [itemsPerPage, setItemsPerPage] = useState(getItemsPerPage());

  return (
    <div className="media-list">
      <h2>{title}</h2>
      <div className="carousel">
        <button onClick={handleLeftClick} className="carousel-button">
          &lt;
        </button>
        <div className="card-grid">
          {getVisibleItems().map((item) => (
            <Link
              to={`/${itemType}/${item.tConst || item.nConst}`}
              key={item.tConst || item.nConst}
              className="home-card"
            >
              <img
                src={item.poster || item.image}
                alt={itemType === "title" ? "Poster" : "Actor"}
                className="home-card-img"
              />
            </Link>
          ))}
        </div>
        <button onClick={handleRightClick} className="carousel-button">
          &gt;
        </button>
      </div>
    </div>
  );
};

export default Carousel;
