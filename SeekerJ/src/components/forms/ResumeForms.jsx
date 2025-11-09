 import React from "react";
import BaseForm from "./BaseForm";
import { Form, Button, Row, Col } from "react-bootstrap";
import { useState } from "react";

export const StepBasicDetails = ({ onNext, defaultData }) => {
  const fields = [
    { name: "firstName", label: "First Name", type: "text", required: true, showRequired: true, twoColumn: true },
    { name: "lastName", label: "Last Name", type: "text", required: true, showRequired: true, twoColumn: true },
    { name: "state", label: "State", type: "text", twoColumn: true },
    { name: "country", label: "Country", type: "text", twoColumn: true },
    { name: "aboutLine", label: "About You", type: "textarea", required: true, showRequired: true },
  ];

  const validate = (data) => {
    const errors = {};
    if (!data.firstName) errors.firstName = "First name is required";
    if (!data.lastName) errors.lastName = "Last name is required";
    if (!data.aboutLine) errors.aboutLine = "Please write something about yourself";
    return errors;
  };

  const handleSubmit = async (data) => {
    onNext(data);
  };

  return (
    <BaseForm
    //   title="Basic Details"
      fields={fields}
      onSubmit={handleSubmit}
      validate={validate}
      loading={false}
    />
  );
};

export const StepContactDetails = ({ onNext, defaultData }) => {
  const fields = [
    { name: "email", label: "Email", type: "email", required: true, showRequired: true },
    { name: "githubProfileLink", label: "GitHub Profile", type: "text" },
    { name: "linkedInProfileLink", label: "LinkedIn Profile", type: "text" },
    { name: "phoneNumber", label: "Phone Number", type: "text", required: true, showRequired: true },
  ];

  const validate = (data) => {
    const errors = {};
    if (!data.email) errors.email = "Email is required";
    if (!data.phoneNumber) errors.phoneNumber = "Phone number is required";
    return errors;
  };

  const handleSubmit = async (data) => {
    onNext(data);
  };

  return (
    <BaseForm
    //   title="Contact Details"
      fields={fields}
      onSubmit={handleSubmit}
      validate={validate}
      loading={false}
    />
  );
};


export const StepProjects = ({ onNext, onBack, initialData = [] }) => {
  const [technologiesUsages, setTechnologiesUsages] = useState([
    { name: "", usage: "" },
  ]);

  const fields = [
    { name: "name", label: "Project Name", type: "text", required: true },
    { name: "description", label: "Description", type: "textarea", required: true },
    { name: "startDate", label: "Start Date", type: "date", required: true, twoColumn: true },
    { name: "lastUpdatedDate", label: "Last Updated", type: "date", required: true, twoColumn: true },
    { name: "githubRepoLink", label: "GitHub Repo Link", type: "text", required: false },
  ];

  const handleTechChange = (index, field, value) => {
    const updated = [...technologiesUsages];
    updated[index][field] = value;
    console.log("Updated Technologies Usages:", updated);
    setTechnologiesUsages(updated);
  };

  const addTech = () => {
    setTechnologiesUsages((prev) => [...prev, { name: "", usage: "" }]);
  };

  const removeTech = (index) => {
    setTechnologiesUsages((prev) => prev.filter((_, i) => i !== index));
  };

  const handleSubmit = (data) => {
    const cleanedTechs = technologiesUsages
      .filter((t) => t.name.trim() !== "")
      .map((t) => ({
        name: t.name,
        usage: parseFloat(t.usage || 0),
      }));

    onNext({
      projectDetails: [
        {
          ...data,
          technologiesUsages: cleanedTechs,
        },
      ],
    });
  };

  return (
    <div>
      <BaseForm fields={fields} onSubmit={handleSubmit} />

      {/* --- Technologies Section --- */}
      <div className="mt-4 p-3 border rounded-3 bg-light">
        <h5 className="mb-3 text-primary">Technologies Used</h5>

        {technologiesUsages.map((tech, index) => (
          <Row key={index} className="align-items-center mb-2">
            <Col md={6}>
              <Form.Control
                type="text"
                placeholder="Technology Name"
                value={tech.name}
                onChange={(e) =>
                  handleTechChange(index, "name", e.target.value)
                }
              />
            </Col>
            <Col md={4}>
              <Form.Control
                type="number"
                placeholder="Usage %"
                value={tech.usage}
                onChange={(e) =>
                  handleTechChange(index, "usage", e.target.value)
                }
              />
            </Col>
            <Col md={2}>
              <Button
                variant="outline-danger"
                size="sm"
                onClick={() => removeTech(index)}
                disabled={technologiesUsages.length === 1}
              >
                ✕
              </Button>
            </Col>
          </Row>
        ))}

        <Button
          variant="outline-primary"
          size="sm"
          className="mt-2"
          onClick={addTech}
        >
          + Add Technology
        </Button>
      </div>
    </div>
  );
};

export const StepExperience = ({ onNext, onBack, initialData }) => {
  const fields = [
    { name: "role", label: "Role", type: "text", required: true, twoColumn: true },
    { name: "companyName", label: "Company Name", type: "text", required: true, twoColumn: true },
    { name: "description", label: "Description", type: "textarea", required: true },
    { name: "startDate", label: "Start Date", type: "date", required: true, twoColumn: true },
    { name: "endDate", label: "End Date", type: "date", required: true, twoColumn: true },
  ];

  return (
    <BaseForm
    //   title="Work Experience"
    //   subtitle="List your professional experience."
      fields={fields}
      onSubmit={(data) => onNext({ workExperienceDetails: [data] })}
    />
  );
};

export const StepEducation = ({ onNext, onBack, initialData }) => {
  const fields = [
    { name: "study", label: "Degree / Study", type: "text", required: true, twoColumn: true },
    { name: "instituteName", label: "Institute Name", type: "text", required: true, twoColumn: true },
    { name: "state", label: "State", type: "text", required: true, twoColumn: true },
    { name: "country", label: "Country", type: "text", required: true, twoColumn: true },
    { name: "startDate", label: "Start Date", type: "date", required: true, twoColumn: true },
    { name: "endDate", label: "End Date", type: "date", required: true, twoColumn: true },
  ];

  return (
    <BaseForm
    //   title="Education Details"
    //   subtitle="Tell us about your academic background."
      fields={fields}
      onSubmit={(data) => onNext({ educationDetails: [data] })}
    />
  );
};

export const StepLanguages = ({ onNext, onBack }) => {
  const fields = [
    { name: "name", label: "Language Name", type: "text", required: true, twoColumn: true },
    {
      name: "level",
      label: "Proficiency Level",
      type: "select",
      options: [
        { label: "Learning", value: "Learning" },
        { label: "Fluent", value: "Fluent" },
        { label: "Native", value: "Native" },
      ],
      required: true,
      twoColumn: true,
    },
  ];

  return (
    <BaseForm
    //   title="Language Proficiency"
    //   subtitle="List the languages you speak."
      fields={fields}
      onSubmit={(data) => onNext({ languageDetails: [data] })}
    />
  );
};

