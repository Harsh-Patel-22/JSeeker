import 'leaflet/dist/leaflet.css'
import { MapContainer, Marker, TileLayer, Popup} from 'react-leaflet';
import './Map.css'
import { useEffect, useState } from 'react';
import { api } from '../services/APIClient';
import MapCard from './MapCard';


const Map = () => {
    // TODO - Remove all the logical code from here and add it to the upper component. Just pass in the data as props to this component in order to render
    const position = [54.505, 45] // TODO - set the position dynamically based on the user lcoation 
    // TODO - Fetch the position based on the user position for the map center, also get the nearby positions of the hirers and data from the backend and populate the map with the same.
    let [nearbyJobs, setNearbyJobs] = useState([])
    let [searchTerm, setSearchTerm] = useState("")
    let [zoom, setZoom] = useState(5)
    
    useEffect(() => {
        async function searchAndFetchNearbyJobs() {
            let locationobj = {
                "latitude": position[0],
                "longitude": position[1],
                "searchdistance": 10
            }
            try {
                let response = await api.post("jobs/location", locationobj);
                // console.log(response.data);
                setNearbyJobs(response.data);
                // console.log(nearbyJobs);
                // nearbyJobs.map((job) => console.log([job.latitude, job.longitude]))
            } catch (error) {
                console.log(error);
            }
        }
        searchAndFetchNearbyJobs();
    }, []);
    
    useEffect(() => {
      function logOnUpdate() {
        console.log(zoom);
      }
      
      logOnUpdate();
    }, [zoom])

    return <>
        <MapContainer 
      center={position} 
      zoom={zoom} // TODO - set the zoom level dynamically based on the search distance
      scrollWheelZoom={false} 
      style={{ height: "75vh", width: "100%" }}
    >
            <TileLayer url = "https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png" />

            <MapCard position={position} message={"This is you"} job={null}></MapCard>
            {nearbyJobs.map((job) => (<MapCard position={[job.location.latitude, job.location.longitude]} job={job} message=""></MapCard>))}
        </MapContainer>

        <div className="overlay-controls">
        <input
          type="text"
          placeholder="Search skill based jobs..."
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
          className="search-bar"
        />

        <div className="slider-wrapper">
          <input
            type="range"
            min="1"
            max="10"
            value={zoom}
            onChange={(e) => setZoom(e.target.value)}
            className="slider"
          />
          <span className="slider-value">{zoom}</span>
        </div>
      </div>
    </>
    
}

export default Map; 