import React, { createContext, useContext, useState, useEffect } from "react";
import {
  fetchBookmarks as fetchBookmarksFromApi,
  isBookmarked as isBookmarkedFromApi,
  addBookmark as addBookmarkToApi,
} from "../services/apiService";

const BookmarkContext = createContext();

export const BookmarkProvider = ({ children }) => {
  const [bookmarks, setBookmarks] = useState(new Set()); // Use a Set for fast lookups

  const fetchBookmarks = async () => {
    try {
      const token = localStorage.getItem("token") || sessionStorage.getItem("token");;
      if (!token) {
        console.log("No token found in localStorage or sessionStorage");
        return;
      }

      const userBookmarks = await fetchBookmarksFromApi();

      const identifierSet = new Set(
        userBookmarks.items.map((bookmark) =>
          bookmark.tConst ? bookmark.tConst : bookmark.nConst
        )
      );

      setBookmarks(identifierSet);
    } catch (err) {
      console.error("Error fetching bookmarks in Context:", err);
    }
  };

  const isBookmarked = async (identifier) => {
    try {
      const result = await isBookmarkedFromApi(identifier);
      return result;
    } catch (err) {
      console.error("Error checking if title is bookmarked:", err);
      return false;
    }
  };

  useEffect(() => {
    fetchBookmarks();
  }, []);

  return (
    <BookmarkContext.Provider value={{ bookmarks, isBookmarked }}>
      {children}
    </BookmarkContext.Provider>
  );
};

export const useBookmarks = () => useContext(BookmarkContext);
