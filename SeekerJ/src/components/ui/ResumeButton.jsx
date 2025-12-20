import { useState } from "react";
import SpinnerButton from "./SpinnerButton";
import { downloadResumePDF, fetchResumePDF } from "../../services/Utils";

const ResumeButton = ({ targetClientId, name="resume.pdf",useCase = "fetch", className ="btn btn-primary p-1 px-2", style={},  ...props }) => {
  const [loading, setLoading] = useState(false);

  const handleFetchPDF = async () => {
    try {
      setLoading(true);
      useCase === "fetch" ? await fetchResumePDF(targetClientId) : await downloadResumePDF(targetClientId, name);
    } catch (err) {
      console.error("Error fetching PDF:", err);
      if(err.status === 500) {
        alert("Resume not found. Please create by filling in your details.");
      }
      else{
        alert("Failed to fetch resume.");
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <SpinnerButton
      loading={loading}
      style={style}
      className={className}
      handleClick={handleFetchPDF}
      type="button"
      {...props}
    >
    </SpinnerButton>
  );
};

export default ResumeButton;
