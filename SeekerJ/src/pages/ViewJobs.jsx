// import 'bootstrap/dist/css/bootstrap.css'
import { useState, useEffect } from 'react';
import './ViewJobs.css'
import { Link } from 'react-router';
import Navbar from '../components/Navbar';
import { api } from '../services/APIClient';
import { AxiosError } from 'axios';

const JobCard = ({job, type}) => {
    return <div className="col-lg-4 col-md-6 col-12 mt-4 pt-2">
        <div className="card border-0 bg-light rounded shadow">
            <div className="card-body p-4">
                <span className="badge rounded-pill bg-primary float-md-end mb-3 mb-sm-0">Full time</span>
                <h5>{job.title}</h5>
                <span className="badge rounded-pill bg-secondary float-md-end mb-3 mb-sm-0">Open</span>
                <div className="mt-3">
                    <span className="text-muted d-block"><i className="fa fa-home" aria-hidden="true"></i> <a href="#" target="_blank" className="text-muted">Bootdey.com LLC.</a></span>
                    <span className="text-muted d-block"><i className="fa fa-map-marker" aria-hidden="true"></i> USA</span>
                </div>
                
                <div className="mt-3">
                    <Link to={"/job"} state={{jobData: job.id}} className="btn btn-primary">View Details</Link>
                    {type == "hirer" ? <a href="#" className="btn btn-danger" style={{float: 'right'}}>Close Applications</a> : <a href="#" className="btn btn-primary" style={{float: 'right'}}>Apply Now</a>}
                </div>
            </div>
        </div>
    </div>
}

const ViewJobs = () => {
    // let [type, setType] = useState("hirer");
    let type = sessionStorage.getItem("type");
    let [relevantJobs, setRelevantJobs] = useState([]);

    const splitArray = (arr, size) => {
        let returnArray = [];
        for (let index = 0; index < arr.length; index += size) {
            returnArray.push(arr.slice(index, index + size));
        }
        return returnArray
    }
    
    useEffect(() => {
            async function fetchRelevantJobs() {
                try {
                    let response = await api.get("job/get");
                    setRelevantJobs(response.data);
                } catch (error) {
                    if(error == AxiosError)
                        console.log("Axios Error");
                    else
                        console.log(error)
                }
            }
            fetchRelevantJobs();
        }, []);

//     useEffect(() => {
//   console.log("Updated relevantJobs:", relevantJobs);
// }, [relevantJobs]);

    return <>
    <Navbar />
    <div className="container mt-5 pt-4">
        <div className="row align-items-end mb-4 pb-2">
            <div className="col-md-8">
                <div className="section-title text-center text-md-start">
                    <h4 className="title mb-4">{type == "seeker" ? "Find the perfect jobs" : "View your posts"}</h4>
                </div>
            </div>

            <div className="col-md-4 mt-4 mt-sm-0 d-none d-md-block">
                <div className="text-center text-md-end">
                    {type == "hirer" && <Link to={"/job/new"} className="btn btn-secondary">{type == "seeker" ? "" : "Add New Job"} <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round" className="feather feather-arrow-right fea icon-sm"><line x1="5" y1="12" x2="19" y2="12"></line><polyline points="12 5 19 12 12 19"></polyline></svg></Link>}
                </div>
            </div>
        </div>

            {relevantJobs.length > 0 ? (
                splitArray(relevantJobs, 3).map((group) => {
                    return <div className="row"> 
                    {group.map((job) => {
                        return <JobCard job={job} type={type}></JobCard>
                    })}
                    </div>
            })
            ) : <p>No jobs found!</p>}

    {/* TODO - Add a search bar */}
        
    </div>
    </>
}

export default ViewJobs;