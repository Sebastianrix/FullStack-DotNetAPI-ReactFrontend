import React, { useState, useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { setFilters, setSortBy } from "../redux/slices/searchSlice";
import {
  fetchGenres,
  fetchYears,
  fetchTitleTypes,
} from "../services/apiService";
import "./FiltersColumn.css";

const FiltersColumn = () => {
  const [genres, setGenres] = useState([]);
  const [years, setYears] = useState([]);
  const [titleTypes, setTitleTypes] = useState([]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [genresData, yearsData, titleTypesData] = await Promise.all([
          fetchGenres(),
          fetchYears(),
          fetchTitleTypes(),
        ]);
        setGenres(genresData);
        setYears(yearsData);
        setTitleTypes(titleTypesData);
      } catch (error) {
        console.error("Error fetching filter data:", error);
      }
    };

    fetchData();
  }, []);

  const dispatch = useDispatch();
  const { filters, sortBy } = useSelector((state) => state.search);

  const [localFilters, setLocalFilters] = useState(filters);
  const [localSortBy, setLocalSortBy] = useState(sortBy);

  const handleFilterChange = (e) => {
    const { name, value } = e.target;
    setLocalFilters((prev) => ({
      ...prev,
      [name]: value === "null" ? null : value,
    }));
  };

  const handleSortChange = (e) => {
    setLocalSortBy(e.target.value);
  };

  const handleApplyFilters = () => {
    dispatch(setFilters(localFilters));
    dispatch(setSortBy(localSortBy));
  };

  const handleClearFilters = () => {
    const clearedFilters = { titleType: null, genre: null, year: -1 };
    setLocalFilters(clearedFilters);
    dispatch(setFilters(clearedFilters));
    dispatch(setSortBy("popularity"));
  };

  return (
    <div>
      <h4>Filters</h4>
      <div>
        <label>Sort By:</label>
        <select
          className="dropdown"
          value={localSortBy}
          onChange={handleSortChange}
        >
          <option value="popularity">Popularity</option>
          <option value="rating">Rating</option>
          <option value="releaseYear">Release Year</option>
        </select>
      </div>
      <div>
        <label>Title Type:</label>
        <select
          name="titleType"
          className="dropdown"
          value={localFilters.titleType || "null"}
          onChange={handleFilterChange}
        >
          <option value="null">Select</option>
          {titleTypes.map((type) => (
            <option key={type.titleType} value={type.titleType}>
              {type.titleType}
            </option>
          ))}
        </select>
      </div>
      <div>
        <label>Genre:</label>
        <select
          name="genre"
          className="dropdown"
          value={localFilters.genre || "null"}
          onChange={handleFilterChange}
        >
          <option value="null">Select</option>
          {genres.map((genre) => (
            <option key={genre.genre} value={genre.genre}>
              {genre.genre}
            </option>
          ))}
        </select>
      </div>
      <div>
        <label>Years:</label>
        <select
          name="year"
          className="dropdown"
          value={localFilters.year || "null"}
          onChange={handleFilterChange}
        >
          <option value="null">Select</option>
          {years.map((year) => (
            <option key={year.startYear} value={year.startYear}>
              {year.startYear}
            </option>
          ))}
        </select>
      </div>
      <button onClick={handleApplyFilters} className="apply-filters">
        Apply Filters
      </button>
      <button onClick={handleClearFilters} className="clear-filters">
        Clear Filters
      </button>
    </div>
  );
};

export default FiltersColumn;
