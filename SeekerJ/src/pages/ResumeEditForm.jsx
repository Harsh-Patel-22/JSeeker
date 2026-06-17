import React, { useEffect, useState } from "react";
import { api } from "../services/APIClient";

const ResumeEditForm = () => {
  const [resumeData, setResumeData] = useState({});

  useEffect(() => {
    const fetchResume = async () => {
      try {
        const response = await api.get("user/get/resume");
        setResumeData(response.data);
        console.log("Fetched resume data:", response.data['BasicDetails']);
      } catch (error) {
        console.error("Error fetching resume data:", error);
      }
    };
    fetchResume();
  }, []);

  const handleChange = (section, field, value) => {
    setResumeData((prev) => ({
      ...prev,
      [section]: {
        ...prev[section],
        [field]: value,
      },
    }));
  };

  const handleArrayChange = (section, index, field, value) => {
    const updated = [...resumeData[section]];
    updated[index] = { ...updated[index], [field]: value };
    setResumeData((prev) => ({ ...prev, [section]: updated }));
  };

  async function handleSubmit(e){
    e.preventDefault();
    const response = await api.post("user/update/resume", resumeData);
    
  };

  return (
    <form className="container my-4" onSubmit={handleSubmit}>
      {/* BASIC DETAILS */}
      <div className="card shadow-sm mb-4">
        <div className="card-header bg-primary text-white fw-bold">
          Basic Details
        </div>
        <div className="card-body row g-3">
          {["FirstName", "LastName", "State", "Country", "AboutLine"].map(
            (field) => (
              <div key={field} className="col-md-6">
                <label className="form-label">{field}</label>
                <input
                  type="text"
                  className="form-control"
                  value={resumeData['BasicDetails']?.[field] || ""}
                  onChange={(e) =>
                    handleChange("BasicDetails", field, e.target.value)
                  }
                />
              </div>
            )
          )}
        </div>
      </div>

      {/* CONTACT DETAILS */}
      <div className="card shadow-sm mb-4">
        <div className="card-header bg-primary text-white fw-bold">
          Contact Details
        </div>
        <div className="card-body row g-3">
          {[
            "email",
            "githubProfileLink",
            "linkedInProfileLink",
            "phoneNumber",
          ].map((field) => (
            <div key={field} className="col-md-6">
              <label className="form-label">{field}</label>
              <input
                type="text"
                className="form-control"
                value={resumeData.contactDetails?.[field] || ""}
                onChange={(e) =>
                  handleChange("contactDetails", field, e.target.value)
                }
              />
            </div>
          ))}
        </div>
      </div>

      {/* PROJECTS */}
      <div className="card shadow-sm mb-4">
        <div className="card-header bg-primary text-white fw-bold">
          Projects
        </div>
        <div className="card-body">
          {Object.entries(resumeData.projectDetails || {}).map(
            ([key, project], index) => (
              <div key={key} className="border p-3 rounded mb-3">
                <h6 className="fw-bold">{project.Name}</h6>
                <div className="row g-3">
                  {["name", "description", "githubRepoLink"].map((field) => (
                    <div key={field} className="col-md-6">
                      <label className="form-label">{field}</label>
                      <input
                        type="text"
                        className="form-control"
                        value={project[field] || ""}
                        onChange={(e) => {
                          const updatedProjects = {
                            ...resumeData.projectDetails,
                            [key]: {
                              ...project,
                              [field]: e.target.value,
                            },
                          };
                          setResumeData((prev) => ({
                            ...prev,
                            projectDetails: updatedProjects,
                          }));
                        }}
                      />
                    </div>
                  ))}
                </div>
              </div>
            )
          )}
        </div>
      </div>

      {/* WORK EXPERIENCE */}
      <div className="card shadow-sm mb-4">
        <div className="card-header bg-primary text-white fw-bold">
          Work Experience
        </div>
        <div className="card-body">
          {resumeData.workExperienceDetails?.map((exp, index) => (
            <div key={index} className="border p-3 rounded mb-3">
              <div className="row g-3">
                {["role", "description", "companyName"].map((field) => (
                  <div key={field} className="col-md-6">
                    <label className="form-label">{field}</label>
                    <input
                      type="text"
                      className="form-control"
                      value={exp[field] || ""}
                      onChange={(e) =>
                        handleArrayChange(
                          "workExperienceDetails",
                          index,
                          field,
                          e.target.value
                        )
                      }
                    />
                  </div>
                ))}
              </div>
            </div>
          ))}
        </div>
      </div>

      {/* EDUCATION */}
      <div className="card shadow-sm mb-4">
        <div className="card-header bg-primary text-white fw-bold">
          Education
        </div>
        <div className="card-body">
          {resumeData.educationDetails?.map((edu, index) => (
            <div key={index} className="border p-3 rounded mb-3">
              <div className="row g-3">
                {["study", "instituteName", "state", "country"].map((field) => (
                  <div key={field} className="col-md-6">
                    <label className="form-label">{field}</label>
                    <input
                      type="text"
                      className="form-control"
                      value={edu[field] || ""}
                      onChange={(e) =>
                        handleArrayChange(
                          "educationDetails",
                          index,
                          field,
                          e.target.value
                        )
                      }
                    />
                  </div>
                ))}
              </div>
            </div>
          ))}
        </div>
      </div>

      {/* LANGUAGES */}
      <div className="card shadow-sm mb-4">
        <div className="card-header bg-primary text-white fw-bold">
          Languages
        </div>
        <div className="card-body">
          {resumeData.languageDetails?.map((lang, index) => (
            <div key={index} className="row g-3 mb-3">
              <div className="col-md-6">
                <label className="form-label">Name</label>
                <input
                  type="text"
                  className="form-control"
                  value={lang.name}
                  onChange={(e) =>
                    handleArrayChange(
                      "languageDetails",
                      index,
                      "Name",
                      e.target.value
                    )
                  }
                />
              </div>
              <div className="col-md-6">
                <label className="form-label">Level</label>
                <select
                  className="form-select"
                  value={lang.Level}
                  onChange={(e) =>
                    handleArrayChange(
                      "LanguageDetails",
                      index,
                      "Level",
                      e.target.value
                    )
                  }
                >
                  <option value="Fluent">Fluent</option>
                  <option value="Native">Native</option>
                  <option value="Learning">Learning</option>
                </select>
              </div>
            </div>
          ))}
        </div>
      </div>

      <div className="text-end">
        <button type="submit" className="btn btn-success px-4">
          Save Changes
        </button>
      </div>
    </form>
  );
};

export default ResumeEditForm;
