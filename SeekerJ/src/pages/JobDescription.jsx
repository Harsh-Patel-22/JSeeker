import { useLocation } from "react-router";
import Navbar from "../components/Navbar";

const JobDescription = () => {
    var location = useLocation();
    var {jobData} = location.state;
    // console.log(job)
    return <>
        <Navbar></Navbar>
        <center>
            <h1>
                {jobData.title}
            </h1>
            <p>
                Description: {jobData.description}
            </p>
            <p>
                Terms and conditions: 
                {jobData.termsAndConditions}
            </p>
            <span>
                Salary: {jobData.salary}
            </span>
        </center>
    </>

}

export default JobDescription;