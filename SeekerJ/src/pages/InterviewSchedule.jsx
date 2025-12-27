import "bootstrap/dist/css/bootstrap.css"
import {applicationService, interviewService} from '../services/apiServices'
import { useEffect, useState } from "react";
import { AxiosError, HttpStatusCode } from "axios";
import ConfirmModal from '../components/forms/ConfirmModal'
import {useToast} from '../contexts/ToastContext'
import { useAuth } from "../contexts/AuthContext";
import { InterviewSchedulingModal } from "../components/forms/FormModals";
import ResumeButton from "../components/ui/ResumeButton";

const Tabs = {
  Updates: "Updates",
  Scheduled: "Scheduled",
  Finished: "Finished"
};

const Tasks = {
  Accept: "Accept",
  Reschedule: "Reschedule"
}

const InterviewCard = ({interviewData, selectedTab, setSelectedTab}) => {
  const [showConfirm, setShowConfirm] = useState(false);
  const [loading, setLoading] = useState(false);
  const [task, setTask] = useState(null);
  const [showInterviewModal, setShowInterviewModal] = useState(false);
  const [interviewDateTime, setInterviewDateTime] = useState({});

  const {showToast} = useToast();
  const {user} = useAuth();

  async function execute(){
    setLoading(true);
    try {
      if(task === Tasks.Accept){
        let response = await interviewService.setInterviewScheduled(interviewData.id);
        if(response.status === HttpStatusCode.Ok){
          showToast("Interview accepted and scheduled successfully.", true);
          setSelectedTab(Tabs.Scheduled);
          setShowConfirm(false);
        }
        else{
          showToast("Failed to update the status.", false);
        }
      }
      else if(task === Tasks.Reschedule){
        let response = await interviewService.updateDateTime(interviewData.id, {"date": interviewDateTime.date, "time": interviewDateTime.time});
        if(response.status === HttpStatusCode.Ok){
          setShowInterviewModal(false);
          setShowConfirm(false);
          showToast("Request Submitted Successfully. Waiting for confirmation from the other end.", true);
          setSelectedTab(Tabs.Scheduled);
        }
        else{
          showToast("Failed to submit the reschedule request.", false);
        }
      }
    } catch (error) {
      showToast("Error occurred while processing the request.", false);
    }
    finally{
      setLoading(false);
    }
  }

  function handleConfirm(task){
    setShowConfirm(true);
    setTask(task);

  }
    return <div className="container mt-4">
  <div className="card shadow-sm p-4 mb-4">
    <div className="d-flex justify-content-between align-items-start flex-wrap">
      <div>
        <h5 className="card-title mb-1">{interviewData.jobTitle}</h5>
        {<p className="text-muted mb-1">@ {interviewData.companyName}</p>}
        {interviewData.outcome !== "Pending" && <p className="mb-0"><strong>Outcome:</strong> <span className={`badge ${interviewData.outcome === "Hired" ? "bg-success" : interviewData.outcome == "Rejected" ? "bg-danger" : "bg-secondary"}`}>{interviewData.outcome}</span></p>}
        {/* <span className="badge bg-success">Open</span> */}
      </div>
      <div className="text-md-end mt-3 mt-md-0">
        <p className="mb-1"><strong>Interview Date:</strong> {interviewData.date}</p>
        <p className="mb-1"><strong>Time:</strong> {interviewData.time}</p>
        <p className="mb-0"><strong>Mode:</strong> <span className="badge bg-success">{interviewData.mode}</span></p>
      </div>
    </div>

    <hr />

    <div className="row">
      <div className="col-md-7 pe-md-5 border-end">
        <div className="mb-3">
          {/* <p className="mb-1"><strong>Salary:</strong> ₹{interviewData.job.salary} per month</p> */}
          {/* <p className="text-muted"><em>Terms:</em> 6-month probation. Notice period of 30 days.</p> */}
        </div>

        <div className="mb-3">
          <h6>Description</h6>
          <p>
            {interviewData.jobDescription}
          </p>
        </div>

        <div className="mb-3">
          <h6>Responsibilities</h6>
          <p>
            {interviewData.jobResponsibilities}
          </p>
          {/* <ul className="mb-0">
            <li>Build and maintain reusable code libraries</li>
            <li>Collaborate with backend and product teams</li>
            <li>Ensure pixel-perfect implementation</li>
          </ul> */}
        </div>

        <div>
          <h6>Terms & Conditions</h6>
          {interviewData.jobTermsAndConditions}
          {/* <ul className="mb-0">
            <li>Bachelor's degree in CS or related field</li>
            <li>2+ years of React experience</li>
            <li>Understanding of RESTful APIs and Git</li>
          </ul> */}
        </div>
      </div>

      <div className="col-md-5 ps-md-4 mt-4 mt-md-0">
        <h6>Applicant Details</h6>
        <div className="mb-2">
          <p className="mb-1"><strong>Name:</strong> {interviewData.firstName} {interviewData.lastName}</p>
          <p className="mb-1"><strong>Email:</strong> {interviewData.email}</p>
          <p className="mb-1"><strong>Phone:</strong> {interviewData.phoneNumber}</p>
        </div>

        <div className="mb-3 mt-3">
          {user.role == "Hirer" && <>
            <ResumeButton targetClientId={interviewData.seekerId} className="btn btn-primary p-1 px-2 mx-1" style={{width: "220px" }}>View Resume</ResumeButton>
            <ResumeButton targetClientId={interviewData.seekerId} className="btn btn-primary p-1 px-2 mx-1" style={{width: "220px"}} name={`${interviewData.firstName}_${interviewData.lastName}_Resume.pdf`} useCase="download">Download Resume (PDF)</ResumeButton>
          </>}
        </div>

        {selectedTab === Tabs.Updates && <div className="d-flex flex-column">
          <div className="d-flex mt-4">
            <button className="btn btn-primary mx-1" style={{width: "150px"}}  onClick={() => handleConfirm(Tasks.Accept)}>Accept</button>
            <button className="btn btn-danger mx-1" style={{width: "150px"}} onClick={() => setShowInterviewModal(true)}>Re-Schedule</button>
          </div>
        </div>}
        
        {console.log(interviewData.outcome)}
        {user.role == "Hirer" && selectedTab === Tabs.Finished && interviewData.outcome == "Pending" && <div className="mt-4 d-flex flex-column align-items-center">
          <h5 className="card-title mb-1">Interview Outcome</h5>
          <div className="d-flex mt-4">
            <button className="btn btn-primary mx-1" style={{width: "150px"}} onClick={() => interviewService.updateSuccessStatus(interviewData.id, "Hired")}>Hired</button>
            <button className="btn btn-danger mx-1" style={{width: "150px"}} onClick={() => interviewService.updateSuccessStatus(interviewData.id, "Rejected")}>Rejected</button>
            <button className="btn btn-secondary mx-1" style={{width: "150px"}} onClick={() => interviewService.updateSuccessStatus(interviewData.id, "DidntAppear")}>Didn't Appear</button>
          </div>
        </div>}
        
        <ConfirmModal loading={loading} show={showConfirm} onConfirm={execute} onCancel={() => setShowConfirm(false)} message={task}/>
        <InterviewSchedulingModal show={showInterviewModal} setShow={setShowInterviewModal} onSubmit={() => handleConfirm(Tasks.Reschedule)} setInterviewData={setInterviewDateTime} hasMode={false}/>
        {/* <div className="mb-3">
          <h6 className="mb-1">GitHub Projects</h6>
          <ul className="mb-0">
            <li><a href="https://github.com/aryanmehta/portfolio" target="_blank">Portfolio Website</a></li>
            <li><a href="https://github.com/aryanmehta/task-manager" target="_blank">Task Manager App</a></li>
            <li><a href="https://github.com/aryanmehta/blog-platform" target="_blank">Blog Platform (MERN)</a></li>
          </ul>
        </div> */}
      </div>
    </div>
  </div>
</div>


}

