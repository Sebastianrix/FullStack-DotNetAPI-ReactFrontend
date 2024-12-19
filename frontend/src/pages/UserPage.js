import React, { useEffect, useState, useContext } from "react";
import {
  fetchUserData,
  fetchBookmarks,
  fetchUserRatings,
  fetchUserSearchHistory,
  fetchTitleData,
  fetchNameData,
  fetchImages,
  deleteRating,
  deleteBookmark,
  deleteSearchHistory,
} from "../services/apiService";
import { Link, useNavigate } from "react-router-dom";
import AuthContext from "../context/AuthContext";
import "./UserPage.css";
import { FaStar, FaStarHalfAlt, FaRegStar } from "react-icons/fa";

const UserPage = () => {
  const navigate = useNavigate();
  const { isLoggedIn, loadingAuth, logout } = useContext(AuthContext);
  const [user, setUser] = useState(null);
  const [bookmarks, setBookmarks] = useState([]);
  const [ratings, setRatings] = useState([]);
  const [searchHistory, setSearchHistory] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [selectedSection, setSelectedSection] = useState("user-details");

  useEffect(() => {
    if (!loadingAuth && !isLoggedIn) {
      navigate("/login"); // Redirect to the login page if not logged in
    }
  }, [isLoggedIn, navigate]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [userData, userBookmarks, userRatings, userSearchHistory] =
          await Promise.all([
            fetchUserData().catch(() => ({})),
            fetchBookmarks().catch(() => ({ items: [] })),
            fetchUserRatings().catch(() => ({ items: [] })),
            fetchUserSearchHistory().catch(() => []),
          ]);

        setUser(userData || {});

        const bookmarksWithTitles = await Promise.all(
          userBookmarks.items.map(async (bookmark) => {
            if (bookmark.tConst) {
              const titleData = await fetchTitleData(bookmark.tConst).catch(
                () => ({})
              );
              return { ...bookmark, title: titleData };
            } else if (bookmark.nConst) {
              const nameData = await fetchNameData(bookmark.nConst).catch(
                () => ({})
              );
              const nameImage = await fetchImages(bookmark.nConst).catch(
                () => ({})
              );
              return { ...bookmark, name: nameData, image: nameImage };
            }
            return bookmark;
          })
        );
        setBookmarks({ items: bookmarksWithTitles });

        const ratingsWithTitles = await Promise.all(
          userRatings.items.map(async (rating) => {
            const titleData = await fetchTitleData(rating.tConst).catch(
              () => ({})
            );
            return { ...rating, title: titleData };
          })
        );
        setRatings(ratingsWithTitles || []);

        setSearchHistory(userSearchHistory || []);
      } catch (err) {
        console.error(err);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, []);

  const handleDeleteRating = async (ratingId) => {
    try {
      await deleteRating(ratingId);
      setRatings((prevRatings) => prevRatings.filter((r) => r.id !== ratingId));
    } catch (err) {
      setError(err.message);
    }
  };

  const handleDeleteBookmark = async (bookmarkId) => {
    try {
      await deleteBookmark(bookmarkId);
      setBookmarks((prevBookmarks) => ({
        items: prevBookmarks.items.filter((b) => b.id !== bookmarkId),
      }));
    } catch (err) {
      setError(err.message);
    }
  };

  const handleDeleteSearchHistory = async (searchHistoryId) => {
    try {
      await deleteSearchHistory(searchHistoryId);
      setSearchHistory((prevSearchHistory) => ({
        items: prevSearchHistory.items.filter((s) => s.id !== searchHistoryId),
      }));
    } catch (err) {
      setError(err.message);
    }
  };

  const getStarRating = (rating) => {
    const stars = [];
    const maxStars = 5;
    const halfRating = rating / 2;

    for (let i = 1; i <= maxStars; i++) {
      if (i <= halfRating) {
        stars.push(<FaStar key={i} />);
      } else if (i - 0.5 === halfRating) {
        stars.push(<FaStarHalfAlt key={i} />);
      } else {
        stars.push(<FaRegStar key={i} />);
      }
    }

    return stars;
  };

  if (loading) return <p>Loading...</p>;
  if (error) return <p>Error: {error}</p>;

  return (
    <div className="container-fluid">
      <nav className="sidebar">
        <ul className="nav flex-column">
          <li className="nav-item">
            <a
              className={`nav-link ${
                selectedSection === "user-details" ? "active" : ""
              }`}
              href="#user-details"
              onClick={() => setSelectedSection("user-details")}
            >
              User Details
            </a>
          </li>
          <li className="nav-item">
            <a
              className={`nav-link ${
                selectedSection === "bookmarks" ? "active" : ""
              }`}
              href="#bookmarks"
              onClick={() => setSelectedSection("bookmarks")}
            >
              Bookmarks
            </a>
          </li>
          <li className="nav-item">
            <a
              className={`nav-link ${
                selectedSection === "ratings" ? "active" : ""
              }`}
              href="#ratings"
              onClick={() => setSelectedSection("ratings")}
            >
              Ratings
            </a>
          </li>
          <li className="nav-item">
            <a
              className={`nav-link ${
                selectedSection === "search-history" ? "active" : ""
              }`}
              href="#search-history"
              onClick={() => setSelectedSection("search-history")}
            >
              Search History
            </a>
          </li>
        </ul>
      </nav>
      <div className="main-content">
        {selectedSection === "user-details" && (
          <div>
            <h2>Your Details</h2>
            {user && (
              <>
                <p>
                  <strong>Email:</strong> {user.email}
                </p>
                <p>
                  <strong>Username:</strong> {user.username}
                </p>
              </>
            )}
          </div>
        )}
        {selectedSection === "bookmarks" && (
          <>
            <h2>Bookmarks</h2>
            {bookmarks.items && bookmarks.items.length > 0 ? (
              <div className="user-card-grid">
                {bookmarks.items.map((bookmark) => (
                  <div className="user-card" key={bookmark.id}>
                    <div className="user-card-content">
                      {bookmark.tConst ? (
                        <Link to={`/title/${bookmark.tConst}`}>
                          <>
                            <div className="user-card-title">
                              <p>{bookmark.title.primaryTitle}</p>
                            </div>
                            <img src={bookmark.title.poster} alt="poster" />
                          </>
                        </Link>
                      ) : (
                        <Link to={`/name/${bookmark.nConst}`}>
                          <>
                            <div className="user-card-title">
                              <p>{bookmark.name.actualName}</p>
                            </div>
                            <img src={bookmark.image} alt="person" />
                          </>
                        </Link>
                      )}
                      <p className="note">
                        <strong>Note:</strong> {bookmark.note}
                      </p>
                      <p>
                        <strong>Date:</strong>{" "}
                        {new Date(bookmark.createdAt).toLocaleDateString(
                          "en-GB"
                        )}
                      </p>
                    </div>
                    <button onClick={() => handleDeleteBookmark(bookmark.id)}>
                      Delete
                    </button>
                  </div>
                ))}
              </div>
            ) : (
              <p>No bookmarks found.</p>
            )}
          </>
        )}
        {selectedSection === "ratings" && (
          <>
            <h2>Ratings</h2>
            {ratings && ratings.length > 0 ? (
              <div className="user-card-grid">
                {ratings.map((rating) => (
                  <div className="user-card" key={rating.id}>
                    <div className="user-card-content">
                      <Link to={`/title/${rating.tConst}`}>
                        <>
                          <div className="user-card-title">
                            <p>{rating.title.primaryTitle}</p>
                          </div>
                          <img src={rating.title.poster} alt="poster" />
                        </>
                      </Link>
                      <p>{getStarRating(rating.rating)}</p>
                      <p>
                        <strong>Date:</strong>{" "}
                        {new Date(rating.createdAt).toLocaleDateString("en-GB")}
                      </p>
                    </div>
                    <button onClick={() => handleDeleteRating(rating.id)}>
                      Delete
                    </button>
                  </div>
                ))}
              </div>
            ) : (
              <p>No ratings found.</p>
            )}
          </>
        )}
        {selectedSection === "search-history" && (
          <>
            <h2>Search History</h2>
            {searchHistory.items && searchHistory.items.length > 0 ? (
              <ul className="search-history">
                {searchHistory.items.map((historyItem) => (
                  <li key={historyItem.id}>
                    <p className="query">
                      <Link
                        to={`/search?searchTerm=${encodeURIComponent(
                          historyItem.searchQuery
                        )}`}
                      >
                        <strong>Query:</strong> {historyItem.searchQuery}
                      </Link>
                    </p>
                    <p className="date">
                      <strong>Date:</strong>{" "}
                      {new Date(historyItem.createdAt).toLocaleDateString(
                        "en-GB"
                      )}
                    </p>
                    <button
                      className="delete-button"
                      onClick={() => handleDeleteSearchHistory(historyItem.id)}
                    >
                      Delete
                    </button>
                  </li>
                ))}
              </ul>
            ) : (
              <p>No search history found.</p>
            )}
          </>
        )}
      </div>
    </div>
  );
};

export default UserPage;
