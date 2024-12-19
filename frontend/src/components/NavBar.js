import React, { useState, useEffect, useContext } from "react";
import { Navbar, Button, Row, Col, Modal } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import "./NavBar.css";
import "../pages/IndividualTitle.css";
import Login from "../pages/Login";
import SignUp from "../pages/SignUp";
import { logoutUser } from "../services/apiService";
import SearchBar from "./SearchBar";
import AuthContext from "../context/AuthContext";
import rmdbLogo from "../images/rmdb_logo.png";

const NavBar = () => {
  const { isLoggedIn, logout } = useContext(AuthContext);
  const [showLogin, setShowLogin] = useState(false);
  const [showSignUp, setShowSignUp] = useState(false);
  const [showLogout, setShowLogout] = useState(false);
  const navigate = useNavigate();

  const handleCloseLogin = () => setShowLogin(false);
  const handleShowLogin = () => setShowLogin(true);

  const handleCloseSignUp = () => setShowSignUp(false);
  const handleShowSignUp = () => setShowSignUp(true);

  const handleCloseLogout = () => setShowLogout(false);
  const handleShowLogout = () => setShowLogout(true);

  const handleLogout = async () => {
    try {
      await logoutUser();
      // After a successful server-side logout call the context's logout to update state and clear tokens
      logout();

      // Notify other tabs
      localStorage.setItem("logout", Date.now());
      localStorage.removeItem("token");
      sessionStorage.removeItem("token");
      handleCloseLogout();
    } catch (error) {
      console.error("Error logging out:", error);
    }
  };

  // Synchronize logout across tabs
  useEffect(() => {
    const syncLogout = (event) => {
      if (event.key === "logout") {
        logout();
        handleCloseLogin();
        handleCloseSignUp();
        handleCloseLogout();
      }
    };

    window.addEventListener("storage", syncLogout);
    return () => {
      window.removeEventListener("storage", syncLogout);
    };
  }, [logout]);

  useEffect(() => {
    console.log("Is the user logged In? :", isLoggedIn);
  }, [isLoggedIn]);

  const handleUserPage = () => {
    navigate("/user");
  };

  return (
    <Navbar expand="lg" className="p-3">
      <Row className="w-100 align-items-center">
        {/* Column 1: Logo */}
        <Col xs={3} className="d-flex align-items-center">
          <Navbar.Brand href="/" className="pl-3">
            <img src={rmdbLogo} alt="RMDB Logo" style={{ height: "4rem" }} />
          </Navbar.Brand>
        </Col>

        {/* Column 2: Search bar */}
        <Col xs={6}>
          <Row className="align-items-center">
            <Col xs={12} className="d-flex justify-content-center">
              <SearchBar />
            </Col>
          </Row>
        </Col>

        {/* Column 3: Auth Buttons */}
        <Col xs={3} className="d-flex justify-content-end">
          {!isLoggedIn && (
            <>
              <button
                className="navbar-button-outline"
                onClick={handleShowLogin}
              >
                Log In
              </button>
              <button className="navbar-button" onClick={handleShowSignUp}>
                Sign Up
              </button>
            </>
          )}
          {isLoggedIn && (
            <>
              <button
                className="navbar-button-outline"
                onClick={handleUserPage}
              >
                Profile
              </button>
              <Button
                variant="outline-danger"
                className="ml-2"
                onClick={handleShowLogout}
              >
                Log Out
              </Button>
            </>
          )}
        </Col>
      </Row>

      <Modal show={showLogin} onHide={handleCloseLogin}>
        <Modal.Header closeButton>
          <Modal.Title>Log In</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Login onLoginSuccess={handleCloseLogin} />
        </Modal.Body>
      </Modal>

      <Modal show={showSignUp} onHide={handleCloseSignUp}>
        <Modal.Header closeButton>
          <Modal.Title>Sign Up</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <SignUp onSignupSuccess={handleCloseSignUp} />
        </Modal.Body>
      </Modal>

      <Modal show={showLogout} onHide={handleCloseLogout}>
        <Modal.Header closeButton>
          <Modal.Title>Are you sure you want to log out?</Modal.Title>
        </Modal.Header>
        <Modal.Body className="mt-3">
          <div className="d-flex justify-content-end">
            <Button
              variant="secondary"
              onClick={handleCloseLogout}
              className="mr-2"
            >
              Cancel
            </Button>
            <Button variant="danger" onClick={handleLogout}>
              Yes, Log Out
            </Button>
          </div>
        </Modal.Body>
      </Modal>
    </Navbar>
  );
};

export default NavBar;
