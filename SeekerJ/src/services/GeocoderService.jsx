import axios from 'axios';

const OPENCAGE_API_KEY = 'f9b2bbeffe5e4479bd03b9f0fd8cad6a'; 

const BASE_URL = 'https://api.opencagedata.com/geocode/v1/json';

export const geocodeLocation = async (locationString) => {
  try {
    const response = await axios.get(BASE_URL, {
      params: {
        q: locationString,
        key: OPENCAGE_API_KEY,
        limit: 1
      }
    });

    const result = response.data.results[0];
    if (!result) throw new Error('No results found');

    return {
      lat: result.geometry.lat,
      lng: result.geometry.lng,
      formatted: result.formatted
    };
  } catch (error) {
    console.error('Geocoding error:', error.message);
    throw error;
  }
};

export const reverseGeocode = async (lat, lng) => {
  try {
    const response = await axios.get(BASE_URL, {
      params: {
        q: `${lat},${lng}`,
        key: OPENCAGE_API_KEY,
        limit: 1
      }
    });

    const result = response.data.results[0];
    if (!result) throw new Error('No address found');

    return {
      address: result.formatted,
      components: result.components
    };
  } catch (error) {
    console.error('Reverse geocoding error:', error.message);
    throw error;
  }
};
