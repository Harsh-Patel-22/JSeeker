import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { Container, Card, ProgressBar, Button } from "react-bootstrap";
import { StepBasicDetails, StepContactDetails, StepEducation, StepLanguages, StepExperience, StepProjects } from "../components/forms/ResumeForms";
import { useToast } from "../contexts/ToastContext";
import { userService } from "../services/apiServices";
import { HttpStatusCode } from "axios";

const ResumeBuilderPage = () => {
  const [step, setStep] = useState(0);
  const [formData, setFormData] = useState({
    basicDetails: {},
    contactDetails: {},
    projectDetails: {},
    workExperienceDetails: [],
    educationDetails: [],
    languageDetails: [],
  });

  const steps = [
    { title: "Basic Details", component: StepBasicDetails },
    { title: "Contact Details", component: StepContactDetails },
    { title: "Projects", component: StepProjects },
    { title: "Experience", component: StepExperience },
    { title: "Education", component: StepEducation },
    { title: "Languages", component: StepLanguages },
  ];

  const CurrentStep = steps[step].component;
  const progress = ((step + 1) / steps.length) * 100;
  const {showToast} = useToast();
  const navigate = useNavigate();
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

  // unwrap nested objects for array steps
  if (["projectDetails", "workExperienceDetails", "educationDetails", "languageDetails"].includes(keys[step])) {
    valueToSet = data[keys[step]] || [];
  }

    setFormData((prev) => ({
      ...prev,
      [keys[step]]: valueToSet,
    }));
    if (step < steps.length - 1) setStep(step + 1);
    else{
        
        showToast("Resume creation started!", true);
        let response = await userService.updateResumeContents({ ...formData, [keys[step]]: valueToSet });
        console.log("Final Submit Data:", { ...formData, [keys[step]]: valueToSet });
        console.log(response)
        if(response.status !== HttpStatusCode.Ok){
            showToast("Resume creation failed. Please try again.", false);
        }
        else{
            showToast("Resume creation successful!", true);
            navigate("/dashboard");
        }
    }
  };

  const handleBack = () => {
    if (step > 0) setStep(step - 1);
  };

  return (
    <Container className="py-5">
      <Card className="p-4 shadow-sm rounded-4 border-0">
        <div className="text-center mb-4">
          <h3 className="fw-bold text-primary">{steps[step].title}</h3>
          <ProgressBar now={progress} label={`${Math.round(progress)}%`} className="mt-3" />
        </div>

        <CurrentStep onBack={handleBack} onNext={handleNext} defaultData={formData} />

        <div className="d-flex justify-content-between mt-4">
          <Button
            variant="outline-secondary"
            disabled={step === 0}
            onClick={handleBack}
          >
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
