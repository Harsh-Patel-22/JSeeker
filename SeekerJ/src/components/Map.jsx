import 'leaflet/dist/leaflet.css'
import { MapContainer, Marker, TileLayer, Popup } from 'react-leaflet';
import './Map.css'

const Map = () => {
    const position = [51.505, -0.09]
    // TODO - Fetch the position based on the user position for the map center, also get the nearby positions of the hirers and data from the backend and populate the map with the same.
    
    return <>
        <MapContainer center={position} scrollWheelZoom={false}>
            <TileLayer url = "https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png">
                <Marker position={position}>
                    <Popup>Hi</Popup>
                    {/* TODO - Add a custom popup card component with minimalist information and an apply button */}
                </Marker>
            </TileLayer>
        </MapContainer>
    </>
}

export default Map; 