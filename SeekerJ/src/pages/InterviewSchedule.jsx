import "bootstrap/dist/css/bootstrap.css"
import { api } from "../services/APIClient";
import { useEffect, useState } from "react";
import { Link } from "react-router";
import Navbar from "../components/Navbar";
import Footer from "../components/Footer";

const InterviewCard = ({interviewData}) => {
    return <div className="row">
        <div className="col-lg-4 col-md-6 col-12 mt-4 pt-2">
        <div className="card border-0 bg-light rounded shadow">
            <div className="card-body p-4">
                <span className="badge rounded-pill bg-primary float-md-end mb-3 mb-sm-0">{interviewData.date}</span>
                <h5>{interviewData.job.title}</h5>
                <span className="badge rounded-pill bg-primary float-md-end mb-3 mb-sm-0">{interviewData.time}</span>
                <div className="mt-3">
                    <span className="text-muted d-block"><i className="fa fa-home" aria-hidden="true"></i> <a href="#" target="_blank" className="text-muted">Bootdey.com LLC.</a></span>
                    <span className="text-muted d-block"><i className="fa fa-map-marker" aria-hidden="true"></i> USA</span>
                </div>
                
                <div className="mt-3">
                    <Link to={"/job"} state={{jobData: interviewData.job}} className="btn btn-primary">View Job Details</Link>
                </div>
                <span className="badge rounded-pill bg-secondary float-md-end mb-3 mb-sm-0">{interviewData.mode}</span>
            </div>
        </div>
    </div>
    </div>
}

// const InterviewCard = ({interviewData}) => {
//     return <h1>{interviewData.job.title}</h1>

// }

const InterviewSchedule = () => {
    let [interviews, setInterviews] = useState([]);
    let clientId = parseInt(sessionStorage.getItem("clientId"))

    useEffect(() => {
        async function fetchInterviews(){
            let postObj = {"Id": clientId}
            let response = await api.post("interview/get", postObj);
            // console.log(response.data);
            setInterviews(response.data);
        }

        fetchInterviews();
    }, [])

    return<>
    <Navbar></Navbar>
    {/* {console.log(interviews)} */}
    
    {interviews.map((interview) => {
        console.log(interview);
        return <InterviewCard interviewData = {interview}></InterviewCard>
    })}
    <Footer></Footer>
    </> 
}

export default InterviewSchedule;