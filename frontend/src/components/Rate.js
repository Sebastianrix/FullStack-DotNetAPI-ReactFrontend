import React, { useContext, useState } from "react";
import { Modal, Button, Form } from "react-bootstrap";
import { useParams, useNavigate } from "react-router-dom";
import AuthContext from "../context/AuthContext";
import { addRating } from "../services/apiService";
import "bootstrap/dist/css/bootstrap.min.css";
import "./Bookmark.css";
import "./NavBar.css";
import StarRating from "./StarRating";

const Rate = ({ show, onClose, onRateSuccess }) => {
  const [rating, setRating] = useState("");
  const [successMessage, setSuccessMessage] = useState("");
  const { isLoggedIn } = useContext(AuthContext);
  const { tConst } = useParams();
  const navigate = useNavigate();

  const handleRate = async () => {
    try {
      const result = await addRating(tConst, rating);
      if (result.success) {
        onRateSuccess();
      }
      setSuccessMessage("Rating added successfully!");
      setTimeout(() => {
        onClose();
        setSuccessMessage("");
      }, 1000);
    } catch (err) {
      console.error("Error adding rating:", err);
      alert("Failed to add rating.");
    }
  };

  const handleButtonClick = () => {
    if (isLoggedIn) {
      handleRate();
    } else {
      navigate("/login");
    }
  };

  return (
    <Modal show={show} onHide={onClose} className="bookmark-modal">
      <Modal.Header closeButton>
        <Modal.Title className="bookmark-title">Rate Title</Modal.Title>
      </Modal.Header>
      <Modal.Body className="mt-3">
        {!isLoggedIn ? (
          <p className="bookmark-message">
            You must be logged in to rate this title.
          </p>
        ) : successMessage ? (
          <p className="bookmark-message">{successMessage}</p>
        ) : (
          <Form>
            <Form.Group controlId="rating">
              <Form.Label>Select your rating (1 to 10)</Form.Label>
              <StarRating rating={rating} setRating={setRating} />
            </Form.Group>
          </Form>
        )}
      </Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={onClose}>
          Cancel
        </Button>
        <Button onClick={handleButtonClick} className="navbar-button">
          {isLoggedIn ? "Save" : "Login"}
        </Button>
      </Modal.Footer>
    </Modal>
  );
};

export default Rate;
