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
import 'bootstrap/dist/css/bootstrap.css';
import UnauthorisedPage from './pages/UnauthorisedPage';
import { AuthProvider, useAuth } from './contexts/AuthContext';
import { useEffect } from 'react';
import Navbar from './components/Navbar';
import Footer from './components/Footer';
import ResumeEditForm from './pages/ResumeEditForm';
import HirerRegistrationForm from './components/forms/HirerRegistrationForm';
import SeekerSecondaryDetails from './components/forms/SeekerSecondaryDetails';
import GithubForm from './components/forms/GithubForm';
import MetricsLandingPage from './pages/MetricsLandingPage';
import UnauthNavbar from './components/UnauthNavbar';
import ResumeBuilderPage from './pages/ResumeBuilderPage';
import EditJob from './pages/EditJob';
import About from './pages/About';
import Features from './pages/Features';

const App = () => {
  const authVar = useAuth();
  const { isAuthenticated, user } = authVar;
  useEffect(() => {
  if (user) {
    console.log("User updated in context:", user);
  }
}, [user]);
  return <>
    {isAuthenticated() ? <Navbar /> : <UnauthNavbar />}
    <div className="min-vh-100">
    <Routes>
      {/* {console.log("isAuthenticated:", isAuthenticated())} */}
      {/* {console.log("isAuthenticated user:", user)} */}
      {console.log("AuthVar:", authVar)}
      {isAuthenticated() ? (
        <>

          {/* <Route path="/" element={<LoginPage />} /> */}
          {console.log(user?.role)}
          {user.role === "Hirer" ? (
            <>
              <Route path="/dashboard" element={<HirerDashboard />} />
              <Route path="/job/new" element={<NewJob />} />
              <Route path="/job/edit" element={<EditJob />} />
              <Route path="/applications" element={<ApplicationsPage />} />

              {/* Unauthorised */}
              {/* <Route path="/dashboard/seeker" element={<UnauthorisedPage />} /> */}
              <Route path="/*" element={<UnauthorisedPage />} />
            </>
          ): (
            <>
              <Route path="/dashboard" element={<SeekerDashboard />} />
              {/* Resume Builder Route */}

              {/* Unauthorised */}
              {/* <Route path="/dashboard/hirer" element={<UnauthorisedPage />} /> */}
              <Route path="/job/new" element={<UnauthorisedPage />} />
              <Route path="/applications" element={<UnauthorisedPage />} />
              <Route path="/*" element={<UnauthorisedPage />} />
            </>
          )}

          {/* Common to both */}
          <Route path="/job" element={<JobDescription />} />
          <Route path="/jobs" element={<ViewJobs />} />
          <Route path="/interviews" element={<InterviewSchedule />} />
          <Route path="/profile" element={<ProfilePage />} />
          <Route path="/resume" element={<ResumeBuilderPage />} />

          {/* Testing */}
          <Route path="/seekerReg" element={<SeekerSecondaryDetails />} />
          <Route path="/githubForm" element={<GithubForm />} />
          <Route path="/resume/edit" element={<ResumeEditForm />} />
          <Route path="/hirerReg" element={<HirerRegistrationForm />} />
        </>
      ) : (
        <>
          <Route path="/" element={<MetricsLandingPage />} />
          <Route path="/login" element={<LoginPage />} />
          <Route path="/signup" element={<SignupPage />} />
          <Route path="/testing" element={<Testing />} />
          <Route path="/about" element={<About />} />
          <Route path="/features" element={<Features />} />
          <Route path="/*" element={<UnauthorisedPage />} />

          {/* Testing */}
        {/* <Route path="/jobs" element={<ViewJobs />} />
        <Route path="/seekerReg" element={<SeekerSecondaryDetails />} />
        <Route path="/githubForm" element={<GithubForm />} />
        <Route path="/applications" element={<ApplicationsPage />} />
        <Route path="/interviews" element={<InterviewSchedule />} /> */}
          {/* <Route path="/hirerReg" element={<HirerRegistrationForm />} /> */}
        </>
      )}
    </Routes>
    </div>
    <Footer />
      </>
 
};

export default App
