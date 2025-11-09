import { useState, useCallback } from "react";
import { jwtDecode } from "jwt-decode";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../../contexts/AuthContext";
import { useToast } from "../../contexts/ToastContext";
import { useProgressRedirect } from "../../hooks/useProgressRedirect";
import { authService } from "../../services/apiServices";
import BaseForm from "./BaseForm";

const AuthForm = ({ mode = "login" }) => {
  const { login, jwt } = useAuth();
  const { showToast } = useToast();
  const navigate = useNavigate();

  const [loading, setLoading] = useState(false);
  const [startRedirect, setStartRedirect] = useState(false);

  const onComplete = useCallback(() => {
  const token = localStorage.getItem("token") || jwt;
  if (!token) {
    console.error("No token found during redirect");
    return;
  }
  const decoded = jwtDecode(token);
  const role = decoded.role?.toLowerCase();
  console.log("Redirecting based on role:", role);
  navigate("/dashboard");
}, [navigate]);


  // const progress = useProgressRedirect(onComplete, startRedirect ? 50 : null);

  const fields =
    mode === "login"
      ? [
          { name: "username", label: "Email or Username", type: "text", required: true },
          { name: "password", label: "Password", type: "password", required: true },
        ]
      : [
          { name: "firstName", label: "First Name", type: "text", required: true, twoColumn: true },
          { name: "lastName", label: "Last Name", type: "text", required: true, twoColumn: true },
          { name: "phoneNumber", label: "Phone Number", type: "text", required: true },
          { name: "username", label: "Username", type: "text", required: true },
          { name: "email", label: "Email", type: "email", required: true },
          {
            name: "role",
            label: "Role",
            type: "select",
            required: true,
            options: [
              { label: "Hirer", value: "hirer" },
              { label: "Seeker", value: "seeker" },
            ],
          },
          { name: "password", label: "Password", type: "password", required: true, twoColumn: true},
          {
            name: "confirmPassword",
            label: "Confirm Password",
            type: "password",
            required: true,
            twoColumn: true,
          },
        ];

  const validate = (data) => {
    const errors = {};
    if (mode === "signup") {
      if (data.password !== data.confirmPassword) {
        errors.confirmPassword = "Passwords do not match";
      }
      if (data.phoneNumber && data.phoneNumber.length !== 10) {
        errors.phoneNumber = "Phone number must be 10 digits";
      }
    }
    return errors;
  };

  const handleSubmit = async (formData) => {
    // setLoading(true);
    console.log("Form submitted with data:", formData);
    try {
      if(mode === "login"){
        let response = await authService.login(formData);
        login(response.data);
        navigate("/dashboard");
      }
      else if (mode === "signup") {
        let response = await authService.register(formData)
        login(response.data);
        navigate(res.data.role.toLowerCase() === "hirer" ? "/hirerReg" : "/seekerReg");

      }
      console.log(res);
      showToast(`Authentication Successful!`, true);
      console.log("Authentication successful, preparing to redirect...");


    } catch (err) {
      showToast(err.response?.data?.message || "Authentication Failed", false);
    } finally {
      setLoading(false);
    }
  };

  return (
    <BaseForm
      title={mode === "signup" ? "Create an Account" : "Welcome Back"}
      fields={fields}
      validate={validate}
      onSubmit={handleSubmit}
      loading={loading}
      // redirectProgress={startRedirect ? progress : null}
    />
  );
};

export default AuthForm;
