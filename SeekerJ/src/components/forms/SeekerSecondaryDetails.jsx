import { useState, useCallback, useEffect } from "react";
import { jwtDecode } from "jwt-decode";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../../contexts/AuthContext";
import { useToast, ToastProvider } from "../../contexts/ToastContext";
import { useProgressRedirect } from "../../hooks/useProgressRedirect";
import { userService } from "../../services/apiServices";
import BaseForm from "./BaseForm";
import { geocodeLocation } from "../../services/GeocoderService";
import { HttpStatusCode } from "axios";
import { AddressModal, EducationModal, LanguageModal, WorkExperienceModal } from "./FormModals";
import BaseModal from "./BaseModal";

export const DetailsFillingStates = {
    NoDetailsFilled: "NoDetailsFilled", 
    BasicDetailsFilled: "BasicDetailsFilled", 
    AddressDetailsFilled: "AddressDetailsFilled",
    EducationDetailsFilled: "EducationDetailsFilled",
    WorkDetailsFilled: "WorkDetailsFilled",
    LanguageDetailsFilled: "LanguageDetailsFilled"
  };

const SeekerBasicDetailsForm = () => {
  
  const { jwt } = useAuth();
  const { showToast } = useToast();
  const navigate = useNavigate();

  const [loading, setLoading] = useState(false);
  const [startRedirect, setStartRedirect] = useState(false);
  const [fillingState, setFillingState] = useState(DetailsFillingStates.BasicDetailsFilled);

  const [showAddAddressButton, setShowAddAddressButton] = useState(true);
  const [showAddEducationButton, setShowAddEducationButton] = useState(true);
  const [showAddWorkExperienceButton, setShowAddWorkExperienceButton] = useState(true);
  const [showAddLanguageButton, setShowAddLanguageButton] = useState(true);

  const onComplete = useCallback(() => {
    navigate("/dashboard/hirer");
  }, [jwt, navigate]);

  const progress = useProgressRedirect(onComplete, startRedirect ? 50 : null);

  useEffect(() => {
    console.log("Filling State Changed: ", fillingState);
  }, [fillingState]);

  const fields = [
        {name: "gender", label: "Gender", type: "radio", options: [
          { label: "Male", value: "male", name: "gender" },
          { label: "Female", value: "female", name: "gender" },
          { label: "Others", value: "others", name: "gender" },
          { label: "Prefer Not To Say", value: "preferNotToSay", name: "gender" },
        ], required: true },

        { name: "aboutLine", label: "About Line", type: "text", required: true},
        { name: "description", label: "Write about yourself", type: "text", required: true},
        { name: "linkedInProfileLink", label: "LinkedIn Profile Link", type: "text", required: true},
        { 
          name: "jobPreference", 
          label: "Job Preference", 
          type: "select", 
          options: [
            {label: "Internship", value: "internship"},
            {label: "Part Time", value: "partTime"},
            {label: "Full Time", value: "fullTime"}
          ], 
          required: true},

          (fillingState === DetailsFillingStates.BasicDetailsFilled) && showAddAddressButton && {
            name: "addressDetails", type: "modal", twoColumn: true, content: (extraProps) => (<AddressModal setShowAddAddressButton={setShowAddAddressButton} updateState={setFillingState} {...extraProps} />)
          },
          (fillingState === DetailsFillingStates.AddressDetailsFilled) && showAddEducationButton && {
            name: "educationDetails", type: "modal", twoColumn: true, content: (extraProps) => (<EducationModal setShowAddEducationButton={setShowAddEducationButton} updateState={setFillingState} {...extraProps} />)
          },
          (fillingState === DetailsFillingStates.EducationDetailsFilled) && showAddWorkExperienceButton && {
            name: "workDetails", type: "modal", twoColumn: true, content: (extraProps) => (<WorkExperienceModal setShowAddWorkExperienceButton={setShowAddWorkExperienceButton} updateState={setFillingState} {...extraProps} />)
          },
          (fillingState === DetailsFillingStates.WorkDetailsFilled) && showAddLanguageButton && {
            name: "languageDetails", type: "modal", twoColumn: true, content:(extraProps) => (<LanguageModal setShowAddLanguageButton={setShowAddLanguageButton} updateState={setFillingState} {...extraProps}/>)
          }
        ];

  const validate = (data) => {
    const errors = {};
    // if (mode === "signup") {
    //   if (data.password !== data.confirmPassword) {
    //     errors.confirmPassword = "Passwords do not match";
    //   }
    //   if (data.phoneNumber && data.phoneNumber.length !== 10) {
    //     errors.phoneNumber = "Phone number must be 10 digits";
    //   }
    // }
    return errors;
  };

  const handleSubmit = async (formData) => {
    setLoading(true);
    try {
      const address = formData.addressDetails[0];
      let geocode = await geocodeLocation(`${address.houseNumber} ${address.society} ${address.street} ${address.city} ${address.state} ${address.country} ${address.postalCode}`);
      
      let latitude = 0;
      let longitude = 0;
      if(!geocode){
        showToast("Coundn't locate your address", false);
      }
      else{
        latitude = geocode.lat;
        longitude = geocode.lng;
      }
      const secondaryDetails = {
        gender: formData.gender,
        jobPreference: formData.jobPreference,
        description: formData.description,
        aboutLine: formData.aboutLine,
        linkedInProfileLink: formData.linkedInProfileLink,

        address: {
          houseNumber: address.houseNumber,
          society: address.society,
          street: address.street,
          city: address.city,
          state: address.state,
          country: address.country,
          postalCode: address.postalCode,
          latitude: latitude,
          longitude: longitude
        },

        // TODO - Test the below things..
        educationDetails: formData.educationDetails,
        workExperienceDetails: formData.workExperienceDetails,
        VocalLanguageDetails: formData.languageDetails
      }
      console.log("Secondary Details to be sent: ", secondaryDetails);
      const res = await userService.updateSecondaryDetails(secondaryDetails);
      if(res.status === HttpStatusCode.Ok){
        showToast("Successfully Registered!", true);
        navigate("/githubForm");
        // setStartRedirect(true);
      }
    } catch (err) {
      showToast(err.response?.data?.message || "Failed", false);
    } finally {
      setLoading(false);
    }
  };

  return <div className="auth-page d-flex justify-content-center align-items-center vh-100">
      <div className="w-100" style={{ maxWidth: '420px' }}>
        <ToastProvider>
          <BaseForm
            title="Add Details"
            fields={fields}
            validate={validate}
            onSubmit={handleSubmit}
            loading={loading}
            redirectProgress={startRedirect ? progress : null}
          />
        </ToastProvider>
      </div>
    </div>
};

export default SeekerBasicDetailsForm;
