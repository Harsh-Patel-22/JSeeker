import "bootstrap/dist/css/bootstrap.css"
import { api } from "../services/APIClient";
import { useEffect, useState } from "react";
import { Link } from "react-router";
import Navbar from "../components/Navbar";
import Footer from "../components/Footer";

const ApplicationCard = ({applicationData}) => {
    return <div className="row">
        <div className="col-lg-4 col-md-6 col-12 mt-4 pt-2">
        <div className="card border-0 bg-light rounded shadow">
            <div className="card-body p-4">
                <span className="badge rounded-pill bg-primary float-md-end mb-3 mb-sm-0">{applicationData.applicantId}</span>
                <h5>{applicationData.hirerId}</h5>
                <span className="badge rounded-pill bg-primary float-md-end mb-3 mb-sm-0">{applicationData.jobId}</span>
                <div className="mt-3">
                    <span className="text-muted d-block"><i className="fa fa-home" aria-hidden="true"></i> <a href="#" target="_blank" className="text-muted">Bootdey.com LLC.</a></span>
                    <span className="text-muted d-block"><i className="fa fa-map-marker" aria-hidden="true"></i> USA</span>
                </div>
                
                {/* <div className="mt-3">
                    <Link to={"/job"} state={{jobData: applicationData.job}} className="btn btn-primary">View Job Details</Link>
                </div> */}
                {/* <span className="badge rounded-pill bg-secondary float-md-end mb-3 mb-sm-0">{applicationData.mode}</span> */}
            </div>
        </div>
    </div>
    </div>
}

// const InterviewCard = ({applicationData}) => {
//     return <h1>{applicationData.job.title}</h1>

// }

const ApplicationsPage = () => {
    let [applications, setApplications] = useState([]);
    let clientId = parseInt(sessionStorage.getItem("clientId"))

    useEffect(() => {
        async function fetchInterviews(){
            let postObj = {"Id": clientId}
            let response = await api.post("application/get", postObj);
            // console.log(response.data);
            setApplications(response.data);
        }

        fetchInterviews();
    }, [])

    return<>
    <Navbar></Navbar>
    
    {applications.map((application) => {
        console.log(application);
        return <ApplicationCard applicationData = {application}></ApplicationCard>
    })}
    <Footer></Footer>
    </> 
}

export default ApplicationsPage;