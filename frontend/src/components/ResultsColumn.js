import React, { useEffect } from "react";
import { useSelector, useDispatch } from "react-redux";
import { setQuery as setNamesQuery } from "../redux/slices/namesSearchSlice";
import TitleResults from "./TitleResults";
import NameResults from "./NameResults";

const ResultsColumn = () => {
  const dispatch = useDispatch();
  const { query } = useSelector((state) => state.search);

  useEffect(() => {
    dispatch(setNamesQuery(query));
  }, [query, dispatch]);

  return (
    <div>
      <h2>Results for "{query}"</h2>
      <TitleResults />
      <NameResults />
    </div>
  );
};

export default ResultsColumn;
