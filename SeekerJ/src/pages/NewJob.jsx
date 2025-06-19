import 'bootstrap/dist/css/bootstrap.css'

const NewJob = () => {
    return <div className="container mt-5 mb-5">
        <div className="card shadow-sm p-4">
            <h4 className="mb-4">Create New Job</h4>
            <form>
            <div className="mb-3">
                <label for="jobTitle" className="form-label">Job Title</label>
                <input type="text" className="form-control" id="jobTitle" placeholder="e.g. Frontend Developer" required />
            </div>

            <div className="mb-3">
                <label for="companyName" className="form-label">Company Name</label>
                <input type="text" className="form-control" id="companyName" placeholder="e.g. Techify Solutions" required />
            </div>

            <div className="mb-3">
                <label for="jobType" className="form-label">Job Type</label>
                <select className="form-select" id="jobType" required>
                <option value="">Select type</option>
                <option>Full-time</option>
                <option>Part-time</option>
                <option>Contract</option>
                <option>Internship</option>
                </select>
            </div>

            <div className="mb-3">
                <label for="location" className="form-label">Job Location</label>
                <input type="text" className="form-control" id="location" placeholder="e.g. Bangalore, Karnataka, India" required />
            </div>

            <div className="mb-3">
                <label for="workMode" className="form-label">Work Mode</label>
                <select className="form-select" id="workMode" required>
                <option value="">Select mode</option>
                <option>On-site</option>
                <option>Remote</option>
                <option>Hybrid</option>
                </select>
            </div>

            <div className="mb-3">
                <label for="description" className="form-label">Job Description</label>
                <textarea className="form-control" id="description" rows="5" placeholder="Describe the role, team, and expectations..." required></textarea>
            </div>

            <div className="mb-3">
                <label for="responsibilities" className="form-label">Responsibilities</label>
                <textarea className="form-control" id="responsibilities" rows="4" placeholder="List responsibilities, one per line" required></textarea>
            </div>

            <div className="mb-3">
                <label for="requirements" className="form-label">Requirements</label>
                <textarea className="form-control" id="requirements" rows="4" placeholder="List required qualifications, one per line" required></textarea>
            </div>

            <div className="mb-4">
                <label for="logoUpload" className="form-label">Company Logo</label>
                <input className="form-control" type="file" id="logoUpload" accept="image/*" />
            </div>

            <button type="submit" className="btn btn-primary w-100">Post Job</button>
            </form>
        </div>
    </div>

}

export default NewJob;