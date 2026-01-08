import "bootstrap/dist/css/bootstrap.css"
import "bootstrap/dist/js/bootstrap.bundle.min.js"
import { api } from "../services/APIClient";
import { useEffect, useState } from "react";
import { Link } from "react-router";
import { AxiosError, HttpStatusCode } from "axios";
import { useToast } from "../contexts/ToastContext";
import {InterviewSchedulingModal} from "../components/forms/FormModals" 
import ConfirmModal from "../components/forms/ConfirmModal";
import { applicationService, jobService } from "../services/apiServices";
import { fetchResumePDF } from "../services/Utils";
import ResumeButton from "../components/ui/ResumeButton";

const Statuses = {
    Pending: "Pending",
    Shortlisted: "Shortlisted",
    Rejected: "Rejected",
    InterviewScheduling: "InterviewScheduling"
};


const ApplicationCard = ({applicationData, statusFilter, setStatus, role}) => {
    const Tasks = {
        shortlist: "Shortlist",
        reject: "Reject",
        scheduleinterview: "Schedule Interview"
    }
    
    const [showConfirm, setShowConfirm] = useState(false);
    const [taskToPerform, setTaskToPerform] = useState(null);
    const [loading, setLoading] = useState(false);

    const [job, setJob] = useState(null);
    const [scheduledInterviewData, setScheduledInterviewData] = useState(null);
    const [scheduleInterviewModalToggle, setScheduleInterviewModalToggle] = useState(false);
    const Toast = useToast();

    async function fetchJobAndApplicantDetails() {
        let jobResponse = await jobService.getDescriptionById(applicationData.jobId);
        if(jobResponse.status === HttpStatusCode.Ok) {
            setJob(jobResponse.data);
            console.log(jobResponse.data)
        }
    }
    
    function confirm(task){
        setTaskToPerform(task);
        setShowConfirm(true);
    }

    useEffect(() => console.log(taskToPerform), [taskToPerform]);
    useEffect(() => console.log(scheduledInterviewData), [scheduledInterviewData]);
    
    async function execute(){
        // alert("Action confirmed!");
        setLoading(true);
        // Dummy API Call
        await new Promise(resolve => setTimeout(resolve, 100));
        
            if (taskToPerform === Tasks.scheduleinterview) {
                console.log("Scheduling");
                let data = {
                    "applicationId": applicationData.applicationId,
                    "seekerId": applicationData.seekerId,
                    "hirerId": applicationData.hirerId,
                    "jobId": applicationData.jobId,
                    "dateProposedByHirer": scheduledInterviewData?.date,
                    "timeProposedByHirer": scheduledInterviewData?.time,
                    "mode": scheduledInterviewData?.mode
                };
                console.log(data);
                let response = await applicationService.scheduleInterview(data);
                if(response.status === HttpStatusCode.Ok){
                    Toast.showToast("Interview Created Successfully. Waiting for seeker confirmation.", true);
                }
                else{
                    Toast.showToast("Error Scheduling Interview!", false);
                }
                setScheduleInterviewModalToggle(false);
            }
            else if(taskToPerform === Tasks.shortlist){
                let response = await applicationService.updateStatus({"applicationId": applicationData.applicationId, "state": Statuses.Shortlisted});
                console.log(response);
                if(response.status === HttpStatusCode.Ok){
                    Toast.showToast("Application Shortlisted", true);
                    setStatus(Statuses.Shortlisted);
                }
                else{
                    Toast.showToast("Error Shortlisting!", false);
                }
            }
            else if(taskToPerform === Tasks.reject){    
                let response = await applicationService.updateStatus({"applicationId": applicationData.applicationId, "state": Statuses.Rejected});
                console.log(response);
                if(response.status === HttpStatusCode.Ok){
                    Toast.showToast("Application Rejected", true);
                    setStatus(Statuses.Rejected);
                }
                else{
                    Toast.showToast("Error Rejecting!", false);
                }
            }
        setTaskToPerform(null);
        setLoading(false);
        setShowConfirm(false);
        
    }

    return <>
        <div className="container mt-4">
            
            <div className="card shadow-sm p-4 mb-4 application-card" data-status="all shortlisted">
            
            <div className="d-flex justify-content-between align-items-start flex-wrap">
                <div>
                    <h5 className="mb-1">{applicationData.firstName} {applicationData.lastName}</h5>
                    <p className="mb-0"><strong>Email:</strong> {applicationData.email}</p>
                    <p className="mb-1"><strong>Phone Number:</strong> {applicationData.phoneNumber}</p>
                </div>
                <div className="text-md-end mt-3 mt-md-0">
                    <h1 className="mb-1"><span className={`badge bg-${statusFilter === Statuses.Shortlisted ? "primary" : statusFilter === Statuses.Pending ? "warning text-dark" : statusFilter === Statuses.Rejected ? "danger" : "secondary"}`}>{applicationData.jobTitle}</span></h1>
                    <span className={"badge bg-warning text-dark"}>AI Given Rating: {applicationData.aiGivenRating}</span>
                    <p className="mb-1"><strong>Applied On:</strong> {applicationData.appliedOn}</p>
                {/* <p className="mb-0"><strong>Status:</strong> <span className="text-success">Shortlisted</span></p> */}
                </div>
            </div>

            <hr />

            <div className="row">
                <div className="col-md-12">
                <h6>Skills</h6>
                {applicationData.technologies.split(",").map((tech) => (
                    <span key={tech} className="badge bg-secondary me-1 mb-1">{tech}</span>
                ))}

                <div className="mt-3">
                    <p className="mb-1"><strong>Resume:</strong> <ResumeButton className={"btn btn-primary p-1 px-2"} targetClientId={applicationData.seekerId}>View</ResumeButton><ResumeButton useCase={"download"} name={`${applicationData.firstName}_${applicationData.lastName}_resume.pdf`} className={"btn btn-primary p-1 px-2 mx-2"} targetClientId={applicationData.seekerId}>Download</ResumeButton></p>
                </div>

                <div className="d-flex gap-2 mt-3">
                    {statusFilter === Statuses.Pending && <>
                        <button className="btn btn-outline-success btn-sm" onClick={() => confirm(Tasks.shortlist)}>Shortlist</button>
                        <button className="btn btn-outline-danger btn-sm" onClick={() => confirm(Tasks.reject)}>Reject</button>
                    </>}

                    {statusFilter === Statuses.Shortlisted && role === "hirer" && <>
                        <button className="btn btn-outline-success btn-sm" onClick={() => setScheduleInterviewModalToggle(true)}>Schedule Interview</button>
                    </>}
                    <button className="btn btn-outline-primary btn-sm" data-bs-toggle="modal" data-bs-target="#viewModal" onClick={(e) => {e.currentTarget.blur(); fetchJobAndApplicantDetails();}}>Job Details</button>
                </div>
                </div>
            </div>
            </div>
        </div>

        <div className="modal fade" id="viewModal" tabIndex="-1" aria-labelledby="viewModalLabel" aria-hidden="true">
            <div className="modal-dialog modal-lg modal-dialog-scrollable">
            <div className="modal-content">
                <div className="modal-header">
                    <h5 className="modal-title" id="viewModalLabel">Job Details</h5>
                    <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div className="modal-body">
                    <h6>Title: {job?.title}</h6>
                    <p><strong>Location:</strong> {job?.address?.city}</p>
                    <p><strong>Salary:</strong> ${job?.minSalary}-${job?.maxSalary} · {job?.workMode}</p>

                    <h6 className="mt-3">Description</h6>
                    <p>{job?.description}</p>

                    <h6>Responsibilities</h6>
                    {/* <ul>
                        <li>Build and maintain frontend components</li>
                        <li>Collaborate with backend developers and designers</li>
                        <li>Ensure performance and accessibility</li>
                    </ul> */}
                    <p>{job?.responsibilities}</p>

                    <h6>Terms & Conditions</h6>
                    {/* <ul>
                        <li>2+ years experience with React</li>
                        <li>Familiarity with REST APIs and Git</li>
                        <li>Strong CSS and responsive design skills</li>
                    </ul> */}
                    <p>{job?.termsAndConditions}</p>

                    <hr />

                    {/* <h6>Applicant Details</h6>
                    <p><strong>Name:</strong> Riya Sharma</p>
                    <p><strong>Email:</strong> riya.sharma.dev@gmail.com</p>
                    <p><strong>Phone:</strong> +91 91234 56789</p>
                    <p><strong>Skills:</strong> React, TypeScript, Tailwind, REST APIs</p>
                    <p><strong>Resume:</strong> <a href="https://example.com/resume-riya.pdf" target="_blank" rel="noreferrer">Download</a></p>

                    <h6 className="mt-3">GitHub Projects</h6>
                    <ul>
                        <li><a href="https://github.com/riyasharma/ecom-store" target="_blank" rel="noreferrer">E-Commerce Store</a></li>
                        <li><a href="https://github.com/riyasharma/ui-kit" target="_blank" rel="noreferrer">UI Kit</a></li>
                    </ul> */}
                </div>
                <div className="modal-footer">
                    {statusFilter === Statuses.Pending && <>
                        <button className="btn btn-outline-success" onClick={() => confirm(Tasks.shortlist)}>Shortlist</button>
                        <button className="btn btn-outline-danger" onClick={() => confirm(Tasks.reject)}>Reject</button>
                    </>}
                    {statusFilter === Statuses.Shortlisted && <>
                        <button className="btn btn-outline-success" onClick={() => setScheduleInterviewModalToggle(true)}>Schedule Interview</button>
                    </>}
                    <button type="button" className="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                </div>
            </div>
            </div>
        </div>

        {scheduleInterviewModalToggle && <InterviewSchedulingModal setInterviewData={setScheduledInterviewData} onSubmit={() => confirm(Tasks.scheduleinterview)} show={scheduleInterviewModalToggle} setShow={setScheduleInterviewModalToggle} />}

            <ConfirmModal
                show={showConfirm}
                onCancel={() => setShowConfirm(false)}
                message={taskToPerform}
                onConfirm={execute}
                loading={loading}
            />
            {/* </div> */}
        </>

}

