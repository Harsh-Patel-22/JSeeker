import { useLocation } from "react-router";
import { useEffect, useState } from "react";
import JobForm from "../components/forms/JobForm";
import { jobService } from "../services/apiServices";

const EditJob = () => {
  const location = useLocation();
  const { jobId } = location.state;
  const [job, setJob] = useState(null);

  useEffect(() => {
    async function fetchJob() {
      console.log("Fetching job with ID:", jobId);
      const response = await jobService.getDescriptionById(jobId);
      setJob(response.data);
    }
    fetchJob();
  }, [jobId]);

  if (!job) return <div className="text-center mt-5">Loading...</div>;

  return <JobForm mode="edit" jobData={job} />;
};

export default EditJob;
