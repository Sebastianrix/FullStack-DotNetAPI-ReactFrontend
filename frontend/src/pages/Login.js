import React, { useState, useContext, useEffect } from "react";
import { Form, Alert } from "react-bootstrap";
import { loginUser } from "../services/apiService";
import "./Login.css";
import { useNavigate } from "react-router-dom";
import AuthContext from "../context/AuthContext";

const Login = ({ onLoginSuccess, isFullPage }) => {
  const navigate = useNavigate();
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [stayLoggedIn, setStayLoggedIn] = useState(false);
  const [error, setError] = useState(null);
  const [success, setSuccess] = useState(null);
  const { login, loadingAuth, isLoggedIn } = useContext(AuthContext);


    useEffect(() => {
        if (!loadingAuth && isFullPage && isLoggedIn) {
        navigate("/"); // Redirect to the home page if already logged in ( Only for full-page login since users should'nt be redirected if they are viewing a title or name) 
    }
  }, [isLoggedIn, navigate]);


  const handleLogin = async (e) => {
    e.preventDefault();
    setError(null);
    setSuccess(null);

    try {
      const response = await loginUser({ userName: username, password });

      if (stayLoggedIn) {
        localStorage.setItem("token", response.token);
      } else {
        sessionStorage.setItem("token", response.token);
      }

      login();

      setSuccess("Login successful!");
      if (onLoginSuccess) {
        onLoginSuccess();
      }
    } catch (err) {
      setError(err.message);
    }
  };

  const form = (
    <Form className="login-form" onSubmit={handleLogin}>
      {error && <Alert variant="danger">{error}</Alert>}
      {success && <Alert variant="success">{success}</Alert>}
      <Form.Label className="login-label">Username:</Form.Label>
      <Form.Control
        type="text"
        placeholder="Enter username"
        className="login-input"
        value={username}
        onChange={(e) => setUsername(e.target.value)}
        disabled={success}
      />
      <Form.Label className="login-label">Password: </Form.Label>
      <Form.Control
        type="password"
        placeholder="Enter password"
        className="login-input"
        value={password}
        onChange={(e) => setPassword(e.target.value)}
        disabled={success}
      />
       <Form.Group className="login-label mt-3">
        <Form.Check
          type="checkbox"
          label="Stay Logged In?"
          checked={stayLoggedIn}
          onChange={(e) => setStayLoggedIn(e.target.checked)}
          disabled={success}
        />
      </Form.Group>
      <button type="submit" className="login-submit" disabled={success}>
        Log In
      </button>
    </Form>
  );

  return isFullPage ? (
    <div className="full-page-form-container">
      <div className="full-page-form">
        <div className="full-page-form-header">
           <h3>Log In</h3>
         </div>
         {form}
      </div>
    </div>
  ) : (
    form
  );
};

export default Login;

