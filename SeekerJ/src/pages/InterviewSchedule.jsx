import "bootstrap/dist/css/bootstrap.css"
import { api } from "../services/APIClient";
import { useEffect, useState } from "react";
import { Link } from "react-router";
import { AxiosError } from "axios";

const InterviewCard = ({interviewData}) => {
    return <div className="container mt-4">
  <div className="card shadow-sm p-4 mb-4">
    <div className="d-flex justify-content-between align-items-start flex-wrap">
      <div>
        <h5 className="card-title mb-1">{interviewData.job.title}</h5>
        <p className="text-muted mb-1">Location: Bangalore, Karnataka</p>
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
          <p className="mb-1"><strong>Salary:</strong> ₹{interviewData.job.salary} per month</p>
          <p className="text-muted"><em>Terms:</em> 6-month probation. Notice period of 30 days.</p>
        </div>

        <div className="mb-3">
          <h6>Description</h6>
          <p>
            {interviewData.job.description}
          </p>
        </div>

        <div className="mb-3">
          <h6>Responsibilities</h6>
          <ul className="mb-0">
            <li>Build and maintain reusable code libraries</li>
            <li>Collaborate with backend and product teams</li>
            <li>Ensure pixel-perfect implementation</li>
          </ul>
        </div>

        <div>
          <h6>Requirements</h6>
          <ul className="mb-0">
            <li>Bachelor's degree in CS or related field</li>
            <li>2+ years of React experience</li>
            <li>Understanding of RESTful APIs and Git</li>
          </ul>
        </div>
      </div>

      <div className="col-md-5 ps-md-4 mt-4 mt-md-0">
        <h6>Applicant Details</h6>
        <div className="mb-2">
          <p className="mb-1"><strong>Name:</strong> Aryan Mehta</p>
          <p className="mb-1"><strong>Email:</strong> aryan.mehta@gmail.com</p>
          <p className="mb-1"><strong>Phone:</strong> +91 98765 43210</p>
        </div>

        <div className="mb-3">
          <h6 className="mb-1">Specialties</h6>
          <span className="badge bg-primary me-1 mb-1">React</span>
          <span className="badge bg-primary me-1 mb-1">TypeScript</span>
          <span className="badge bg-primary me-1 mb-1">Next.js</span>
          <span className="badge bg-primary me-1 mb-1">Tailwind CSS</span>
        </div>

        <div className="mb-3">
          <h6 className="mb-1">Resume</h6>
          <a href="https://example.com/resume.pdf" target="_blank">View Resume (PDF)</a>
        </div>

        <div className="mb-3">
          <h6 className="mb-1">GitHub Projects</h6>
          <ul className="mb-0">
            <li><a href="https://github.com/aryanmehta/portfolio" target="_blank">Portfolio Website</a></li>
            <li><a href="https://github.com/aryanmehta/task-manager" target="_blank">Task Manager App</a></li>
            <li><a href="https://github.com/aryanmehta/blog-platform" target="_blank">Blog Platform (MERN)</a></li>
          </ul>
        </div>
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

    useEffect(() => {
        async function fetchInterviews(){
          try {
            let response = await api.get("interview/get");
            // console.log(response.data);
            setInterviews(response.data);
          } catch (error) {
              if(error == AxiosError)
                console.log("Axios Error");
              else
                console.log(error);
          }
        }

        fetchInterviews();
    }, [])

    return<>
    {/* {console.log(interviews)} */}
    
    {interviews.map((interview) => {
        console.log(interview);
        return <InterviewCard interviewData = {interview}></InterviewCard>
    })}
    </> 
}

export default InterviewSchedule;