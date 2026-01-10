import { useEffect, useState, useRef } from "react";
import { useNavigate } from "react-router-dom";
import { Container, Card, ProgressBar, Button, Spinner } from "react-bootstrap";
import { StepBasicDetails, StepContactDetails, StepEducation, StepLanguages, StepExperience, StepProjects } from "../components/forms/ResumeForms";
import { useToast } from "../contexts/ToastContext";
import { userService } from "../services/apiServices";
import { HttpStatusCode } from "axios";

const ResumeBuilderPage = () => {
  const [step, setStep] = useState(0);
  const [loading, setLoading] = useState(true);

  const [formData, setFormData] = useState({
    basicDetails: {},
    contactDetails: {},
    projectDetails: {},
    workExperienceDetails: [],
    educationDetails: [],
    languageDetails: [],
  });

  const steps = [
    { title: "Basic Details", formName: "basicDetails", component: StepBasicDetails },
    { title: "Contact Details", formName: "contactDetails", component: StepContactDetails },
    { title: "Projects", formName: "projectDetails", component: StepProjects },
    { title: "Experience", formName: "workExperienceDetails", formNameForDeleted: "deletedWorkExperienceDetails", component: StepExperience },
    { title: "Education", formName: "educationDetails", formNameForDeleted: "deletedEducationDetails", component: StepEducation },
    { title: "Languages", formName: "languageDetails", formNameForDeleted: "deletedLanguageDetails", component: StepLanguages },
  ];

  const CurrentStep = steps[step].component;
  const progress = ((step + 1) / steps.length) * 100;
  const { showToast } = useToast();
  const navigate = useNavigate();
  const stepRef = useRef();

  useEffect(() => {
    const fetchResume = async () => {
      try {
        const res = await userService.getResume();
        if (res?.data) {
          setFormData({
            basicDetails: res?.data?.BasicDetails || {},
            contactDetails: res?.data?.ContactDetails || {},
            projectDetails: res?.data?.ProjectDetails || {},
            workExperienceDetails: res?.data?.WorkExperienceDetails || [],
            educationDetails: res?.data?.EducationDetails || [],
            languageDetails: res?.data?.LanguageDetails || [],

            deletedWorkExperienceDetails: [],
            deletedEducationDetails: [],
            deletedLanguageDetails: [],
          });
        }
      } catch (err) {
        console.error("Resume fetch failed", err);
      } finally {
        setLoading(false);
      }
    };

    fetchResume();
  }, []);

  const handleNext = async () => {
    const data = stepRef?.current?.getData();
    console.log("Data from step ", step, data);
    setFormData(prev => ({
      ...prev,
      [steps[step].formName]: data
    }));
    if (step == 0 || step == 1) {
      // No deleted data for these steps
    }
    else{
      if(steps[step].formNameForDeleted){
        const deletedData = stepRef?.current?.getDeletedData();
        setFormData(prev => ({
          ...prev,
          [steps[step].formNameForDeleted]: deletedData
        }));
      }
    }

    if (step === steps.length - 1) {
      const response = await userService.updateResumeContents(formData);
      console.log("Resume save response:", response);
      if (response.status !== HttpStatusCode.Ok) {
        showToast("Resume save failed", false);
      } else {
        showToast("Resume saved successfully!", true);
        navigate("/profile");
      }
    } else {
      setStep(prev => prev + 1);
    }
  };

  const handleBack = () => {
    const data = stepRef?.current?.getData();

    setFormData(prev => ({
      ...prev,
      [steps[step].formName]: data
    }));

    setStep(prev => prev - 1);
  };

  if (loading) {
    return (
      <div className="text-center mt-5">
        <Spinner />
      </div>
    );
  }

  return (
    <Container className="py-5">
      <Card className="p-4 shadow-sm rounded-4 border-0">
        <div className="text-center mb-4">
          <h3 className="fw-bold text-primary">{steps[step].title == "Projects" ? "Projects (Read Only)" : steps[step].title}</h3>
          <ProgressBar now={progress} label={`${Math.round(progress)}%`} />
        </div>

        <CurrentStep
          ref={stepRef}
          initialData={formData[steps[step].formName]}
        />

        <div className="d-flex justify-content-between mt-4">
          <Button variant="outline-secondary" disabled={step === 0} onClick={handleBack}>
            ← Back
          </Button>
          <Button
            variant="primary"
            onClick={handleNext}
            >
            {step === steps.length - 1 ? "Finish" : "Next →"}
          </Button>
        </div>
      </Card>
    </Container>
  );
};

export default ResumeBuilderPage;
