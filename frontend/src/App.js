import "bootstrap/dist/css/bootstrap.min.css";
import "./index.css";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import NavBar from "./components/NavBar";
import Footer from "./components/Footer";

import Login from "./pages/Login";
import SignUp from "./pages/SignUp";
import HomePage from "./pages/HomePage";
import IndividualTitle from "./pages/IndividualTitle";
import IndividualName from "./pages/IndividualName";
import SearchResults from "./pages/SearchResults";
import UserPage from "./pages/UserPage";

const App = () => {
  return (
    <div id="app-container">
      <Router>
        <NavBar />
        <div className="main-content">
          <Routes>
            <Route path="/" element={<HomePage />} />
            <Route path="/search" element={<SearchResults />} />
            <Route path="/login" element={<Login isFullPage={true} />} />
            <Route path="/signup" element={<SignUp isFullPage={true} />} />
            <Route path="/title/:tConst" element={<IndividualTitle />} />
            <Route path="/name/:nConst" element={<IndividualName />} />
            <Route path="/User" element={<UserPage />} />
          </Routes>
        </div>
        <Footer />
      </Router>
    </div>
  );
};

export default App;
