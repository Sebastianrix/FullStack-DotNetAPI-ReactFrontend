import React from "react";
import PropTypes from "prop-types";

const CardList = ({ items, renderItem }) => (
  <div className="cards-list">{items.map(renderItem)}</div>
);

CardList.propTypes = {
  items: PropTypes.array.isRequired,
  renderItem: PropTypes.func.isRequired,
};

export default CardList;