import BaseForm from "./BaseForm";
import { Form, Button, Row, Col, Collapse, Card } from "react-bootstrap";
import { useState, useEffect, forwardRef, useImperativeHandle } from "react";

export const StepBasicDetails = forwardRef(({ initialData = {} }, ref) => {
  const [draft, setDraft] = useState(initialData);

  useEffect(() => {
    setDraft(initialData || {});
  }, [initialData]);

  useImperativeHandle(ref, () => ({
    getData: () => draft,
  }));


  const fields = [
    { name: "FirstName", label: "First Name", type: "text", required: true, showRequired: true, twoColumn: true },
    { name: "LastName", label: "Last Name", type: "text", required: true, showRequired: true, twoColumn: true },
    { name: "State", label: "State", type: "text", twoColumn: true },
    { name: "Country", label: "Country", type: "text", twoColumn: true },
    { name: "AboutLine", label: "About You", type: "textarea", required: true, showRequired: true },
  ];

  const validate = (data) => {
    const errors = {};
    if (!data.firstName) errors.firstName = "First name is required";
    if (!data.lastName) errors.lastName = "Last name is required";
    if (!data.aboutLine) errors.aboutLine = "Please write something about yourself";
    return errors;
  };

  return (
    <>
    {console.log("Initial Data in Basic Details:", initialData)}
    <BaseForm
    //   title="Basic Details"
    fields={fields}
    onChange={setDraft}
    initialData={initialData}
    validate={validate}
    loading={false}
    showSubmit={false}
    />
    </>
  );
});

export const StepContactDetails = forwardRef(
  ({ initialData = {} }, ref) => {

    const [draft, setDraft] = useState(initialData);

    useEffect(() => {
      setDraft(initialData || {});
    }, [initialData]);


    const fields = [
      { name: "Email", label: "Email", type: "email", required: true, showRequired: true },
      { name: "GithubProfileLink", label: "GitHub Profile", type: "text" },
      { name: "LinkedInProfileLink", label: "LinkedIn Profile", type: "text" },
      { name: "PhoneNumber", label: "Phone Number", type: "text", required: true, showRequired: true },
    ];

    const validate = (data) => {
      const errors = {};
      if (!data.Email) errors.Email = "Email is required";
      if (!data.PhoneNumber) errors.PhoneNumber = "Phone number is required";
      return errors;
    };

    useImperativeHandle(ref, () => ({
      getData: () => draft,
    }));

    return (
      <BaseForm
        fields={fields}
        initialData={initialData}
        validate={validate}
        showSubmit={false}
        loading={false}
        onChange={setDraft} // updates local draft only
      />
    );
  }
);


