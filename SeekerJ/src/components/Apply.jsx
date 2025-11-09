import { api } from "../services/APIClient";

export async function Apply({seekerId, jobId, hirerId, jobType}) {
    let response = await api.post('application/create', {"seekerId": seekerId, "jobId": jobId, "hirerId": hirerId, "jobType": jobType})
    if(response.status === 200){
        return true;
    }
    return false
};

export async function Schedule({interviewId, date, time}) {
    let response = await api.post(`interview/update/DateTime/${interviewId}`, {date: date, time: time});
    if(response.status === 200){
        return true;
    }
    return false;
}
// export default Apply;