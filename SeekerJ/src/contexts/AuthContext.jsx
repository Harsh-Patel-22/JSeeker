import { createContext, useEffect, useContext, useState } from "react";
import { jwtDecode } from "jwt-decode";

// Utility to decode a JWT and extract user data
export function parseJwt(token) {
  if (!token) return null;
  try {
    const decoded = jwtDecode(token);
    return {
      role:
        decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] ||
        "hirer",
      clientId: decoded.nameid || "2",
    };
  } catch (error) {
    console.error("Invalid JWT:", error);
    return null;
  }
}

// Context with default values
const AuthContext = createContext({
  jwt: "",
  user: null,
  login: () => {},
  logout: () => {},
  isAuthenticated: () => false,
});

export const AuthProvider = ({ children }) => {
  const [jwt, setJwt] = useState(sessionStorage.getItem("jwt") || "");
  const [user, setUser] = useState(parseJwt(sessionStorage.getItem("jwt")));

  useEffect(() => {
    if (jwt) {
      setUser(parseJwt(jwt));
    } else {
      setUser(null);
    }
  }, [jwt]);

  const login = (newJwt) => {
    sessionStorage.setItem("jwt", newJwt);
    setJwt(newJwt);
    setUser(parseJwt(newJwt));
  };

  const logout = () => {
    sessionStorage.removeItem("jwt");
    setJwt("");
    setUser(null);
  };

  const isAuthenticated = () => {
    const storedToken = sessionStorage.getItem("jwt");
    return !!storedToken && storedToken.trim() !== "";
  };

  return (
    <AuthContext.Provider value={{ jwt, user, login, logout, isAuthenticated }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => useContext(AuthContext);
