import { Marker, Popup } from "react-leaflet";
import "bootstrap/dist/css/bootstrap.css"
import { Link, useLocation } from "react-router";
import { applyToJob } from "../services/Utils";
import ConfirmModal from "./forms/ConfirmModal";
import { useAuth } from "../contexts/AuthContext";
import { useState } from "react";
import { useToast } from "../contexts/ToastContext";
import { HttpStatusCode } from "axios";

const JobCard = ({job}) => {
    const [loading, setLoading] = useState(false);
    const [showConfirm, setShowConfirm] = useState(false);
    const {showToast} = useToast();
    let {user} = useAuth();
    async function apply(applicationData) {
            setLoading(true);
            try{
              let response = await applyToJob(applicationData);
              if(response.status == HttpStatusCode.Ok){
                showToast("Application Created Successfully!", true);
              }
              else{
                showToast("Error in Creating Application!", false);
              }
            }
            catch(error){
              if(error.status == HttpStatusCode.InternalServerError){
                console.log("Internal Server Error");
                showToast("Application Already Exist!", false);
            }
          }
            setShowConfirm(false);
            setLoading(false)
        }
    return <div className="card" style={{width: "18rem"}}>
                <img src="..." className="card-img-top" alt="..." />
                <div className="card-body">
                    <h5 className="card-title">{job.title}</h5>
                    <p className="card-text">Distance in coordinates: {job.distance}</p>
                    <a href="#" onClick={() => setShowConfirm(true)} className="btn btn-primary text-light">Apply</a>
                    <ConfirmModal loading={loading} show={showConfirm} onConfirm={() => apply({"seekerId": user.clientId, "jobId": job.id, "hirerId": job.hirerId, "jobType": job.type})} onCancel={() => setShowConfirm(false)}  message={<>Confirm Application Creation</>}/>
                    <Link to="/job" state={{'jobData': job.id}} className="btn btn-danger text-light">More Details</Link>
                </div>
            </div>
}

const MapCard = ({position, message, job}) => {
    return (
        <Marker position={position}>
            <Popup>
                {job == null ? message : <JobCard job={job}></JobCard>}
            </Popup>
        </Marker>
    )
}

export default MapCard;