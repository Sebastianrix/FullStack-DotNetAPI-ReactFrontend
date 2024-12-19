import React from "react";

const Pagination = ({ curPage, totalPages, handlePageChange }) => {
  const getPaginationButtons = () => {
    const buttons = [];
    if (totalPages <= 5) {
      for (let i = 1; i <= totalPages; i++) {
        buttons.push(i);
      }
    } else {
      if (curPage <= 3) {
        buttons.push(1, 2, 3, 4, 5);
      } else if (curPage >= totalPages - 2) {
        buttons.push(
          totalPages - 4,
          totalPages - 3,
          totalPages - 2,
          totalPages - 1,
          totalPages
        );
      } else {
        buttons.push(
          curPage - 2,
          curPage - 1,
          curPage,
          curPage + 1,
          curPage + 2
        );
      }
    }
    return buttons;
  };

  return (
    <div className="search-pagination-container">
      <div className="search-pagination-buttons">
        <button
          onClick={() => handlePageChange(curPage - 1)}
          disabled={curPage === 1}
          className="search-pagination-button"
        >
          &lt;
        </button>
        {getPaginationButtons().map((page, index) => (
          <button
            key={index}
            onClick={() => page !== "..." && handlePageChange(page)}
            className={`search-pagination-button ${
              curPage === page ? "active" : ""
            }`}
            disabled={page === "..."}
          >
            {page}
          </button>
        ))}
        <button
          onClick={() => handlePageChange(curPage + 1)}
          disabled={curPage === totalPages}
          className="search-pagination-button"
        >
          &gt;
        </button>
      </div>
      <p className="search-pagination-text">
        Page {curPage} of {totalPages}
      </p>
    </div>
  );
};

export default Pagination;
