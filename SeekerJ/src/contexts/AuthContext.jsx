import { createContext, useEffect, useContext, useState } from 'react';
import {jwtDecode} from 'jwt-decode';

const AuthContext = createContext({jwt: '', user: {}, login: () => {}, logout: () => {}, isAuthenticated: () => false});

export const AuthProvider = ({ children }) => {
    let [jwt, setJwt] = useState(sessionStorage.getItem("jwt") || "");
    let [user, setUser] = useState(null);

    // setUser = (newUser) => {
    //   user = newUser;
    // }
    
    useEffect(() => {
    if (!jwt && sessionStorage.getItem("jwt")) {
      const token = sessionStorage.getItem("jwt");
      setJwt(token);
      const decoded = jwtDecode(jwt);

      setUser({
        role: decoded.role || "hirer",
        clientId: decoded.nameid || "2"
      });
    }
  }, [jwt]);

    const login = (jwt) => {
      sessionStorage.setItem("jwt", jwt);
      const decoded = jwtDecode(jwt);
      console.log("Decoded JWT:", decoded);
      setUser({
        role: decoded.role || "hirer",
        clientId: decoded.nameid || "2"
      });
      setJwt(jwt)
      console.log("User logged in:", user);
    };
    const logout = () => {
      setJwt(null)
      setUser(null);
      sessionStorage.removeItem("jwt");
    };
    const isAuthenticated = () => !!user;
  return (
    <AuthContext.Provider value={{ jwt, user, login, logout, isAuthenticated }}>
      {children}
    </AuthContext.Provider>
  );
}

export const useAuth = () => useContext(AuthContext);