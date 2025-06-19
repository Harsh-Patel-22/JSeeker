import { BrowserRouter, Routes, Route } from 'react-router';
import LoginPage from './pages/LoginPage';
import HirerDashboard from './pages/HirerDashboard';
import SignupPage from './pages/SignupPage';
import SeekerDashboard from './pages/SeekerDashboard';
import JobDescription from './pages/JobDescription';
import ViewJobs from './pages/ViewJobs';
import NewJob from './pages/NewJob';
function App() {

  return (
    <>
      <BrowserRouter> 
        <Routes>
          <Route path="/" element={<LoginPage/>} />
          <Route path="/dashboard/hirer" element={<HirerDashboard/>} />
          <Route path="/dashboard/seeker" element={<SeekerDashboard/>} />
          <Route path='/job' element={<JobDescription></JobDescription>} />
          <Route path='/job/new' element={<NewJob></NewJob>} />
          <Route path='/jobs' element={<ViewJobs></ViewJobs>} />
          <Route path='/signup' element={<SignupPage/>} />
        </Routes>
      </BrowserRouter>
    </>
  )
}

export default App
