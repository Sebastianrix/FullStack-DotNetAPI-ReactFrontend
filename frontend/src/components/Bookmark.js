import React, { useContext, useState } from "react";
import { Modal, Button, Form } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import { addBookmark } from "../services/apiService";
import AuthContext from "../context/AuthContext";
import "bootstrap/dist/css/bootstrap.min.css";
import "./Bookmark.css";
import "./NavBar.css";

const Bookmark = ({ show, onClose, onBookmarkSuccess, identifier }) => {
  const [note, setNote] = useState("");
  const [successMessage, setSuccessMessage] = useState("");
  const { isLoggedIn } = useContext(AuthContext);
  const navigate = useNavigate();

  const handleBookmark = async () => {
    try {
      const result = await addBookmark(identifier, note);
      if (result.success) {
        onBookmarkSuccess(true);
      }
      setSuccessMessage("Bookmark added successfully!");
      setTimeout(() => {
        onClose();
        setSuccessMessage("");
      }, 1000);
    } catch (err) {
      console.error("Error adding bookmark:", err);
      alert("Failed to add bookmark.");
    }
  };

  const handleButtonClick = () => {
    if (isLoggedIn) {
      handleBookmark();
    } else {
      navigate("/login");
    }
  };

  return (
    <Modal show={show} onHide={onClose} className="bookmark-modal">
      <Modal.Header closeButton>
        <Modal.Title className="bookmark-title">Add Bookmark</Modal.Title>
      </Modal.Header>
      <Modal.Body className="mt-3">
        {!isLoggedIn ? (
          <p className="bookmark-message">
            You must be logged in to add a bookmark.
          </p>
        ) : successMessage ? (
          <p className="bookmark-message">{successMessage}</p>
        ) : (
          <Form>
            <Form.Group controlId="note">
              <Form.Control
                as="textarea"
                rows={4}
                value={note}
                onChange={(e) => setNote(e.target.value)}
                className="bookmark-input"
                placeholder="Write something..."
              />
            </Form.Group>
          </Form>
        )}
      </Modal.Body>
      <Modal.Footer>
        {!successMessage && (
          <>
            <Button variant="secondary" onClick={onClose}>
              Cancel
            </Button>
            <button onClick={handleButtonClick} className="navbar-button">
              {isLoggedIn ? "Save" : "Log in"}
            </button>
          </>
        )}
      </Modal.Footer>
    </Modal>
  );
};

export default Bookmark;
