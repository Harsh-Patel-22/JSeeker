import { useLocation, useNavigate } from "react-router";
import { useEffect, useState } from "react";
import 'bootstrap-icons/font/bootstrap-icons.css';
import { Axios, AxiosError, HttpStatusCode } from "axios";
import { useAuth } from "../contexts/AuthContext";
// import { Apply } from "../components/Apply";
import { applyToJob } from "../services/Utils";
import ConfirmModal from "../components/forms/ConfirmModal";
import { useToast } from "../contexts/ToastContext";
import { jobService } from "../services/apiServices";
import { postedDateToText } from "../services/Utils";

const JobDescription = () => {
    let location = useLocation();
    let {jobData, applied} = location.state;
    let [job, setJob] = useState({});
    let [showConfirm, setShowConfirm] = useState(false);
    let [loading, setLoading] = useState(false);

    let {user} = useAuth();
    let type = user.role.toLowerCase();
    let navigate = useNavigate();
    let {showToast} = useToast();
    // console.log(job)

    async function apply(applicationData) {
        setLoading(true);
        try{

          let response = await applyToJob(applicationData);
          if(response.status == HttpStatusCode.Ok){
            showToast("Application Created Successfully!", true);
            navigate('/jobs');
          }
          else{
            showToast("Error in Creating Application!", false);
          }
        }
        catch(error){
          if(error.status == HttpStatusCode.InternalServerError){
            console.log("Internal Server Error");
            showToast("Application Already Exist!", false);
          }
          else if(error.status == HttpStatusCode.BadRequest){
            showToast("Bad Request!", false);
          }
      }
        setShowConfirm(false);
        setLoading(false)
    }

    useEffect(() => {
      async function getAllJobDetails() {
        try {
          let response = await jobService.getDescriptionById(jobData);
          setJob(response.data);
        } catch (error) {
          if(error == AxiosError)
            console.log("Axios Error!");
          else
            console.log(error);
        }
      }
      
      getAllJobDetails();
    }, [])

    return <>
<div className="container mt-4 mb-5">
  <div className="mb-3">
    <button
      type="button"
      className="btn btn-link text-decoration-none p-0"
      onClick={() => navigate('/jobs')}
    >
      <i className="bi bi-arrow-left me-1"></i>
      Back
    </button>
  </div>
  <div className="card p-4 shadow-sm">
    <div className="d-flex justify-content-between flex-wrap">
      <div className="d-flex align-items-start gap-3">
        <div>
          <h4 className="mb-1">{job.title}</h4>
          <p className="mb-0 text-muted">{job.companyName} · {job.type}</p>
          {/* {console.log(job.address)} */}
          {/* {console.log(job.address.city)} */}
          <p className="mb-0 text-muted">{job?.address?.city}, {job?.address?.state}, {job?.address?.country} · {job.workMode}</p>
          {/* <p className="mb-0 text-muted">{job.address}, {job.address}, {job.address} · {job.workMode}</p> */}
          <small className="text-muted">{postedDateToText(job.postDate)} · {job.numberOfApplicants} applicants · Limit {job.applicationsLimit}</small>
        </div>
      </div>
      <div className="mt-3 mt-md-0">
        {type == "seeker" && !applied && <button className="btn btn-primary" onClick={() => setShowConfirm(true)}>Apply</button>}
        <ConfirmModal loading={loading} show={showConfirm} onConfirm={() => apply({"seekerId": user.clientId, "jobId": job.id, "hirerId": job.hirerId, "jobType": job.type})} onCancel={() => setShowConfirm(false)}  message={<>Confirm Application Creation</>}/>
      </div>
      {type == "hirer" &&
      <div className="mt-3 mt-md-0">
        
          <svg xmlns="http://www.w3.org/2000/svg"
              width="16"
              height="16"
              fill="currentColor"
              className="bi bi-pencil"
              viewBox="0 0 16 16"
              onClick={() => {navigate('/job/edit', {state: {jobId: job.id}})}}
          >
              <path d="M12.146.854a.5.5 0 0 1 .708 0l2.292 2.292a.5.5 0 0 1 0 .708l-9.793 9.793-3.182.795a.25.25 0 0 1-.303-.303l.795-3.182 9.793-9.793z"/>
              <path d="M11.207 2L3 10.207V13h2.793L14 4.793 11.207 2z"/>
          </svg>
      </div>
      }
    </div>

    <hr className="my-4" />

    <div className="job-description">
      <h5>About the job</h5>
      <p>
        {job.description}
      </p>

      <h6>Responsibilities</h6>
      {job.responsibilities}
      {/* <ul>
        <li>Develop responsive, reusable UI components in React</li>
        <li>Collaborate with product and design teams</li>
        <li>Ensure performance, quality, and responsiveness</li>
      </ul> */}

      <h6>Work Experience Requirement</h6>
      {job.requiredWorkExperience > 0 ? `${job.requiredWorkExperience} years` : "No experience needed"}
      {/* <ul>
        <li>2+ years experience in frontend development</li>
        <li>Strong knowledge of HTML, CSS, JavaScript, React</li>
        <li>Familiarity with REST APIs and Git</li>
      </ul> */}

      <h6>About the Company</h6>
      <p>
        Techify Solutions is a leading software firm building scalable products and enterprise tools for global clients.
      </p>
    </div>
  </div>
</div>

    </>

}

export default JobDescription;