import 'bootstrap/dist/css/bootstrap.css'
import { HttpStatusCode } from 'axios';
import { useNavigate } from 'react-router';
import { useToast } from '../../contexts/ToastContext';
import { jobService } from '../../services/apiServices';
import './JobForm.css';

const JobForm = ({ mode = "create", jobData = null }) => {
  const { showToast } = useToast();
  const navigate = useNavigate();
  const isEditMode = mode === "edit";

  async function submitJob(data) {
    try {
      const response = isEditMode
        ? await jobService.updateJob(jobData.id, data)
        : await jobService.createJob(data);

      const successStatus = isEditMode
        ? HttpStatusCode.Ok
        : HttpStatusCode.Created;

      if (response.status === successStatus) {
        showToast(
          isEditMode ? "Job updated successfully!" : "Job posted successfully!",
          true
        );
        navigate('/jobs');
      } else {
        throw new Error("Unexpected response");
      }
    } catch (error) {
      console.log("data to submit:", data);
      console.error(error);
      showToast(
        isEditMode ? "Job could not be updated!" : "Job could not be created!",
        false
      );
    }
  }

  return (
    <div className="container mt-5 mb-5">
      <div className="card shadow-sm p-4 job-form-card">

        <div className="d-flex align-items-center justify-content-between mb-4">
        <button
            type="button"
            className="btn btn-outline-secondary"
            onClick={() => navigate('/jobs')}
        >
            ← Back
        </button>

        <h4 className="mb-0">
            {isEditMode ? "Edit Job" : "Create New Job"}
        </h4>

        {/* Spacer to keep title centered */}
        <div style={{ width: "75px" }} />
        </div>


        {/* Tabs */}
        <ul className="nav nav-tabs mb-4" role="tablist">
          <li className="nav-item">
            <button className="nav-link active" data-bs-toggle="tab" data-bs-target="#basic">
              Basic
            </button>
          </li>
          <li className="nav-item">
            <button className="nav-link" data-bs-toggle="tab" data-bs-target="#details">
              Details
            </button>
          </li>
          <li className="nav-item">
            <button className="nav-link" data-bs-toggle="tab" data-bs-target="#salary">
              Salary
            </button>
          </li>
          <li className="nav-item">
            <button className="nav-link" data-bs-toggle="tab" data-bs-target="#settings">
              Settings
            </button>
          </li>
        </ul>

        <form
          noValidate
          onSubmit={async (e) => {
            e.preventDefault();
            const f = e.target;

            const missingFields = [];

            if (!f.jobTitle.value) missingFields.push("Job Title");
            if (!f.description.value) missingFields.push("Description");
            if (!f.responsibilities.value) missingFields.push("Responsibilities");
            if (!f.termsandconditions.value) missingFields.push("Terms & Conditions");
            if (!f.minimumSalary.value) missingFields.push("Minimum Salary");
            if (!f.maximumSalary.value) missingFields.push("Maximum Salary");
            if (!f.jobType.value) missingFields.push("Job Type");
            if (!f.workMode.value) missingFields.push("Work Mode");
            if (!f.applicationLimit.value) missingFields.push("Applications Limit");

            if (missingFields.length > 0) {
              showToast(
                `Please fill: ${missingFields.join(", ")}`,
                false
              );
              return;
            }

            let payload = {};

            if(jobData) {
              // Edit mode
              payload = {
                title: f.jobTitle.value,
                description: f.description.value,
                responsibilities: f.responsibilities.value,
                termsAndConditions: f.termsandconditions.value,
                minSalary: f.minimumSalary.value,
                maxSalary: f.maximumSalary.value,
                type: f.jobType.value,
                workMode: f.workMode.value,
                status: jobData.status,
                applicationsLimit: f.applicationLimit.value,
                requiredWorkExperience: f.requiredWorkExperience.value,
              };
            }
            else{
              // Create mode
              payload = {
                title: f.jobTitle.value,
                description: f.description.value,
                responsibilities: f.responsibilities.value,
                termsAndConditions: f.termsandconditions.value,
                minSalary: f.minimumSalary.value,
                maxSalary: f.maximumSalary.value,
                type: f.jobType.value,
                workMode: f.workMode.value,
                applicationsLimit: f.applicationLimit.value,
                requiredWorkExperience: f.requiredWorkExperience.value,
              };
            }

            await submitJob(payload);
          }}
        >
          <div className="tab-content job-form-tabs">

            {/* BASIC */}
            <div className="tab-pane fade show active" id="basic">
              <div className="mb-3">
                <label className="form-label">Job Title</label>
                <input
                  className="form-control"
                  name="jobTitle"
                  placeholder="e.g. Frontend Developer"
                  defaultValue={jobData?.title || ""}
                />
              </div>

              <div className="mb-3">
                <label className="form-label">Job Description</label>
                <textarea
                  className="form-control"
                  rows="5"
                  name="description"
                  placeholder="Describe the role, team, and expectations..."
                  defaultValue={jobData?.description || ""}
                />
              </div>
            </div>

            {/* DETAILS */}
            <div className="tab-pane fade" id="details">
              <div className="mb-3">
                <label className="form-label">Responsibilities</label>
                <textarea
                  className="form-control"
                  rows="4"
                  name="responsibilities"
                  placeholder="List the responsibilities, one per line"
                  defaultValue={jobData?.responsibilities || ""}
                />
              </div>

              <div className="mb-3">
                <label className="form-label">Terms & Conditions</label>
                <textarea
                  className="form-control"
                  rows="4"
                  name="termsandconditions"
                  placeholder="List the terms and conditions, one per line"
                  defaultValue={jobData?.termsAndConditions || ""}
                />
              </div>
            </div>

            {/* SALARY */}
            <div className="tab-pane fade" id="salary">
              <div className="row">
                <div className="col-md-6 mb-3">
                  <label className="form-label">Minimum Salary</label>
                  <input
                    type="number"
                    className="form-control"
                    name="minimumSalary"
                    placeholder="e.g. 1250"
                    defaultValue={jobData?.minSalary || ""}
                  />
                </div>

                <div className="col-md-6 mb-3">
                  <label className="form-label">Maximum Salary</label>
                  <input
                    type="number"
                    className="form-control"
                    name="maximumSalary"
                    placeholder="e.g. 25110"
                    defaultValue={jobData?.maxSalary || ""}
                  />
                </div>
              </div>
            </div>

            {/* SETTINGS */}
            <div className="tab-pane fade" id="settings">
              <div className="mb-3">
                <label className="form-label">Required Experience (years)</label>
                <input
                  type="number"
                  className="form-control"
                  name="requiredWorkExperience"
                  placeholder="e.g. 5"
                  defaultValue={jobData?.requiredWorkExperience || ""}
                />
              </div>

              <div className="mb-3">
                <label className="form-label">Job Type</label>
                <select
                  className="form-select"
                  name="jobType"
                  defaultValue={jobData?.type || ""}
                >
                  <option value="">Select type</option>
                  <option value="FullTime">Full-time</option>
                  <option value="PartTime">Part-time</option>
                  <option value="Internship">Internship</option>
                </select>
              </div>

              <div className="mb-3">
                <label className="form-label">Work Mode</label>
                <select
                  className="form-select"
                  name="workMode"
                  defaultValue={jobData?.workMode || ""}
                >
                  <option value="">Select mode</option>
                  <option value="OnSite">On-site</option>
                  <option value="WorkFromHome">Work From Home</option>
                </select>
              </div>

              <div className="mb-3">
                <label className="form-label">Applications Limit</label>
                <input
                  type="number"
                  className="form-control"
                  name="applicationLimit"
                  placeholder="e.g. 200"
                  defaultValue={jobData?.applicationsLimit || ""}
                />
              </div>
            </div>

          </div>

          <button type="submit" className="btn btn-primary w-100 mt-4">
            {isEditMode ? "Update Job" : "Post Job"}
          </button>
        </form>
      </div>
    </div>
  );
};

export default JobForm;
