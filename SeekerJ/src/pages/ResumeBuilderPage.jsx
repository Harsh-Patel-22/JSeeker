import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Container, Card, ProgressBar, Button } from "react-bootstrap";
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
    { title: "Experience", formName: "workExperienceDetails", component: StepExperience },
    { title: "Education", formName: "educationDetails", component: StepEducation },
    { title: "Languages", formName: "languageDetails", component: StepLanguages },
  ];

  const CurrentStep = steps[step].component;
  const progress = ((step + 1) / steps.length) * 100;
  const { showToast } = useToast();
  const navigate = useNavigate();

  // 🔹 Fetch resume once
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

  const handleNext = async (data) => {
    const keys = [
      "basicDetails",
      "contactDetails",
      "projectDetails",
      "workExperienceDetails",
      "educationDetails",
      "languageDetails",
    ];

    let valueToSet = data;

    if (
      ["projectDetails", "workExperienceDetails", "educationDetails", "languageDetails"]
        .includes(keys[step])
    ) {
      valueToSet = data[keys[step]] || [];
    }

    const updated = { ...formData, [keys[step]]: valueToSet };
    setFormData(updated);

    if (step < steps.length - 1) {
      setStep(step + 1);
    } else {
      showToast("Saving resume...", true);
      const response = await userService.updateResumeContents(updated);

      if (response.status !== HttpStatusCode.Ok) {
        showToast("Resume save failed", false);
      } else {
        showToast("Resume saved successfully!", true);
        navigate("/dashboard");
      }
    }
  };

  const handleBack = () => step > 0 && setStep(step - 1);

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
          <h3 className="fw-bold text-primary">{steps[step].title}</h3>
          <ProgressBar now={progress} label={`${Math.round(progress)}%`} />
        </div>

        <CurrentStep
          onBack={handleBack}
          onNext={handleNext}
          initialData={formData[steps[step].formName]}
        />

        <div className="d-flex justify-content-between mt-4">
          <Button variant="outline-secondary" disabled={step === 0} onClick={handleBack}>
            ← Back
          </Button>
          <Button
            variant="primary"
            type="submit"
            onClick={handleNext}
            form={`form-step-${step}`}
          >
            {step === steps.length - 1 ? "Finish" : "Next →"}
          </Button>
        </div>
      </Card>
    </Container>
  );
};

export default ResumeBuilderPage;