export const StepProjects = forwardRef(({ initialData = [] }, ref) => {

  const fields = [
    { name: "Name", label: "Project Name", type: "text", required: true },
    { name: "Description", label: "Description", type: "textarea", required: true },
    { name: "StartDate", label: "Start Date", type: "date", twoColumn: true },
    { name: "LastUpdatedDate", label: "Last Updated", type: "date", twoColumn: true },
    { name: "GithubRepoLink", label: "GitHub Repo Link", type: "text" },
  ];

  
  const [projects, setProjects] = useState([]);
  const [activeIndex, setActiveIndex] = useState(null);

  const [versions, setVersions] = useState(
    () => projects.map(() => 0)
  );


  useEffect(() => {
    if (Array.isArray(initialData) && initialData.length > 0) {
      setProjects(initialData);
    } else {
      setProjects([{}]);
    }
  }, [initialData]);

  useEffect(() => {
    setVersions(
      (Array.isArray(initialData) && initialData.length
        ? initialData
        : [{}]
      ).map(() => 0)
    );
  }, [initialData]);


  const addProject = () => {
    setProjects(prev => {
      setActiveIndex(prev.length);
      return [...prev, {}];
    });
  };

  const clearProject = (index) => {
    setProjects(prev => {
      const updated = [...prev];
      updated[index] = {};
      return updated;
    });

    setVersions(prev => {
      const updated = [...prev];
      updated[index] += 1;
      return updated;
    });
  };


  const deleteProject = (index) => {
    setProjects(prev => {
      if (prev.length === 1) return prev;

      const updated = prev.filter((_, i) => i !== index);

      setActiveIndex(prevIndex => {
        if (prevIndex === index) return null;
        if (prevIndex > index) return prevIndex - 1;
        return prevIndex;
      });

      return updated;
    });
  };

  const toggleAccordion = (index) => {
    setActiveIndex((prev) => (prev === index ? null : index));
  };


  const updateProject = (index, data) => {
    setProjects((prev) => {
      const updated = [...prev];
      updated[index] = data;
      return updated;
    });
  };

  useImperativeHandle(ref, () => ({
    getData: () => projects,
  }));

  return (
    <>
      {projects.map((project, index) => (
        <Card key={index} className="mb-3">
          <Card.Header
            className="d-flex justify-content-between align-items-center"
            onClick={() => toggleAccordion(index)}
            style={{ cursor: "pointer" }}
          >
            <span>
              Project {index + 1}
              {project.Name && ` — ${project.Name}`}
            </span>
            <div className="d-flex align-items-center gap-2">
                {activeIndex === index && (
                  <Button
                    variant="warning"
                    size="sm"
                    onClick={(e) => {
                      e.stopPropagation();
                      clearProject(index);
                    }}
                  >
                    Clear
                  </Button>
                )}

                <Button
                  variant="danger"
                  size="sm"
                  disabled={projects.length === 1}
                  onClick={(e) => {
                    e.stopPropagation();
                    deleteProject(index);
                  }}
                >
                  🗑️
                </Button>

                <span className="ms-2">{activeIndex === index ? "▲" : "▼"}</span>
              </div>
          </Card.Header>

          <Collapse in={activeIndex === index}>
            <div className="p-3">
              <BaseForm
                key={`project-${index}-${versions[index]}`}
                formId={`form-step-2-${index}`}
                fields={fields}
                initialData={project}
                onChange={(data) => updateProject(index, data)}
                showSubmit={false}
              />
            </div>
          </Collapse>
        </Card>
      ))}

        <Button variant="outline-primary" onClick={addProject}>
          ➕ Add Project
        </Button>
    </>
  ); 
});

export const StepExperience = forwardRef(({ initialData = [] }, ref) => {
  const fields = [
    { name: "Role", label: "Role", type: "text", required: true, twoColumn: true },
    { name: "CompanyName", label: "Company Name", type: "text", required: true, twoColumn: true },
    { name: "Description", label: "Description", type: "textarea", required: true },
    { name: "StartDate", label: "Start Date", type: "date", required: true, twoColumn: true },
    { name: "EndDate", label: "End Date", type: "date", required: true, twoColumn: true },
  ];

  const [experiences, setExperiences] = useState([]);
  const [activeIndex, setActiveIndex] = useState(null);

  const [versions, setVersions] = useState(
    () => experiences.map(() => 0)
  );

  useEffect(() => {
    if (Array.isArray(initialData) && initialData.length > 0) {
      setExperiences(initialData);
    } else {
      setExperiences([{}]);
    }
  }, [initialData]);

  useEffect(() => {
    setVersions(
      (Array.isArray(initialData) && initialData.length
        ? initialData
        : [{}]
      ).map(() => 0)
    );
  }, [initialData]);

  useImperativeHandle(ref, () => ({
    getData: () => experiences,
  }));

  const addExperience = () => {
    setExperiences(prev => {
      setActiveIndex(prev.length);
      return [...prev, {}];
    });
  };

  const clearExperience = (index) => {
    setExperiences(prev => {
      const updated = [...prev];
      updated[index] = {};
      return updated;
    });

    setVersions(prev => {
      const updated = [...prev];
      updated[index] += 1;
      return updated;
    });
  };


  const deleteExperience = (index) => {
    setExperiences(prev => {
      if (prev.length === 1) return prev;

      const updated = prev.filter((_, i) => i !== index);

      setActiveIndex(prevIndex => {
        if (prevIndex === index) return null;
        if (prevIndex > index) return prevIndex - 1;
        return prevIndex;
      });

      return updated;
    });
  };

  const toggleAccordion = (index) => {
    setActiveIndex(prev => (prev === index ? null : index));
  };

  const updateExperience = (index, data) => {
    const updated = [...experiences];
    updated[index] = data;
    setExperiences(updated);
  };

  return (
    <>
      {experiences.map((exp, index) => (
        <Card key={index} className="mb-3">
          <Card.Header
            onClick={() => toggleAccordion(index)}
            className="d-flex justify-content-between align-items-center"
            style={{ cursor: "pointer" }}
          >
            <span>
              Experience {index + 1}
              {exp.Role && ` — ${exp.Role}`}
            </span>
            <div className="d-flex align-items-center gap-2">
                {activeIndex === index && (
                  <Button
                    variant="warning"
                    size="sm"
                    onClick={(e) => {
                      e.stopPropagation();
                      clearExperience(index);
                    }}
                  >
                    Clear
                  </Button>
                )}

                <Button
                  variant="danger"
                  size="sm"
                  disabled={experiences.length === 1}
                  onClick={(e) => {
                    e.stopPropagation();
                    deleteExperience(index);
                  }}
                >
                  🗑️
                </Button>

                <span className="ms-2">{activeIndex === index ? "▲" : "▼"}</span>
              </div>
          </Card.Header>

          <Collapse in={activeIndex === index}>
            <div className="p-3">
              <BaseForm
              key={`experience-${index}-${versions[index]}`}
                fields={fields}
                initialData={exp}
                onChange={(data) => updateExperience(index, data)}
                showSubmit={false}
              />
            </div>
          </Collapse>
        </Card>
      ))}

      <Button variant="outline-primary" onClick={addExperience}>
        ➕ Add Experience
      </Button>
    </>
  );
});

