import { Link } from "react-router";
import SignupPage from "./SignupPage";

const LoginPage = () => {
    return <>
        <div>
            {/* <label htmlFor="username">Username</label> */}
            <form action="http://localhost:5150/api/login" method="post">
                <input name="username" type="text" placeholder="Username or email"/>
                <br />
                <input name="password" type="password" placeholder="Password"/>
                <br />
                <button type="submit">Submit</button>
            </form>
            <Link to={{pathname: "/signup"}}>New? Signup</Link>
        </div>
    </> 
}

export default LoginPage;