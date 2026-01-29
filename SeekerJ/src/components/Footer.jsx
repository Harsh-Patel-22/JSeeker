import { useNavigate } from "react-router";
const Footer = () => {
    const navigate = useNavigate();
    return <footer className="py-3 bg-dark" >
        <ul className="nav justify-content-center border-bottom pb-3 mb-3"> 
            <li className="nav-item"><button className=" text-white px-2 nav-link" onClick={() => {navigate("/")}}>Home</button></li> 
            <li className="nav-item"><button className=" text-white px-2 nav-link" onClick={() => {navigate("/features")}}>Features</button></li> 
            <li className="nav-item"><button className=" text-white px-2 nav-link" onClick={() => {navigate("/about")}}>About</button></li> 
        </ul> 
        <p className="text-center text-white">© JSeeker, Inc</p> 
    </footer>
}

export default Footer;