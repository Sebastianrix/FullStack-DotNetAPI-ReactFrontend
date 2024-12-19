import React, { useEffect } from "react";
import { useSearchParams } from "react-router-dom";
import FiltersColumn from "../components/FiltersColumn";
import ResultsColumn from "../components/ResultsColumn";
import { useDispatch } from "react-redux";
import { setQuery, setFilters, setSortBy } from "../redux/slices/searchSlice";
import "./SearchResults.css";

const SearchPage = () => {
  const [searchParams] = useSearchParams();
  const dispatch = useDispatch();

  useEffect(() => {
    const query = searchParams.get("searchTerm") || "";
    const titleType = searchParams.get("titleType") || null;
    const genre = searchParams.get("genre") || null;
    const year = parseInt(searchParams.get("year")) || -1;
    const sortBy = searchParams.get("sortBy") || "popularity";

    dispatch(setQuery(query));
    dispatch(setFilters({ titleType, genre, year }));
    dispatch(setSortBy(sortBy));
  }, [searchParams, dispatch]);

  return (
    <div className="search-page">
      <div className="filters-column">
        <FiltersColumn />
      </div>
      <div className="results-column">
        <ResultsColumn />
      </div>
    </div>
  );
};

export default SearchPage;
