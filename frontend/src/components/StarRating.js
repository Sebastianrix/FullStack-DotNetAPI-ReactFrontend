import React, { useState } from "react";
import "./StarRating.css";

const StarRating = ({ rating, setRating }) => {
  const [hover, setHover] = useState(0);

  return (
    <div className="star-rating">
      {[...Array(10)].map((_, index) => {
        const starValue = index + 1;

        return (
          <span
            key={starValue}
            className={
              starValue <= (hover || rating) ? "star filled" : "star"
            }
            onClick={() => setRating(starValue)}
            onMouseEnter={() => setHover(starValue)}
            onMouseLeave={() => setHover(rating)}
          >
            ★
          </span>
        );
      })}
    </div>
  );
};

export default StarRating;
