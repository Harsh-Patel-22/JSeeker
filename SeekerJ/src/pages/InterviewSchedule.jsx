import "bootstrap/dist/css/bootstrap.css"
import { api } from "../services/APIClient";
import { useEffect, useState } from "react";
import { Link } from "react-router";
import Navbar from "../components/Navbar";
import Footer from "../components/Footer";

const InterviewCard = ({interviewData}) => {
    return <div class="container mt-4">
  <div class="card shadow-sm p-4 mb-4">
    <div class="d-flex justify-content-between align-items-start flex-wrap">
      <div>
        <h5 class="card-title mb-1">{interviewData.job.title}</h5>
        <p class="text-muted mb-1">Location: Bangalore, Karnataka</p>
        {/* <span class="badge bg-success">Open</span> */}
      </div>
      <div class="text-md-end mt-3 mt-md-0">
        <p class="mb-1"><strong>Interview Date:</strong> {interviewData.date}</p>
        <p class="mb-1"><strong>Time:</strong> {interviewData.time}</p>
        <p class="mb-0"><strong>Mode:</strong> <span class="badge bg-success">{interviewData.mode}</span></p>
      </div>
    </div>

    <hr />

    <div class="row">
      <div class="col-md-7 pe-md-5 border-end">
        <div class="mb-3">
          <p class="mb-1"><strong>Salary:</strong> ₹{interviewData.job.salary} per month</p>
          <p class="text-muted"><em>Terms:</em> 6-month probation. Notice period of 30 days.</p>
        </div>

        <div class="mb-3">
          <h6>Description</h6>
          <p>
            {interviewData.job.description}
          </p>
        </div>

        <div class="mb-3">
          <h6>Responsibilities</h6>
          <ul class="mb-0">
            <li>Build and maintain reusable code libraries</li>
            <li>Collaborate with backend and product teams</li>
            <li>Ensure pixel-perfect implementation</li>
          </ul>
        </div>

        <div>
          <h6>Requirements</h6>
          <ul class="mb-0">
            <li>Bachelor's degree in CS or related field</li>
            <li>2+ years of React experience</li>
            <li>Understanding of RESTful APIs and Git</li>
          </ul>
        </div>
      </div>

      <div class="col-md-5 ps-md-4 mt-4 mt-md-0">
        <h6>Applicant Details</h6>
        <div class="mb-2">
          <p class="mb-1"><strong>Name:</strong> Aryan Mehta</p>
          <p class="mb-1"><strong>Email:</strong> aryan.mehta@gmail.com</p>
          <p class="mb-1"><strong>Phone:</strong> +91 98765 43210</p>
        </div>

        <div class="mb-3">
          <h6 class="mb-1">Specialties</h6>
          <span class="badge bg-primary me-1 mb-1">React</span>
          <span class="badge bg-primary me-1 mb-1">TypeScript</span>
          <span class="badge bg-primary me-1 mb-1">Next.js</span>
          <span class="badge bg-primary me-1 mb-1">Tailwind CSS</span>
        </div>

        <div class="mb-3">
          <h6 class="mb-1">Resume</h6>
          <a href="https://example.com/resume.pdf" target="_blank">View Resume (PDF)</a>
        </div>

        <div class="mb-3">
          <h6 class="mb-1">GitHub Projects</h6>
          <ul class="mb-0">
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