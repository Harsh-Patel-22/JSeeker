import 'bootstrap/dist/css/bootstrap.css'
import { api } from '../services/APIClient';
const NewJob = () => {
    async function postJob(jobData) {
        try {
            const response = await api.post('/job/new', jobData);
            console.log("Job posted successfully:", response.data);
        } catch (error) {
            console.error("Error posting job:", error)
        };
    }


    return <div className="container mt-5 mb-5">
        <div className="card shadow-sm p-4">
            <h4 className="mb-4">Create New Job</h4>
            <form onSubmit={async (e) => {
                e.preventDefault();
                const jobData = {
                    title: e.target.jobTitle.value,
                    description: e.target.description.value,
                    responsibilities: e.target.responsibilities.value.split('\n'),
                    termsAndConditions: e.target.termsandconditions.value.split('\n'),
                    jobType: e.target.jobType.value,
                    workMode: e.target.workMode.value,
                    requiredWorkExperience: e.target.requiredWorkExperience.value,
                    minimumSalary: e.target.minimumSalary.value,
                    maximumSalary: e.target.maximumSalary.value,
                    applicationsLimit: e.target.applicationLimit.value
                }
                await postJob(jobData);
                e.target.reset();
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
                <label htmlFor="jobType" className="form-label">Job Type</label>
                <select className="form-select" id="jobType" required>
                <option value="">Select type</option>
                <option>Full-time</option>
                <option>Part-time</option>
                <option>Contract</option>
                <option>Internship</option>
                </select>
            </div>


            <div className="mb-3">
                <label htmlFor="workMode" className="form-label">Work Mode</label>
                <select className="form-select" id="workMode" required>
                <option value="">Select mode</option>
                <option>Onsite</option>
                <option>Remote</option>
                <option>Hybrid</option>
                </select>
            </div>

            <div className="mb-3">
                <label htmlFor="requiredWorkExperience" className="form-label">Required Work Experience (in years)</label>
                <input type="number" className="form-control" id="requiredWorkExperience" placeholder="e.g. 5" required />
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