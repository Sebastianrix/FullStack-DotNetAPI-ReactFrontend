import React from "react";
import PropTypes from "prop-types";

const PaginationButtons = ({
  currentPage,
  totalItems,
  itemsPerPage,
  onNext,
  onPrevious,
}) => {
  const canNext = (currentPage + 1) * itemsPerPage < totalItems;
  const canPrevious = currentPage > 0;

  return (
    <div className="pagination-buttons">
      <button onClick={onPrevious} disabled={!canPrevious}>
        &lt;
      </button>
      <button onClick={onNext} disabled={!canNext}>
        &gt;
      </button>
    </div>
  );
};

PaginationButtons.propTypes = {
  currentPage: PropTypes.number.isRequired,
  totalItems: PropTypes.number.isRequired,
  itemsPerPage: PropTypes.number.isRequired,
  onNext: PropTypes.func.isRequired,
  onPrevious: PropTypes.func.isRequired,
};

export default PaginationButtons;