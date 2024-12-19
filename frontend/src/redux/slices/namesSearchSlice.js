import { createSlice } from "@reduxjs/toolkit";

const initialState = {
  query: "",
  sortBy: "popularity",
  results: [],
  currentPage: 1,
  pageSize: 10,
  totalPages: 1,
  loading: false,
  error: null,
};

const namesSearchSlice = createSlice({
  name: "namesSearch",
  initialState,
  reducers: {
    setQuery: (state, action) => {
      state.query = action.payload;
    },
    setSortBy: (state, action) => {
      state.sortBy = action.payload;
    },
    setResults: (state, action) => {
      state.results = action.payload.items;
      state.totalPages = action.payload.numberPages;
    },
    setCurrentPage: (state, action) => {
      state.currentPage = action.payload;
    },

    setLoading: (state, action) => {
      state.loading = action.payload;
    },
    setError: (state, action) => {
      state.error = action.payload;
    },
  },
});

export const {
  setQuery,
  setSortBy,
  setResults,
  setCurrentPage,
  setLoading,
  setError,
} = namesSearchSlice.actions;

export default namesSearchSlice.reducer;
