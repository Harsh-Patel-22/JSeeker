import { Marker, Popup } from "react-leaflet";
import "bootstrap/dist/css/bootstrap.css"
import { Link, useLocation } from "react-router";

const JobCard = ({job}) => {
    return <div className="card" style={{width: "18rem"}}>
                <img src="..." className="card-img-top" alt="..." />
                <div className="card-body">
                    <h5 className="card-title">{job.title}</h5>
                    <p className="card-text">Distance in coordinates: {job.distance}</p>
                    <a href="#" className="btn btn-primary text-light">Apply</a>
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