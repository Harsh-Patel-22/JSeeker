import 'bootstrap/dist/css/bootstrap.css'
import { api } from '../services/APIClient';
import { useToast } from '../contexts/ToastContext';
import { HttpStatusCode } from 'axios';
import { useNavigate } from 'react-router';
import { jobService } from '../services/apiServices';
const NewJob = () => {
    const {showToast} = useToast();
    const nagivate = useNavigate(); 
    async function postJob(jobData) {
        try {
            const response = await jobService.createJob(jobData);
            if(response.status == HttpStatusCode.Created){
                console.log("Job posted successfully:", response.data);
                return true;
            }
            else{
                return false;
            }
        } catch (error) {
            console.error("Error posting job:", error);
            return false;
        };
    }
    
    
    return <div className="container mt-5 mb-5">
        <div className="card shadow-sm p-4">
            <h4 className="mb-4">Create New Job</h4>
            <form onSubmit={async (e) => {
                e.preventDefault();
                const jobData = {
                    "title": e.target.jobTitle.value,
                    "description": e.target.description.value,
                    "responsibilities": e.target.responsibilities.value,
                    "termsAndConditions": e.target.termsandconditions.value,
                    "jobType": e.target.jobType.value,
                    "workMode": e.target.workMode.value,
                    "requiredWorkExperience": e.target.requiredWorkExperience.value,
                    "minSalary": e.target.minimumSalary.value,
                    "maxSalary": e.target.maximumSalary.value,
                    "applicationsLimit": e.target.applicationLimit.value
                }
                let success = await postJob(jobData);
                if(success){
                    await new Promise(resolve => setTimeout(resolve, 300));
                    showToast("Job posted successfully!", true);
                    e.target.reset();
                    nagivate('/jobs');
                }
                else{
                    showToast("Job Could Not Be Created!", false);
                }
                }}>
            <div className="mb-3">
                <label htmlFor="jobTitle" className="form-label">Job Title</label>
                <input type="text" className="form-control" id="jobTitle" placeholder="e.g. Frontend Developer" required />
            </div>

           
            <div className="mb-3">
                <label htmlFor="description" className="form-label">Job Description</label>
                <textarea className="form-control" id="description" rows="5" placeholder="Describe the role, team, and expectations..." required></textarea>
            </div>

            <div className="mb-3">
                <label htmlFor="responsibilities" className="form-label">Responsibilities</label>
                <textarea className="form-control" id="responsibilities" rows="4" placeholder="List the responsibilities, one per line" required></textarea>
            </div>

            <div className="mb-3">
                <label htmlFor="termsandconditions" className="form-label">Terms & Conditions</label>
                <textarea className="form-control" id="termsandconditions" rows="4" placeholder="List the terms and conditions, one per line" required></textarea>
            </div>

            <div className="mb-3">
                <label htmlFor="requiredWorkExperience" className="form-label">Required Work Experience (in years)</label>
                <input type="number" className="form-control" id="requiredWorkExperience" placeholder="e.g. 5" required />
            </div>

            <div className="mb-3">
                <label htmlFor="jobType" className="form-label">Job Type</label>
                <select className="form-select" id="jobType" required>
                <option value="">Select type</option>
                <option value="FullTime">Full-time</option>
                <option value="PartTime">Part-time</option>
                <option value="Internship">Internship</option>
                </select>
            </div>


            <div className="mb-3">
                <label htmlFor="workMode" className="form-label">Work Mode</label>
                <select className="form-select" id="workMode" required>
                <option value="">Select mode</option>
                <option value="OnSite">On-site</option>
                <option value="WorkFromHome">Work From Home</option>
                </select>
            </div>

            <div className="mb-3">
                <label htmlFor="minimumSalary" className="form-label">Minimum Salary (in $)</label>
                <input type="number" className="form-control" id="minimumSalary" placeholder="e.g. 1250" required />
            </div>

            <div className="mb-3">
                <label htmlFor="maximumSalary" className="form-label">Maximum Salary (in $)</label>
                <input type="number" className="form-control" id="maximumSalary" placeholder="e.g. 25110" required />
            </div>
            
            <div className="mb-3">
                <label htmlFor="applicationsLimit" className="form-label">Applications Limit</label>
                <input type="number" className="form-control" id="applicationLimit" placeholder="e.g. 200" required />
            </div>

            <button type="submit" className="btn btn-primary w-100">Post Job</button>
            </form>
        </div>
    </div>

}

export default NewJob;