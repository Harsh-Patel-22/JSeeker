import axios, { all } from 'axios'
import { useAuth } from '../contexts/AuthContext';



export const api = axios.create({
    baseURL: "http://localhost:5275/api",
    headers: {
        'Content-Type': 'application/json',
    }
})

export const blobApi = axios.create({
    baseURL: "http://localhost:5275/api",
    headers: {
        'Content-Type': 'application/json',
    },
    responseType: 'blob',
})

api.interceptors.request.use(
    config => {
        const token = sessionStorage.getItem("jwt");
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;0
        }
        return config;
    },
    error => {
        return Promise.reject(error);
    }
);

blobApi.interceptors.request.use(
    config => {
        const token = sessionStorage.getItem("jwt");
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;0
        }
        return config;
    },
    error => {
        return Promise.reject(error);
    }
);