import { createContext, useState, useEffect } from 'react';

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
    const [isLoggedIn, setIsLoggedIn] = useState(null);
    const [loadingAuth, setLoadingAuth] = useState(true); 

    const login = () => {
        setIsLoggedIn(true);
    };
    // Remove tokens on logout
    const logout = () => {
     const hasToken =
        localStorage.removeItem("token");
        sessionStorage.removeItem("token");
    setIsLoggedIn(hasToken);
    };

    // Check tokens to set initial state on mount
    useEffect(() => {
        const hasToken =
            Boolean(localStorage.getItem("token")) ||
            Boolean(sessionStorage.getItem("token"));
        setIsLoggedIn(hasToken);
        setLoadingAuth(false); // Loading complete
    }, []);

    return (
        <AuthContext.Provider value={{ isLoggedIn, login, logout, loadingAuth }}>
            {children}
        </AuthContext.Provider>
    );
};

export default AuthContext;
