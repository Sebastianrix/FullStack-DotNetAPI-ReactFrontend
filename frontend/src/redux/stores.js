import { configureStore } from "@reduxjs/toolkit";
import searchReducer from "./slices/searchSlice";
import namesSearchSlice from "./slices/namesSearchSlice";

const store = configureStore({
  reducer: {
    search: searchReducer,
    namesSearch: namesSearchSlice,
  },
});

export default store;
