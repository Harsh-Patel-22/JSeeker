import { api } from './APIClient';

export const authService = {
  login: async (credentials) => await api.post('auth/login', credentials),
  register: async (data) => await api.post('auth/register', data),
  registerHirer: async (data) => await api.post('user/update/hirer', data),
};

export const userService = {
  updateSecondaryDetails: async (data) => await api.post('user/update', data),
  updateResumeContents: async (resumeContentsDto) => await api.post('user/update/resume', resumeContentsDto),
  updateGithubUsername: async (data) => await api.post('user/update/github', data),
  updateRepoNames: async (data) => await api.post('user/update/repoNames', data),
  updateAutoPickRepoNames: async () => await api.post('user/generate/repoNames'),
  updateResume: async (data) => await api.post('user/update/resume', data),
  getResume: async (data) => await api.get('get/resume', data),
  getResumePdf: async (targetGuid) => await api.post('user/get/resume/pdf', targetGuid),
  getCoordinates: async () => await api.get('user/get/coordinates'),
  // TODO - TO add profile related details fetching calls 
  getSeekerProfileDetails: async () => await api.get('user/profile/details/seeker'),
  getHirerProfileDetails: async () => await api.get('user/profile/details/hirer'),
  getHirerDashboardDetails: async () => await api.get('user/get/dashboard'),
};

export const jobService = {
  getNearbyJobs: async (searchDistance, searchFilter) => await api.post('job/location/searchRadius=' + searchDistance, searchFilter),
  getRelevantJobs: async (searchFilter) => await api.post('job/get', searchFilter),
  getAppliedJobs: async () => await api.get('job/get/applied'),
  createJob: async (creationData) => await api.post('job/new', creationData),
  updateJob: async (jobId, updateData) => await api.post('job/update/' + jobId, updateData),
  updateJobStatus: async (jobId, status) => await api.post('job/update/status/' + jobId, status),
  getDescriptionById: async (jobId) => await api.get('job/description/' + jobId)
};

export const applicationService = {
  apply: async (applicationDto) => await api.post('application/create', applicationDto),
  getByStatus: async (status) => await api.get(`application/get/state=${status}`),
  updateStatus: async (data) => await api.post('application/status', data),
  getById: async (id) => await api.get(`application/get/${id}`),
  scheduleInterview: async (data) => await api.post('interview/create', data),
};

export const interviewService = {
  getInterviews: async (state) => await api.get('interview/get/state=' + state),
  getById: async (interviewId) => await api.get('interview/get/' + interviewId),
  updateDateTime: async (interviewId, dateTimeDto) => await api.post('interview/update/DateTime/' + interviewId, dateTimeDto),
  setInterviewScheduled: async (interviewId) => await api.post('interview/scheduled/'+ interviewId),
  updateSuccessStatus: async (interviewId, outcome) => await api.post('interview/update/success/' + interviewId, outcome),
};

export const miscApiService = {
  getMetrics: async () => await api.get('metrics/get'),
}