export const StepEducation = forwardRef(({ initialData = [] }, ref) => {
  const fields = [
    { name: "Study", label: "Degree / Study", type: "text", required: true, twoColumn: true },
    { name: "InstituteName", label: "Institute Name", type: "text", required: true, twoColumn: true },
    { name: "State", label: "State", type: "text", required: true, twoColumn: true },
    { name: "Country", label: "Country", type: "text", required: true, twoColumn: true },
    { name: "StartDate", label: "Start Date", type: "date", required: true, twoColumn: true },
    { name: "EndDate", label: "End Date", type: "date", required: true, twoColumn: true },
  ];

  const [educationList, setEducationList] = useState([]);
  const [activeIndex, setActiveIndex] = useState(null);

  const [versions, setVersions] = useState(
    () => educationList.map(() => 0)
  );

  useEffect(() => {
    if (Array.isArray(initialData) && initialData.length > 0) {
      setEducationList(initialData);
    } else {
      setEducationList([{}]);
    }
  }, [initialData]);

  useEffect(() => {
    setVersions(
      (Array.isArray(initialData) && initialData.length
        ? initialData
        : [{}]
      ).map(() => 0)
    );
  }, [initialData]);

  useImperativeHandle(ref, () => ({
    getData: () => educationList,
  }));

  const addEducation = () => {
    setEducationList(prev => {
      setActiveIndex(prev.length);
      return [...prev, {}];
    });
  };

  const clearEducation = (index) => {
    setEducationList(prev => {
      const updated = [...prev];
      updated[index] = {};
      return updated;
    });

    setVersions(prev => {
      const updated = [...prev];
      updated[index] += 1;
      return updated;
    });
  };


  const deleteEducation = (index) => {
    setEducationList(prev => {
      if (prev.length === 1) return prev;

      const updated = prev.filter((_, i) => i !== index);

      setActiveIndex(prevIndex => {
        if (prevIndex === index) return null;
        if (prevIndex > index) return prevIndex - 1;
        return prevIndex;
      });

      return updated;
    });
  };

  const toggleAccordion = (index) => {
    setActiveIndex(prev => (prev === index ? null : index));
  };

  const updateEducation = (index, data) => {
    const updated = [...educationList];
    updated[index] = data;
    setEducationList(updated);
  };

  return (
    <>
      {educationList.map((edu, index) => (
        <Card key={index} className="mb-3">
          <Card.Header
            onClick={() => toggleAccordion(index)}
            className="d-flex justify-content-between align-items-center"
            style={{ cursor: "pointer" }}
          >
            <span>
              Education {index + 1}
              {edu.Study && ` — ${edu.Study}`}
            </span>
              <div className="d-flex align-items-center gap-2">
                {activeIndex === index && (
                  <Button
                    variant="warning"
                    size="sm"
                    onClick={(e) => {
                      e.stopPropagation();
                      clearEducation(index);
                    }}
                  >
                    Clear
                  </Button>
                )}

                <Button
                  variant="danger"
                  size="sm"
                  disabled={educationList.length === 1}
                  onClick={(e) => {
                    e.stopPropagation();
                    deleteEducation(index);
                  }}
                >
                  🗑️
                </Button>

                <span className="ms-2">{activeIndex === index ? "▲" : "▼"}</span>
              </div>
          </Card.Header>

          <Collapse in={activeIndex === index}>
            <div className="p-3">
              <BaseForm
                key={`education-${index}-${versions[index]}`}
                fields={fields}
                initialData={edu}
                onChange={(data) => updateEducation(index, data)}
                showSubmit={false}
              />
            </div>
          </Collapse>
        </Card>
      ))}

      <Button variant="outline-primary" onClick={addEducation}>
        ➕ Add Education
      </Button>
    </>
  );
});

