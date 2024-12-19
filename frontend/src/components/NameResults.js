import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import {
  setResults,
  setLoading,
  setError,
  setCurrentPage,
} from "../redux/slices/namesSearchSlice";
import { fetchNamesSearch } from "../services/apiService";
import Pagination from "./Pagination";
import SearchItem from "./SearchItem";

const NameResults = () => {
  const dispatch = useDispatch();
  const [seeMore, setSeeMore] = useState(false);
  const [itemsToShow, setItemsToShow] = useState(3);
  const { query, sortBy, results, currentPage, pageSize, totalPages } =
    useSelector((state) => state.namesSearch);

  useEffect(() => {
    const fetchData = async () => {
      setLoading(true);
      dispatch(setLoading(true));
      try {
        const data = await fetchNamesSearch(
          query,
          sortBy,
          currentPage,
          pageSize
        );
        dispatch(setResults(data));
      } catch (error) {
        dispatch(setError(error.message));
        console.error("Error fetching names:", error);
      } finally {
        setLoading(false);
        dispatch(setLoading(false));
      }
    };

    if (query) fetchData();
  }, [query, sortBy, currentPage, pageSize, dispatch]);

  const handleToggleClick = () => {
    setSeeMore(!seeMore);
    setItemsToShow(seeMore ? 3 : 10);
  };

  const handlePageChange = (page) => {
    dispatch(setCurrentPage(page));
  };

  return (
    <div>
      <h2>Names</h2>
      {results.length === 0 && <p>No results found.</p>}
      {results.slice(0, itemsToShow).map((name) => (
        <SearchItem key={name.id} item={name} type="name" />
      ))}
      {results.length > 3 && (
        <p className="see-more-text" onClick={handleToggleClick}>
          {seeMore ? "See less.." : "See more.."}
        </p>
      )}
      {seeMore && (
        <>
          <Pagination
            curPage={currentPage}
            totalPages={totalPages}
            handlePageChange={handlePageChange}
          />
        </>
      )}
    </div>
  );
};

export default NameResults;
