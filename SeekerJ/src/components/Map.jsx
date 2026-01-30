import 'leaflet/dist/leaflet.css'
import { MapContainer, TileLayer, useMap, Circle} from 'react-leaflet';
import './Map.css'
import { useEffect, useState } from 'react';
import MapCard from './MapCard';
import { AxiosError } from 'axios';
import { jobService, userService } from '../services/apiServices';

const Map = () => {
    // TODO - Remove all the logical code from here and add it to the upper component. Just pass in the data as props to this component in order to render
    const [position, setPosition] = useState([54.505, 45])
    // TODO - Fetch the position based on the user position for the map center, also get the nearby positions of the hirers and data from the backend and populate the map with the same.
    let [nearbyJobs, setNearbyJobs] = useState([])
    let [searchTerm, setSearchTerm] = useState("")
    let [searchDistance, setSearchDistance] = useState(5)
    
    useEffect(() => {
        async function searchAndFetchNearbyJobs() {
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
//         setNearbyJobs([
//   {
//     "id": 1,
//     "title": "Frontend Developer Intern",
//     "companyName": "TechNova Solutions",
//     "type": "Internship",
//     "distance": 2.3,
//     "address": {
//       "id": 101,
//       "houseNumber": "12A",
//       "society": "Silver Leaf Residency",
//       "street": "Prahlad Nagar Road",
//       "city": "Ahmedabad",
//       "state": "Gujarat",
//       "country": "India",
//       "postalCode": "380015",
//       "latitude": 23.0208,
//       "longitude": 72.5714
//     },
//     "hirerId": "e4b50a8e-3f3a-4df1-85c3-7b6a5c3b8a21"
//   },
//   {
//     "id": 2,
//     "title": "Backend Engineer",
//     "companyName": "CloudBridge Technologies",
//     "type": "FullTime",
//     "distance": 4.1,
//     "address": {
//       "id": 102,
//       "houseNumber": "7B",
//       "society": "Galaxy Business Park",
//       "street": "SG Highway",
//       "city": "Ahmedabad",
//       "state": "Gujarat",
//       "country": "India",
//       "postalCode": "380054",
//       "latitude": 23.0251,
//       "longitude": 72.5642
//     },
//     "hirerId": "cbeec771-1df3-4207-b412-7129ef8f1e33"
//   },
//   {
//     "id": 3,
//     "title": "UI/UX Designer",
//     "companyName": "PixelWave Studio",
//     "type": "PartTime",
//     "distance": 1.8,
//     "address": {
//       "id": 103,
//       "houseNumber": "301",
//       "society": "Sun Corporate Hub",
//       "street": "Drive-In Road",
//       "city": "Ahmedabad",
//       "state": "Gujarat",
//       "country": "India",
//       "postalCode": "380052",
//       "latitude": 23.0185,
//       "longitude": 72.5789
//     },
//     "hirerId": "7e29b52f-9093-4b57-9929-f1ab9b1e34dd"
//   }
// ]
// );
        searchAndFetchNearbyJobs();
    }, [searchDistance]);

    useEffect(() => {
      async function fetchUserCoordinates() {
        let response = await userService.getCoordinates()
        setPosition([response.data?.latitude, response.data?.longitude]);
      }
      fetchUserCoordinates();
    }, []);

    const getZoomFromDistance = (meters) => {
      if (meters <= 2000) return 14;
      if (meters <= 5000) return 13;
      if (meters <= 10000) return 12;
      return 11;
    };

    return <>
        <MapContainer 
          center={position} 
          zoom={getZoomFromDistance(searchDistance)} // TODO - set the searchDistance level dynamically based on the search distance
          scrollWheelZoom={false} 
          style={{ height: "100vh", width: "100%" }}
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
            {nearbyJobs && nearbyJobs.map((job) => (<MapCard position={[job?.address?.latitude, job?.address?.longitude]} job={job} message=""></MapCard>))}
            {console.log(nearbyJobs)}

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
            max="40"
            value={searchDistance / 1000}
            onChange={(e) => setSearchDistance(e.target.value * 1000)}
            className="slider"
            />
          <span className="slider-value">{searchDistance / 1000} km</span>
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