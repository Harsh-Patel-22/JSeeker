import { Marker, Popup } from "react-leaflet";

const MapCard = ({position, message}) => {
    return (
        <Marker position={position}>
            <Popup>
                {message}
            </Popup>
        </Marker>
    )
}

export default MapCard;