const ApplicationsPage = () => {

    const [applications, setApplications] = useState([]);
    const [status, setStatus] = useState(Statuses.Pending);
    const role = "hirer";


    useEffect(() => {
        async function fetchApplications(){
            try {
                // setApplications(DummyApplications);
                let response = await applicationService.getByStatus(status);
                // console.log(response.data);
                setApplications(response.data);
                
            } catch (error) {
                setApplications([]);
                if(error == AxiosError)
                    console.log("Axios Error")
                else
                    console.log(error)
            }
        }

        fetchApplications();
    }, [status])

    return<>
    <div className="container mt-4">
        <div className="mb-4 d-flex gap-2 flex-wrap align-items-center justify-content-center">
            {role === "hirer" && <button className={`btn btn-outline-warning ${status === Statuses.Pending ? "active" : ""}`} data-status="all" onClick={() => setStatus(Statuses.Pending)}>Pending</button>}
            <button className={`btn btn-outline-success ${status === Statuses.Shortlisted ? "active" : ""}`} data-status="shortlisted" onClick={() => setStatus(Statuses.Shortlisted)}>Shortlisted</button>
            <button className={`btn btn-outline-danger ${status === Statuses.Rejected && "active"}`} data-status="rejected" onClick={() => setStatus(Statuses.Rejected)}>Rejected</button>
            <button className={`btn btn-outline-secondary ${status === Statuses.InterviewScheduling && "active"}`} data-status="InterviewScheduling" onClick={() => setStatus(Statuses.InterviewScheduling)}>InterviewScheduling</button>
        </div>
    {applications.length > 0 ?
    applications.map((application) => {
        return <ApplicationCard applicationData = {application} statusFilter={status} setStatus={setStatus} role={role}></ApplicationCard>
    }) :
        <p>No Applications Found</p>
    }
     
    </div>
    </> 
}

export default ApplicationsPage;