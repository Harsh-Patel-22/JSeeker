import "bootstrap/dist/css/bootstrap.css"
import "bootstrap/dist/js/bootstrap.bundle.min.js"
import { api } from "../services/APIClient";
import { useEffect, useState } from "react";
import { Link } from "react-router";
import Navbar from "../components/Navbar";
import Footer from "../components/Footer";

const ApplicationCard = ({applicationData}) => {
    return <>
        <div className="container mt-4">
            <div className="mb-4 d-flex gap-2 flex-wrap">
            <button className="btn btn-outline-primary active" data-status="all">All</button>
            <button className="btn btn-outline-success" data-status="shortlisted">Shortlisted</button>
            <button className="btn btn-outline-danger" data-status="rejected">Rejected</button>
            </div>

            <div className="card shadow-sm p-4 mb-4 application-card" data-status="all shortlisted">
            <div className="d-flex justify-content-between align-items-start flex-wrap">
                <div>
                <h5 className="mb-1">Riya Sharma</h5>
                <p className="mb-1"><strong>Email:</strong> riya.sharma.dev@gmail.com</p>
                </div>
                <div className="text-md-end mt-3 mt-md-0">
                <p className="mb-1"><strong>Applied On:</strong> 2025-06-18</p>
                <p className="mb-0"><strong>Status:</strong> <span className="text-success">Shortlisted</span></p>
                </div>
            </div>

            <hr />

            <div className="row">
                <div className="col-md-12">
                <h6>Skills</h6>
                <span className="badge bg-secondary me-1 mb-1">React</span>
                <span className="badge bg-secondary me-1 mb-1">TypeScript</span>
                <span className="badge bg-secondary me-1 mb-1">Tailwind</span>
                <span className="badge bg-secondary me-1 mb-1">REST APIs</span>

                <div className="mt-3">
                    <p className="mb-1"><strong>Resume:</strong> <a href="https://example.com/resume-riya.pdf" target="_blank" rel="noreferrer">Download</a></p>
                </div>

                <div className="d-flex gap-2 mt-3">
                    <button className="btn btn-outline-success btn-sm">Shortlist</button>
                    <button className="btn btn-outline-danger btn-sm">Reject</button>
                    <button className="btn btn-outline-primary btn-sm" data-bs-toggle="modal" data-bs-target="#viewModal">View Details</button>
                </div>
                </div>
            </div>
            </div>
        </div>

        <div className="modal fade" id="viewModal" tabIndex="-1" aria-labelledby="viewModalLabel" aria-hidden="true">
            <div className="modal-dialog modal-lg modal-dialog-scrollable">
            <div className="modal-content">
                <div className="modal-header">
                <h5 className="modal-title" id="viewModalLabel">Application Details</h5>
                <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div className="modal-body">
                <h6>Job Title: Frontend Developer</h6>
                <p><strong>Location:</strong> Bangalore</p>
                <p><strong>Salary:</strong> ₹75,000/month · Full-time</p>

                <h6 className="mt-3">Job Description</h6>
                <p>Build scalable frontend applications with React and integrate with backend APIs. Work in a remote-first company with a dynamic team.</p>

                <h6>Responsibilities</h6>
                <ul>
                    <li>Build and maintain frontend components</li>
                    <li>Collaborate with backend developers and designers</li>
                    <li>Ensure performance and accessibility</li>
                </ul>

                <h6>Requirements</h6>
                <ul>
                    <li>2+ years experience with React</li>
                    <li>Familiarity with REST APIs and Git</li>
                    <li>Strong CSS and responsive design skills</li>
                </ul>

                <hr />

                <h6>Applicant Details</h6>
                <p><strong>Name:</strong> Riya Sharma</p>
                <p><strong>Email:</strong> riya.sharma.dev@gmail.com</p>
                <p><strong>Phone:</strong> +91 91234 56789</p>
                <p><strong>Skills:</strong> React, TypeScript, Tailwind, REST APIs</p>
                <p><strong>Resume:</strong> <a href="https://example.com/resume-riya.pdf" target="_blank" rel="noreferrer">Download</a></p>

                <h6 className="mt-3">GitHub Projects</h6>
                <ul>
                    <li><a href="https://github.com/riyasharma/ecom-store" target="_blank" rel="noreferrer">E-Commerce Store</a></li>
                    <li><a href="https://github.com/riyasharma/ui-kit" target="_blank" rel="noreferrer">UI Kit</a></li>
                </ul>
                </div>
                <div className="modal-footer">
                <button className="btn btn-outline-success">Shortlist</button>
                <button className="btn btn-outline-danger">Reject</button>
                <button type="button" className="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                </div>
            </div>
            </div>
        </div>
        </>

}

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