// const InterviewCard = ({interviewData}) => {
//     return <h1>{interviewData.job.title}</h1>

// }

const InterviewSchedule = () => {
    const [interviews, setInterviews] = useState([]);
    const [selectedTab, setSelectedTab] = useState(Tabs.Scheduled);

    async function updateSelectedTab(tab){
      setSelectedTab(tab);
    }

    useEffect(() => {
        async function fetchInterviews(){
          try {
            let response;
            if(selectedTab === Tabs.Finished){
              response = await interviewService.getInterviews("Taken");
            }
            else{
              response = await interviewService.getInterviews(selectedTab);
            }
            setInterviews(response.data);
          } catch (error) {
              if(error == AxiosError)
                console.log("Axios Error");
              else
                console.log(error);
          }
        }

        fetchInterviews();
    }, [selectedTab]);

    return<>
    {/* {console.log(interviews)} */}
    <div className="container mt-4">
        <div className="mb-4 d-flex gap-2 flex-wrap align-items-center justify-content-center">
             <button className={`btn btn-outline-warning ${selectedTab === Tabs.Updates ? "active" : "text-dark"}`} data-status="updates" onClick={() => updateSelectedTab(Tabs.Updates)}>Need Action</button>
            <button className={`btn btn-outline-success ${selectedTab === Tabs.Scheduled ? "active" : ""}`} data-status="schedule" onClick={() => updateSelectedTab(Tabs.Scheduled)}>Scheduled</button>
            <button className={`btn btn-outline-secondary ${selectedTab === Tabs.Finished ? "active" : ""}`} data-status="finished" onClick={() => updateSelectedTab(Tabs.Finished)}>Finished</button>
            {/* <button className={`btn btn-outline-danger ${status === Statuses.Rejected && "active"}`} data-status="rejected" onClick={() => fetchApplicationsBasedOnStatus(Statuses.Rejected)}>Rejected</button> */}
        </div>
    {interviews.length > 0 ? 
      interviews.map((interview) => {
        console.log(interview);
        return <InterviewCard interviewData = {interview} selectedTab={selectedTab} setSelectedTab={setSelectedTab}></InterviewCard>
    })
      : selectedTab === Tabs.Updates ? <p>No updates to show.</p> : <p>No interviews in the schedule.</p> 
    }
    </div>
    
    </> 
}

export default InterviewSchedule;