export const StepLanguages = forwardRef(({ initialData = [] }, ref) => {
  const fields = [
    { name: "Name", label: "Language Name", type: "text", required: true, twoColumn: true },
    {
      name: "Level",
      label: "Proficiency Level",
      type: "select",
      required: true,
      twoColumn: true,
      options: [
        { label: "Learning", value: "Learning" },
        { label: "Fluent", value: "Fluent" },
        { label: "Native", value: "Native" },
      ],
    },
  ];

  const [languages, setLanguages] = useState([]);
  const [activeIndex, setActiveIndex] = useState(null);
  const [versions, setVersions] = useState(
    () => languages.map(() => 0)
  );

  useEffect(() => {
    console.log("Initial Data in Languages:", initialData);
    if (Array.isArray(initialData) && initialData.length > 0) {
      setLanguages(initialData);
    } else {
      setLanguages([{}]);
    }
  }, [initialData]);

  useEffect(() => {
    setVersions(
      (Array.isArray(initialData) && initialData.length
        ? initialData
        : [{}]
      ).map(() => 0)
    );
  }, [initialData]);

  useImperativeHandle(ref, () => ({
    getData: () => languages,
  }));

  const addLanguage = () => {
    setLanguages(prev => {
      setActiveIndex(prev.length);
      return [...prev, {}];
    });
  };

  const clearLanguage = (index) => {
    setLanguages(prev => {
      const updated = [...prev];
      updated[index] = {};
      return updated;
    });

    setVersions(prev => {
      const updated = [...prev];
      updated[index] += 1;
      return updated;
    });
  };


  const deleteLanguage = (index) => {
    setLanguages(prev => {
      if (prev.length === 1) return prev;

      const updated = prev.filter((_, i) => i !== index);

      setActiveIndex(prevIndex => {
        if (prevIndex === index) return null;
        if (prevIndex > index) return prevIndex - 1;
        return prevIndex;
      });

      return updated;
    });
  };

  const toggleAccordion = (index) => {
    setActiveIndex(prev => (prev === index ? null : index));
  };

  const updateLanguage = (index, data) => {
    const updated = [...languages];
    updated[index] = data;
    setLanguages(updated);
  };

  return (
    <>
      {languages.map((lang, index) => (
        <Card key={index} className="mb-3">
          <Card.Header
            onClick={() => toggleAccordion(index)}
            className="d-flex justify-content-between align-items-center"
            style={{ cursor: "pointer" }}
          >
            <span>
              Language {index + 1}
              {lang.Name && ` — ${lang.Name}`}
            </span>
            <div className="d-flex align-items-center gap-2">
                {activeIndex === index && (
                  <Button
                    variant="warning"
                    size="sm"
                    onClick={(e) => {
                      e.stopPropagation();
                      clearLanguage(index);
                    }}
                  >
                    Clear
                  </Button>
                )}

                <Button
                  variant="danger"
                  size="sm"
                  disabled={languages.length === 1}
                  onClick={(e) => {
                    e.stopPropagation();
                    deleteLanguage(index);
                  }}
                >
                  🗑️
                </Button>

                <span className="ms-2">{activeIndex === index ? "▲" : "▼"}</span>
              </div>
          </Card.Header>

          <Collapse in={activeIndex === index}>
            <div className="p-3">
              <BaseForm
                key={`language-${index}-${versions[index]}`}
                fields={fields}
                initialData={lang}
                onChange={(data) => updateLanguage(index, data)}
                showSubmit={false}
              />
            </div>
          </Collapse>
        </Card>
      ))}

      <Button variant="outline-primary" onClick={addLanguage}>
        ➕ Add Language
      </Button>
    </>
  );
});