import React, { useState, useContext, useEffect } from "react";
import { Form, Alert } from "react-bootstrap";
import { registerUser, loginUser } from "../services/apiService";
import "./Login.css";
import { useNavigate } from "react-router-dom";
import AuthContext from "../context/AuthContext";

const SignUp = ({ onSignupSuccess, isFullPage }) => {
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    name: "",
    userName: "",
    email: "",
    password: "",
    confirmPassword: "",
    role: "user",
  });
  const [error, setError] = useState(null);
  const [stayLoggedIn, setStayLoggedIn] = useState(false);
  const [success, setSuccess] = useState(false);
  const { login, loadingAuth, isLoggedIn } = useContext(AuthContext);

    useEffect(() => {
    if (!loadingAuth && isFullPage && isLoggedIn) {
        navigate("/"); // Redirect to the home page if already logged in ( Only for full page SignUp, since users should'nt be redirected if they are viewing a title or name) 
    }
  }, [isLoggedIn, navigate]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleSignUp = async (e) => {
    e.preventDefault();
    setError(null);
    setSuccess(false);

    if (!formData.name) {
      setError("Name is required");
      return;
    }

    if (!formData.userName) {
      setError("Username is required");
      return;
    }

    if (!formData.email) {
      setError("Email is required");
      return;
    }

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(formData.email)) {
      setError("Please enter a valid email address");
      return;
    }

    if (!formData.password) {
      setError("Password is required");
      return;
    }

    if (!formData.confirmPassword) {
      setError("Please confirm password");
      return;
    }

    if (formData.password !== formData.confirmPassword) {
      setError("Passwords do not match");
      return;
    }

    try {
      await registerUser({
        name: formData.name,
        username: formData.userName,
        email: formData.email,
        password: formData.password,
        role: formData.role,
      });

      const loginResponse = await loginUser({
        userName: formData.userName,
        password: formData.password,
      });

      if (stayLoggedIn) {
        localStorage.setItem("token", loginResponse.token);
      } else {
        sessionStorage.setItem("token", loginResponse.token);
      }

      login();

      setSuccess(true);
      if (onSignupSuccess) {
        onSignupSuccess();
      }
    } catch (err) {
      setError(err.message);
    }
  };

  const form = (
    <Form className="login-form" onSubmit={handleSignUp}>
      {error && <Alert variant="danger">{error}</Alert>}
      {success && <Alert variant="success">Registration successful!</Alert>}

      <Form.Label className="login-label">Name:</Form.Label>
      <Form.Control
        type="text"
        placeholder="Enter your full name"
        className="login-input"
        name="name"
        value={formData.name}
        onChange={handleChange}
        disabled={success}
      />

      <Form.Label className="login-label">Username:</Form.Label>
      <Form.Control
        type="text"
        placeholder="Enter username"
        className="login-input"
        name="userName"
        value={formData.userName}
        onChange={handleChange}
        disabled={success}
      />

      <Form.Label className="login-label">Email:</Form.Label>
      <Form.Control
        type="email"
        placeholder="Enter email"
        className="login-input"
        name="email"
        value={formData.email}
        onChange={handleChange}
        disabled={success}
      />

      <Form.Label className="login-label">Password:</Form.Label>
      <Form.Control
        type="password"
        placeholder="Enter password"
        className="login-input"
        name="password"
        value={formData.password}
        onChange={handleChange}
        disabled={success}
      />

      <Form.Label className="login-label">Confirm Password:</Form.Label>
      <Form.Control
        type="password"
        placeholder="Confirm password"
        className="login-input"
        name="confirmPassword"
        value={formData.confirmPassword}
        onChange={handleChange}
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
      <button className="login-submit" disabled={success}>
        Sign Up
      </button>
    </Form>
  );

  return isFullPage ? (
    <div className="full-page-form-container">
      <div className="full-page-form">
         <div className="full-page-form-header">
           <h3>Sign Up</h3>
         </div>
      {form}</div>
    </div>
  ) : (
    form
  );
};

export default SignUp;
