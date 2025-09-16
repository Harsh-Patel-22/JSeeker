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
    if (!jwt) return;
    const decoded = jwtDecode(jwt);
    const role = decoded.role?.toLowerCase();
    navigate(role === "hirer" ? "/dashboard/hirer" : "/dashboard/seeker");
  }, [jwt, navigate]);

  const progress = useProgressRedirect(onComplete, startRedirect ? 50 : null);

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
    setLoading(true);
    try {
      const res =
        mode === "signup"
          ? await authService.register(formData)
          : await authService.login(formData);

      login(res.data); // store token
      showToast(`${mode === "signup" ? "Signup" : "Login"} successful!`, true);
      setStartRedirect(true);
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
      redirectProgress={startRedirect ? progress : null}
    />
  );
};

export default AuthForm;
