import { useState } from "react";
import BaseForm from "./BaseForm";
import BaseModal from "./BaseModal";
import {BaseModalWithoutButton} from "./BaseModal";
import { DetailsFillingStates } from "./SeekerSecondaryDetails";

const GetRequiredError = (fields, data) => {
  let errors = [];
    fields.map((field) => {
      if (field.required) {
        data.forEach((entry, index) => {
          if (!entry[field.name] || entry[field.name].trim() === "") {
            // console.log("Error in field:", field.name, "for entry index:", index);
            if (!errors[index]){
              errors.push({});
              // console.log("EROR:", errors);
            }
            errors[index][field.name] = `${field.label} is required`;
            // console.log("EROR2:", errors);
          }
        });
      }
    });
    console.log("Validation errors:", errors);
    return errors;
}

export const AddressModal = ({setShowAddAddressButton, updateState, onSubmit}) => {

  const fields = [
    { name: "houseNumber", label: "Building Number", type: "text", twoColumn: true },
    { name: "society", label: "Society", type: "text", twoColumn: true },
    { name: "street", label: "Street", type: "text", required: true, twoColumn: true },
    { name: "city", label: "City", type: "text", required: true, twoColumn: true },
    { name: "state", label: "State", type: "text", required: true, twoColumn: true },
    { name: "country", label: "Country", type: "text", required: true, twoColumn: true },
    { name: "postalCode", label: "Zip Code", type: "text", required: true, twoColumn: true },
  ];

  const handleSubmit = (values) => {
    console.log("Form Submitted:", values);
    updateState(DetailsFillingStates.AddressDetailsFilled);
    onSubmit({
      target: {
        name: "addressDetails",
        value: values
      }
    });
    setShowAddAddressButton(false);
  };
  
  const validate = (data) => {
    const errors = GetRequiredError(fields, data);
    return errors;
  }
  
  return (
    <BaseModal title="Add Address" triggerText="Add Address" validate={validate} handleSubmit={handleSubmit} fields={fields}></BaseModal>
  );
}

export const EducationModal = ({setShowAddEducationButton, updateState, onSubmit}) => {
  const studies = ["ssc", "hsc", "ug", "pg", "phd"];
  /* 
  const rules = {
    // No repition rule
    // Allow only certain values rule
    // At least have x entries, rule
    // Min (years) rule
    // Max (years) rule
    
    values: array,
    repition: bool,
  };
  */ 
  
  const fields = [
    { name: "study", label: "Study", type: "select", options: [{value: "ssc", label: "SSC", },{value: "hsc", label: "HSC"},{value: "phd", label: "PhD"},], twoColumn: true, required: true },
    { name: "instituteName", label: "Institute Name", type: "text", required: true, twoColumn: true },
    { name: "state", label: "State", type: "text", required: true, twoColumn: true },
    { name: "country", label: "Country", type: "text", required: true, twoColumn: true },
    { name: "startDate", label: "Start Date", type: "date", required: true, twoColumn: true },
    { name: "endDate", label: "End Date", type: "date", required: true, twoColumn: true },
  ];
  
  const handleSubmit = (values) => {
    console.log("Form Submitted:", values);
    updateState(DetailsFillingStates.EducationDetailsFilled);
    onSubmit({
      target: {
        name: "educationDetails",
        value: values
      }
    });
    setShowAddEducationButton(false);
  };

  const validate = (data) => {
    const errors = GetRequiredError(fields, data);
    return errors;
  }

  return (
    <BaseModal title="Add Education" triggerText="Add Education" validate={validate} handleSubmit={handleSubmit} fields={fields} hasMultipleEntries={true}></BaseModal>
  );
}

export const WorkExperienceModal = ({setShowAddWorkExperienceButton, updateState, onSubmit}) => {
  
  const fields = [
    { name: "companyName", label: "Company Name", type: "text", required: true, twoColumn: true },
    { name: "role", label: "Role", type: "text", twoColumn: true, required: true },
    { name: "description", label: "Description", type: "textarea", required: true},
    { name: "startDate", label: "Start Date", type: "date", required: true, twoColumn: true },
    { name: "endDate", label: "End Date", type: "date", required: true, twoColumn: true },
  ];
  
  const handleSubmit = (values) => {
    console.log("Form Submitted:", values);
    updateState(DetailsFillingStates.WorkDetailsFilled);
    onSubmit({
      target: {
        name: "workExperienceDetails",
        value: values
      }
    });
    setShowAddWorkExperienceButton(false);
  };

  const validate = (data) => {
    const errors = GetRequiredError(fields, data);
    return errors;
  }

  return (
    <BaseModal title="Add WorkExperience" triggerText="Add WorkExperience" validate={validate} handleSubmit={handleSubmit} fields={fields} hasMultipleEntries={true}></BaseModal>
  );
}

export const LanguageModal = ({setShowAddLanguageButton, updateState, onSubmit}) => {
  
  const fields = [
    { name: "name", label: "Name", type: "text", required: true, twoColumn: true },
    { name: "level", label: "Fluency", type: "select", options: [{value: "fluent", label: "Fluent"},{value: "native", label: "Native"},{value: "learning", label: "Learning"}], twoColumn: true, required: true },
];
  // TODO - Pass the formData here and add those details... Do for all.
  const handleSubmit = (values) => {
    console.log("Form Submitted:", values);
    updateState(DetailsFillingStates.LanguageDetailsFilled);
    onSubmit({
      target: {
        name: "languageDetails",
        value: values
      }
    });
    setShowAddLanguageButton(false);
  };

  const validate = (data) => {
    const errors = GetRequiredError(fields, data);
    return errors;
  }

  return (
    <BaseModal title="Add Languages" triggerText="Add Language Details" validate={validate} handleSubmit={handleSubmit} fields={fields} hasMultipleEntries={true}></BaseModal>
  );
}

export const InterviewSchedulingModal = ({show, setShow, onSubmit}) => {
  
  const fields = [
      { name: "date", label: "Date", type: "date", required: true, twoColumn: true },
      { name: "time", label: "Time", type: "time", required: true, twoColumn: true },
  ];
  
  // const handleSubmit = (values) => {
  //     console.log("Form Submitted:", values);
  // };

  const validate = (data) => {
      const errors = []
      // const errors = GetRequiredError(fields, data);
      return errors;
  }

  return (
      <BaseModalWithoutButton title="Schedule Interview" validate={validate} show={show} setShow={setShow} handleSubmit={onSubmit} fields={fields}></BaseModalWithoutButton>
  );
}

