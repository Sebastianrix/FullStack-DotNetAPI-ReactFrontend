import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { fetchTitlesSearch } from "../services/apiService";
import { setResults, setCurrentPage } from "../redux/slices/searchSlice";
import SearchItem from "./SearchItem";
import Pagination from "./Pagination";

const TitleResults = () => {
  const dispatch = useDispatch();
  const [seeMore, setSeeMore] = useState(false);
  const [itemsToShow, setItemsToShow] = useState(3);
  const { query, filters, sortBy, currentPage, pageSize, totalPages, results } =
    useSelector((state) => state.search);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const data = await fetchTitlesSearch(
          query,
          filters,
          sortBy,
          currentPage,
          pageSize
        );
        if (data.length === 0) {
          dispatch(setResults([]));
        } else {
          dispatch(setResults(data));
        }
      } catch (error) {
        console.error("Error fetching search results:", error);
        dispatch(setResults([]));
      }
    };

    if (query) fetchData();
  }, [query, filters, sortBy, currentPage, pageSize, dispatch]);

  const handleToggleClick = () => {
    setSeeMore(!seeMore);
    setItemsToShow(seeMore ? 3 : 10);
  };

  const handlePageChange = (page) => {
    dispatch(setCurrentPage(page));
  };

  return (
    <>
      <h2>Titles</h2>
      {(!results || results.length === 0) && (
        <p>No titles results found for specified filters.</p>
      )}
      {results &&
        results
          .slice(0, itemsToShow)
          .map((result, index) => (
            <SearchItem key={index} item={result} type="title" />
          ))}
      {results && results.length > 3 && (
        <p className="see-more-text" onClick={handleToggleClick}>
          {seeMore ? "See less.." : "See more.."}
        </p>
      )}
      {seeMore && results && (
        <>
          <Pagination
            curPage={currentPage}
            totalPages={totalPages}
            handlePageChange={handlePageChange}
          />
        </>
      )}
    </>
  );
};

export default TitleResults;
