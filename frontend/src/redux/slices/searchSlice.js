import { createSlice } from "@reduxjs/toolkit";

const initialState = {
  query: "",
  sortBy: "popularity",
  filters: {
    titleType: null,
    genre: null,
    year: -1,
  },
  currentPage: 1,
  pageSize: 10,
  results: [],
  totalPages: 1,
  loading: false,
};

const searchSlice = createSlice({
  name: "search",
  initialState,
  reducers: {
    setQuery: (state, action) => {
      state.query = action.payload;
    },
    setSortBy: (state, action) => {
      state.sortBy = action.payload;
    },
    setFilters: (state, action) => {
      state.filters = action.payload;
    },
    setCurrentPage: (state, action) => {
      state.currentPage = action.payload;
    },
    setResults: (state, action) => {
      state.results = action.payload.items;
      state.totalPages = action.payload.numberPages;
    },
    setLoading: (state, action) => {
      state.loading = action.payload;
    },
    clearFilters: (state) => {
      state.filters = { titleType: null, genre: null, year: -1 };
    },
  },
});

export const {
  setQuery,
  setSortBy,
  setFilters,
  setCurrentPage,
  setResults,
  setLoading,
  clearFilters,
} = searchSlice.actions;

export default searchSlice.reducer;
