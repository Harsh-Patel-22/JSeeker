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
    var [nearbyJobs, setNearbyJobs] = useState([])
    
    useEffect(() => {
        async function searchAndFetchNearbyJobs() {
            var locationobj = {
                "latitude": position[0],
                "longitude": position[1],
                "searchdistance": 10
            }
            try {
                var response = await api.post("joblocation", locationobj);
                // console.log(response.data);
                setNearbyJobs(response.data);
                // console.log(nearbyJobs);
                // nearbyJobs.map((job) => console.log([job.latitude, job.longitude]))
            } catch (error) {
                console.log(error);
            }
        }
        searchAndFetchNearbyJobs();
    }, [nearbyJobs]);
    

    return (
        <MapContainer 
      center={position} 
      zoom={5} // TODO - set the zoom level dynamically based on the search distance
      scrollWheelZoom={false} 
      style={{ height: "100vh", width: "100vh" }}
    >
            <TileLayer url = "https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png" />

            <MapCard position={position} message={"This is you"} job={null}></MapCard>
            {nearbyJobs.map((job) => (<MapCard position={[job.location.latitude, job.location.longitude]} job={job} message=""></MapCard>))}
        </MapContainer>
    )
}

export default Map; 