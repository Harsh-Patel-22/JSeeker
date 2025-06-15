import { Link } from "react-router";
import LoginPage from "./LoginPage";

const SignupPage = () => {
    return <>
        <div>
            {/* <label htmlFor="username">Username</label> */}
            <form action="http:localhost:5150/api/login">
                <h1>Its signup alright?</h1>
                <input type="text" placeholder="Username or email"/>
                <br />
                <input type="password" placeholder="Password"/>
                <br />
                <button type="submit">Submit</button>
            </form>
            <Link to={{pathname: "/"}}>Already Registered? Login</Link>
        </div>
    </> 
}

export default SignupPage;