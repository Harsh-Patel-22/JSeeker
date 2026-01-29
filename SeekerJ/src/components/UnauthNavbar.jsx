
import { useState, } from 'react';
import { Link, useNavigate } from 'react-router';
import { useAuth } from '../contexts/AuthContext';

const UnauthNavbar = () => {
    let navigate = useNavigate();
    

    return <div className="px-3 py-2 text-bg-dark border-bottom">
        <div className="container">
            <div className="d-flex flex-wrap align-items-center justify-content-center justify-content-lg-start">
                <a onClick={() => navigate("/")} className="d-flex align-items-center my-2 my-lg-0 me-lg-auto text-white text-decoration-none"> <button className='bg-dark text-white'>SEEKERJ</button><svg className="bi me-2" width="40" height="32" role="img" aria-label="Bootstrap"></svg> </a> 
                <ul className="nav col-12 col-lg-auto my-2 justify-content-center my-md-0 text-small"> 
                    <li> 
                        <button className="btn btn-outline-primary text-white" onClick={(e) => {e.currentTarget.blur(); navigate("/login")}} >Login/Signup</button>
                    </li>
                </ul>
            </div>
        </div>
    </div>
}

export default UnauthNavbar;