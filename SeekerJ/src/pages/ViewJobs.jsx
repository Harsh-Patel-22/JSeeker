// import 'bootstrap/dist/css/bootstrap.css'
import { useState, useEffect } from 'react';
import './ViewJobs.css'
import { Link } from 'react-router';
import {postedDateToText} from '../services/Utils'
import { AxiosError } from 'axios';
import { useAuth } from '../contexts/AuthContext';
import {jobService} from '../services/apiServices';
import ConfirmModal from '../components/forms/ConfirmModal'

const Statuses = {
    Open: "Open",
    ClosingSoon: "ClosingSoon",
    Closed: "Closed"
};


const JobCard = ({job, type, setRefetch}) => {
    const [showConfirm, setShowConfirm] = useState(false);
    const [loading, setLoading] = useState(false);

    async function updateJobStatusToClose(jobId) {
        setLoading(true);
        await jobService.updateJobStatus(jobId,Statuses.Closed);
        setRefetch(true);
        setShowConfirm(false);
        setLoading(false)
    }
    return <div className="col-lg-4 col-md-6 col-12 mt-4 pt-2">
        <div className="card border-0 bg-light rounded shadow">
            <div className="card-body p-4">
                <span className="badge rounded-pill bg-primary float-md-end mb-3 mb-sm-0">{job.type}</span>
                <h5>{job.title}</h5>
                <span className="badge rounded-pill bg-secondary float-md-end mb-3 mb-sm-0">{job.status}</span>
                <div className="mt-3">
                    <span className="text-muted d-block"><i className="fa fa-home" aria-hidden="true"></i> <a href="#" target="_blank" className="text-muted">{job.companyName}</a></span>
                    <span className="text-muted d-block"><i className="fa fa-map-marker" aria-hidden="true"></i> {job.address.country}</span>
                </div>
                <div className="mt-3">
                    <span className="text-muted d-block"><i className="fa fa-home" aria-hidden="true"><b>Salary:</b></i> ${job.minSalary}-${job.maxSalary}</span>
                    <span className="text-muted d-block"><b>Work Mode: </b>{job.workMode}</span>
                </div>
                
                <div className="mt-3">
                    <Link to={"/job"} state={{jobData: job.id}} className="btn btn-primary">View Details</Link>
                    {job.status != Statuses.Closed && (type == "hirer"  ? <a href="#" className="btn btn-danger" style={{float: 'right'}} onClick={() => setShowConfirm(true)}>Close Applications</a> : <a href="#" className="btn btn-primary" style={{float: 'right'}}>Apply Now</a>)}
                    <ConfirmModal loading={loading} show={showConfirm} onConfirm={() => updateJobStatusToClose(job.id)} onCancel={() => setShowConfirm(false)}  message={<>You <strong>won't be able to reopen</strong> this job post again.</>}/>
                </div>
                <span className="mt-5 badge bg-secondary float-md-end mb-3 mb-sm-0"> {postedDateToText(job.postDate)}</span>
            </div>
        </div>
    </div>
}

const ViewJobs = () => {
    let {user} = useAuth();
    let type = "seeker";
    if(user != null){
        type = user.role.toLowerCase();
    }
    let [relevantJobs, setRelevantJobs] = useState([]);
    const [statusFilter, setStatusFilter] = useState(Statuses.Open);
    const [refetch, setRefetch] = useState(false);

    const splitArray = (arr, size) => {
        let returnArray = [];
        for (let index = 0; index < arr.length; index += size) {
            returnArray.push(arr.slice(index, index + size));
        }
        return returnArray
    }
    
    useEffect(() => {
            fetchJobsBasedOnStatus(statusFilter);
            setRefetch(false);
        }, [refetch==true]);


    async function fetchJobsBasedOnStatus (status) {
        setStatusFilter(status); 
        try {
            let response = await jobService.getRelevantJobs({"type": "Internship", "status": status, "mode": "OnSite"});
            setRelevantJobs(response.data);
            console.log(response)
        } catch (error) {
            setRelevantJobs(null)
            if(error == AxiosError)
                console.log("Axios Error");
            else
                console.log(error)
        }
    }

    return <>
    <style>
        {
        `
            .btn-closing-soon {
                background-color: #ff5733;
                color: #fff;
                border: none;
                }
                
                .btn-closing-soon:hover {
                    background-color: #e00000ff;
                    color: #fff;
            }

            .btn-unselected {
                background-color: #f8f9fa;
                color: #000;
            }
        `
        }
    </style>
    <div className="container pt-4">
        <div className="row align-items-end pb-2">
            {/* <div className="col-md-8"> */}
                <div className="section-title text-center">
                    <h4 className="title">{type == "seeker" ? "Find the perfect jobs" : "View your posts"}</h4>
                </div>
            {/* </div> */}
                <div className="text-center text-md-end mb-4">
                    {type == "hirer" && <Link to={"/job/new"} className="btn btn-secondary">{type == "seeker" ? "" : "Add New Job"} <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round" className="feather feather-arrow-right fea icon-sm"><line x1="5" y1="12" x2="19" y2="12"></line><polyline points="12 5 19 12 12 19"></polyline></svg></Link>}
                </div>

                <div className="btn-group rounded-pill shadow overflow-hidden" role="group" style={{background: "#f8f9fa;"}}>
                    <button type="button" className={`btn ${statusFilter === Statuses.Open ? "btn-primary": ""} `} onClick={() => {fetchJobsBasedOnStatus(Statuses.Open)}}>Open</button>
                    <button type="button" className={`btn ${statusFilter === Statuses.ClosingSoon ? "btn-closing-soon": ""} `} onClick={() => {fetchJobsBasedOnStatus(Statuses.ClosingSoon)}}>Closing Soon</button>
                    <button type="button" className={`btn ${statusFilter === Statuses.Closed ? "btn-danger": ""} `} onClick={() => {fetchJobsBasedOnStatus(Statuses.Closed)}}>Closed</button>
                </div>
            </div>

        

            {relevantJobs ? (
                splitArray(relevantJobs, 3).map((group) => {
                    return <div className="row"> 
                    {group.map((job) => {
                        return <JobCard job={job} type={type} setRefetch={setRefetch}></JobCard>
                    })}
                    </div>
            })
            ) : <p>No jobs found!</p>}

    {/* TODO - Add a search bar */}
        
    </div>
    </>
}

export default ViewJobs;