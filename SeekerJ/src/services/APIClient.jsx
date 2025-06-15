import axios, { all } from 'axios'

export const api = axios.create({
    baseURL: "http://localhost:5275/api",
    headers: all
}) 