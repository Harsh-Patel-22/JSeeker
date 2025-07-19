import { api } from './APIClient';

export const authService = {
  login: (credentials) => api.post('auth/login', credentials),
  register: (data) => api.post('auth/register', data),
};
