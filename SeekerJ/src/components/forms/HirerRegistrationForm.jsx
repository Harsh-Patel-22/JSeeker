import { useState, useCallback } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../../contexts/AuthContext";
import { useToast, ToastProvider } from "../../contexts/ToastContext";
import { useProgressRedirect } from "../../hooks/useProgressRedirect";
import { authService } from "../../services/apiServices";
import BaseForm from "./BaseForm";
import { geocodeLocation } from "../../services/GeocoderService";
import { HttpStatusCode } from "axios";

const HirerRegistrationForm = () => {
  const { showToast } = useToast();
  const navigate = useNavigate();

  const [loading, setLoading] = useState(false);
  const [startRedirect, setStartRedirect] = useState(false);

  const onComplete = useCallback(() => {
    navigate("/dashboard");
  }, [navigate]);

  const progress = useProgressRedirect(onComplete, startRedirect ? 50 : null);

  const fields = [
          { name: "companyName", label: "Company Name", type: "text", required: true, twoColumn: true },
          { name: "designation", label: "Designation", type: "text", required: true, twoColumn: true },
          { name: "websiteLink", label: "Company Website Link", type: "text", required: true },

          {},
          // Address Fields
          {name: "companyAddress", label: "Company Address", type: "section"},
          { name: "houseNumber", label: "Building Number", type: "text", twoColumn: true },
          { name: "society", label: "Society", type: "text", twoColumn: true },
          { name: "street", label: "Street", type: "text", required: true, twoColumn: true},
          { name: "city", label: "City", type: "text", required: true, twoColumn: true },
          { name: "state", label: "State", type: "text", required: true, twoColumn: true },
          { name: "country", label: "Country", type: "text", required: true, twoColumn: true},
          { name: "postalCode", label: "Zip Code", type: "text", required: true, twoColumn: true},
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
      let geocode = await geocodeLocation(`${formData.houseNumber} ${formData.society} ${formData.street} ${formData.city} ${formData.state} ${formData.country} ${formData.postalCode}`);
      let latitude = 0;
      let longitude = 0;
      if(!geocode){
        showToast("Coundn't locate your address", false);
      }
      else{
        latitude = geocode.lat;
        longitude = geocode.lng;
      }
      const res = await authService.registerHirer({
        companyName: formData.companyName,
        designation: formData.designation,
        websiteLink: formData.websiteLink,

        companyAddress: {
          houseNumber: formData.houseNumber,
          society: formData.society,
          street: formData.street,
          city: formData.city,
          state: formData.state,
          country: formData.country,
          postalCode: formData.postalCode,
          latitude: latitude,
          longitude: longitude
        }
      });
      if(res.status === HttpStatusCode.Ok){
        showToast("Successfully Registered!", true);
        setStartRedirect(true);
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
            title="Hirer Details"
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

export default HirerRegistrationForm;
