
import { useState, } from 'react';
import { Link, useNavigate } from 'react-router';
import { useAuth } from '../contexts/AuthContext';

const Navbar = () => {
    // let [type, setType] = useState("hirer");
    let navigate = useNavigate();
    let {user, logout} = useAuth();
    let type = "hirer";
    console.log("User in Navbar:", user);
    if(user != null) {
        type = user.role.toLowerCase();
        console.log("User role in Navbar:", type);
    }

    function handleLogout() {
        logout();
        navigate('/');
    }

    // let type = sessionStorage.getItem("type");

    return <div className="px-3 py-2 text-bg-dark border-bottom">
        <div className="container">
            <div className="d-flex flex-wrap align-items-center justify-content-center justify-content-lg-start">
                <a href="/" className="d-flex align-items-center my-2 my-lg-0 me-lg-auto text-white text-decoration-none"> SEEKERJ<svg className="bi me-2" width="40" height="32" role="img" aria-label="Bootstrap"></svg> </a> 
                <ul className="nav col-12 col-lg-auto my-2 justify-content-center my-md-0 text-small"> 
                    <li> 
                        <Link to={"/dashboard"} className="nav-link text-white">Home</Link> 
                    </li>
                    <li> 
                        <Link to={"/jobs"} className="nav-link text-white">{type == "hirer" ? "View your jobs" : "Look for jobs"}</Link>
                        {/* {type == "hirer" ? <Link to={"/jobs"} className="nav-link text-white">View your jobs</Link>: <Link to={"/jobs"} className="nav-link text-white">Look for jobs</Link>}  */}
                    </li>
                    <li> 
                        {type == "hirer" ? <Link to={"/applications"} className="nav-link text-white">Applications</Link>: <Link to={"/resume"} className="nav-link text-white">Resume Builder</Link>} 
                    </li>
                    <li>
                        <Link to={"/interviews"} className="nav-link text-white">Interview Schedule</Link>
                    </li> 
                    <li> 
                        <Link to={"/profile"} className="nav-link text-white">Profile {/* Add account icon */}</Link>
                    </li>
                    <li> 
                        <button className="btn btn-danger text-white" onClick={handleLogout}>Logout</button>
                    </li>
                </ul>
            </div>
        </div>
    </div>
}

export default Navbar;