import { BrowserRouter, Routes, Route } from 'react-router';
import LoginPage from './pages/LoginPage';
import HirerDashboard from './pages/HirerDashboard';
import SignupPage from './pages/SignupPage';
import SeekerDashboard from './pages/SeekerDashboard';
import JobDescription from './pages/JobDescription';
import ViewJobs from './pages/ViewJobs';
import NewJob from './pages/NewJob';
import InterviewSchedule from './pages/InterviewSchedule';
import ApplicationsPage from './pages/ApplicantsPage';
import Testing from './pages/Testing';
import ProfilePage from './pages/ProfilePage';

function App() {

  return (
    <>
    {sessionStorage.setItem("type", "hirer")}
    {sessionStorage.setItem("clientId", "2")}
      <BrowserRouter> 
        <Routes>
          <Route path="/" element={<LoginPage/>} />
          <Route path="/dashboard/hirer" element={<HirerDashboard/>} />
          <Route path="/dashboard/seeker" element={<SeekerDashboard/>} />
          <Route path='/job' element={<JobDescription />} />
          <Route path='/job/new' element={<NewJob/>} />
          <Route path='/jobs' element={<ViewJobs/>} />
          <Route path='/interviews' element={<InterviewSchedule/>} />
          <Route path='/applications' element={<ApplicationsPage/>}/>
          <Route path='/signup' element={<SignupPage/>} />
          <Route path='/testing' element={<Testing/>} />
          <Route path='/profile' element={<ProfilePage />}/>
        </Routes>
      </BrowserRouter>
    </>
  )
}

export default App
