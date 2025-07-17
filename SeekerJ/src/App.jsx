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
import Footer from './components/Footer';
import Navbar from './components/Navbar';
import 'bootstrap/dist/css/bootstrap.css';
import UnauthorisedPage from './pages/UnauthorisedPage';
import { AuthProvider, useAuth } from './contexts/AuthContext';
import { useEffect } from 'react';

const App = () => {
  const authVar = useAuth();
  const { isAuthenticated, user } = authVar;
  useEffect(() => {
  if (user) {
    console.log("User updated in context:", user);
  }
}, [user]);
  return (
    <Routes>
      {/* {console.log("isAuthenticated:", isAuthenticated())} */}
      {/* {console.log("isAuthenticated user:", user)} */}
      {console.log("AuthVar:", authVar)}
      {isAuthenticated() ? (
        <>
          <Route path="/dashboard/hirer" element={<HirerDashboard />} />
          <Route path="/dashboard/seeker" element={<SeekerDashboard />} />
          <Route path="/job" element={<JobDescription />} />
          <Route path="/job/new" element={<NewJob />} />
          <Route path="/jobs" element={<ViewJobs />} />
          <Route path="/interviews" element={<InterviewSchedule />} />
          <Route path="/applications" element={<ApplicationsPage />} />
          <Route path="/profile" element={<ProfilePage />} />
        </>
      ) : (
        <>
          <Route path="/" element={<LoginPage />} />
          <Route path="/signup" element={<SignupPage />} />
          <Route path="/testing" element={<Testing />} />
          <Route path="/*" element={<UnauthorisedPage />} />
        </>
      )}
    </Routes>
  );
 
};

export default App
