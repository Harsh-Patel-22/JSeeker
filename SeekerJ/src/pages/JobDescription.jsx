import { useLocation } from "react-router";
import { useEffect, useState } from "react";
import { api } from "../services/APIClient";
import { Axios, AxiosError } from "axios";
import { useAuth } from "../contexts/AuthContext";
import { Apply } from "../components/Apply";
import { jobService } from "../services/apiServices";
import { postedDateToText } from "../services/Utils";

const JobDescription = () => {
    let location = useLocation();
    let {jobData} = location.state;
    let [job, setJob] = useState({});

    let {user} = useAuth();
    let type = user.role.toLowerCase();
    // console.log(job)

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
  <div className="card p-4 shadow-sm">
    <div className="d-flex align-items-start justify-content-between flex-wrap">
      <div className="d-flex align-items-start">
        <img src="company-logo.png" alt="Company Logo" className="me-3 rounded" style={{width: "64px", height: "64px"}} />
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
        {type == "seeker" && <button className="btn btn-primary0" onClick={Apply({seekerId:user["clientId"], hirerId:job.hirerId, jobId: job.id, jobType: job.type})}>Apply</button>}
      </div>
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