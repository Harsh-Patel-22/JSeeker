import 'bootstrap/dist/css/bootstrap.css'; 
import { useState } from 'react';
import { Link } from 'react-router';

const Navbar = () => {
    // let [type, setType] = useState("hirer");
    let type = sessionStorage.getItem("type");

    return <div className="px-3 py-2 text-bg-dark border-bottom">
        <div className="container">
            <div className="d-flex flex-wrap align-items-center justify-content-center justify-content-lg-start">
                <a href="/" className="d-flex align-items-center my-2 my-lg-0 me-lg-auto text-white text-decoration-none"> SEEKERJ<svg className="bi me-2" width="40" height="32" role="img" aria-label="Bootstrap"></svg> </a> 
                <ul className="nav col-12 col-lg-auto my-2 justify-content-center my-md-0 text-small"> 
                    <li> 
                        {type == "hirer" ? <Link to={"/dashboard/hirer"} className="nav-link text-white">Home</Link>: <Link to={"/dashboard/seeker"} className="nav-link text-white">Home</Link>} 
                    </li>
                    <li> 
                        {/* <Link to={"/jobs"}>{type == "hirer" ? "View your jobs" : "Look for jobs"}</Link> */}
                        {type == "hirer" ? <Link to={"/jobs"} className="nav-link text-white">View your jobs</Link>: <Link to={"/jobs"} className="nav-link text-white">Look for jobs</Link>} 
                    </li>
                    <li> 
                        {type == "hirer" ? <Link to={"/applications"} className="nav-link text-white">Applications</Link>: <Link to={""} className="nav-link text-white">Resume Builder</Link>} 
                    </li>
                    <li>
                        <Link to={"/interviews"} className="nav-link text-white">Interview Schedule</Link>
                    </li> 
                    <li> 
                        <Link to={""} className="nav-link text-white">Profile {/* Add account icon */}</Link>
                    </li>
                </ul>
            </div>
        </div>
    </div>
}

export default Navbar;