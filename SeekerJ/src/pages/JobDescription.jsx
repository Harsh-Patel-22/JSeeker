import { useLocation } from "react-router";
import Navbar from "../components/Navbar";
import { useEffect, useState } from "react";
import { api } from "../services/APIClient";
import { Axios, AxiosError } from "axios";

const JobDescription = () => {
    let location = useLocation();
    let {jobData} = location.state;
    let [job, setJob] = useState({});

    let type = sessionStorage.getItem("type");
    // console.log(job)

    useEffect(() => {
      async function getAllJobDetails() {
        try {
          let response = await api.get("job/description/"+ jobData);
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
        <Navbar />
<div className="container mt-4 mb-5">
  <div className="card p-4 shadow-sm">
    <div className="d-flex align-items-start justify-content-between flex-wrap">
      <div className="d-flex align-items-start">
        <img src="company-logo.png" alt="Company Logo" className="me-3 rounded" style={{width: "64px", height: "64px"}} />
        <div>
          <h4 className="mb-1">{job.title}</h4>
          <p className="mb-0 text-muted">Techify Solutions · Full-time</p>
          <p className="mb-0 text-muted">Bangalore, Karnataka, India · On-site</p>
          <small className="text-muted">Posted 2 days ago · 23 applicants</small>
        </div>
      </div>
      <div className="mt-3 mt-md-0">
        {type == "seeker" && <button className="btn btn-primary">Apply</button>}
      </div>
    </div>

    <hr className="my-4" />

    <div className="job-description">
      <h5>About the job</h5>
      <p>
        We are looking for a passionate frontend developer to join our growing team.
        You’ll work on modern UI/UX, web app features, and performance improvements.
      </p>

      <h6>Responsibilities</h6>
      <ul>
        <li>Develop responsive, reusable UI components in React</li>
        <li>Collaborate with product and design teams</li>
        <li>Ensure performance, quality, and responsiveness</li>
      </ul>

      <h6>Requirements</h6>
      <ul>
        <li>2+ years experience in frontend development</li>
        <li>Strong knowledge of HTML, CSS, JavaScript, React</li>
        <li>Familiarity with REST APIs and Git</li>
      </ul>

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