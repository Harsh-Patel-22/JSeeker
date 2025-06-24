import React, { useState } from 'react';
import { geocodeLocation, reverseGeocode } from '../services/GeocoderService';

export default function GeoTool() {
  const [locationInput, setLocationInput] = useState('');
  const [latLng, setLatLng] = useState(null);
  const [address, setAddress] = useState(null);

  const handleGeocode = async () => {
    try {
      const result = await geocodeLocation(locationInput);
      setLatLng(result);
    } catch (e) {
      alert('Error geocoding location');
    }
  };

  const handleReverseGeocode = async () => {
    if (!latLng) return;

    try {
      const result = await reverseGeocode(latLng.lat, latLng.lng);
      setAddress(result.address);
    } catch (e) {
      alert('Error reverse geocoding');
    }
  };

  return (
    <div className="p-4">
      <h2>GeoTool</h2>
      <input
        type="text"
        value={locationInput}
        onChange={(e) => setLocationInput(e.target.value)}
        placeholder="Enter address or location"
      />
      <button onClick={handleGeocode}>Get Coordinates</button>

      {latLng && (
        <div>
          <p>Latitude: {latLng.lat}</p>
          <p>Longitude: {latLng.lng}</p>
          <button onClick={handleReverseGeocode}>Get Address from Coordinates</button>
        </div>
      )}

      {address && (
        <div>
          <h4>Resolved Address:</h4>
          <p>{address}</p>
        </div>
      )}
    </div>
  );
}
