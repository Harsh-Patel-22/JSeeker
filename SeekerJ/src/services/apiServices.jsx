import { api } from './APIClient';

export const authService = {
  login: async (credentials) => await api.post('auth/login', credentials),
  register: async (data) => await api.post('auth/register', data),
  registerHirer: async (data) => await api.post('user/update/hirer', data),
};

export const userService = {
  updateSecondaryDetails: async (data) => await api.post('user/update', data),
  updateGithubUsername: async (data) => await api.post('user/update/github', data),
  updateRepoNames: async (data) => await api.post('user/update/repoNames', data),
  updateResume: async (data) => await api.post('user/update/resume', data),
  getResume: async (data) => await api.get('get/resume', data),
  getSelfResumePdf: async (data) => await api.get('get/resume/pdf', data),
  // TODO - TO add profile related details fetching calls 
};

export const jobService = {
  getNearbyJobs: async (searchDistance, searchFilter) => await api.post('job/location/searchRadius=' + searchDistance, searchFilter),
  getRelevantJobs: async (searchFilter) => await api.post('job/get', searchFilter),
  createJob: async (creationData) => await api.post('job/new', creationData),
  updateJob: async (jobId, updateData) => await api.post('job/update/' + jobId, updateData),
  updateJobStatus: async (jobId, status) => await api.post('job/update/status/' + jobId, status),
  getDescriptionById: async (jobId) => await api.get('job/description/' + jobId)
};

export const applicationService = {
  updateStatus: async (data) => await api.post('application/status', data),
  scheduleInterview: async (data) => await api.post('interview/create', data)

};


