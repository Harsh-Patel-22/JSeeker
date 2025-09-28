import 'leaflet/dist/leaflet.css'
import { MapContainer, TileLayer, useMap, Circle} from 'react-leaflet';
import './Map.css'
import { useEffect, useState } from 'react';
import MapCard from './MapCard';
import { AxiosError } from 'axios';
import { jobService } from '../services/apiServices';

// TODO - Add a circle with the user position as the centre. A transparent green circle showing the search distance. 
const Map = () => {
    // TODO - Remove all the logical code from here and add it to the upper component. Just pass in the data as props to this component in order to render
    const position = [54.505, 45] // TODO - set the position dynamically based on the user lcoation 
    // TODO - Fetch the position based on the user position for the map center, also get the nearby positions of the hirers and data from the backend and populate the map with the same.
    let [nearbyJobs, setNearbyJobs] = useState([])
    let [searchTerm, setSearchTerm] = useState("")
    let [searchDistance, setSearchDistance] = useState(10)
    
    useEffect(() => {
        async function searchAndFetchNearbyJobs() {
            let locationobj = {
                "latitude": position[0],
                "longitude": position[1],
                "searchdistance": searchDistance / 120000
            }
            try {
                let response = await jobService.getNearbyJobs(searchDistance, {"type": "Internship", "status": "Open", "mode": "OnSite"});
                setNearbyJobs(response.data);
            } catch (error) {
                if(error == AxiosError)
                  console.log("Axios Error");
                else
                  console.log(error)
            }
        }
        searchAndFetchNearbyJobs();
    }, [searchDistance]);

    return <>
        <MapContainer 
      center={position} 
      zoom={5} // TODO - set the searchDistance level dynamically based on the search distance
      scrollWheelZoom={false} 
      style={{ height: "75vh", width: "100%" }}
    >
            <TileLayer url = "https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png" />
            <MapViewUpdater center={position}/>

            <Circle center={position} radius={searchDistance} pathOptions={{
            color: "lightgreen",
            fillColor: "rgba(0, 255, 123, 0.3)",
            fillOpacity: 0.4,
            weight: 2,
          }}
        />

            <MapCard position={position} message={"This is you"} job={null}></MapCard>
            {nearbyJobs && nearbyJobs.map((job) => (<MapCard position={[job.location.latitude, job.location.longitude]} job={job} message=""></MapCard>))}

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
            max="20"
            value={searchDistance / 120000}
            onChange={(e) => setSearchDistance(e.target.value * 120000)}
            className="slider"
            />
          <span className="slider-value">{searchDistance/ 120000}</span>
        </div>
        <center>Set Search Distance</center>
      </div>
    </>
    
}

const MapViewUpdater = ({ center, zoom }) => {
  const map = useMap();

  useEffect(() => {
    map.setView(center, zoom);
  }, [center, zoom, map]);

  return null;
};


export default Map; 