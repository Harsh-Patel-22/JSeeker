import { useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'
import { BrowserRouter, Routes, Route } from 'react-router';
import LoginPage from './pages/LoginPage';
import HirerDashboard from './pages/HirerDashboard';
import SignupPage from './pages/SignupPage';
import Navbar from './components/Navbar';
import SeekerDashboard from './pages/SeekerDashboard';
import JobDescription from './pages/JobDescription';
function App() {

  return (
    <>
      <BrowserRouter> 
        <Routes>
          <Route path="/" element={<LoginPage/>} />
          <Route path="/dashboard/hirer" element={<HirerDashboard/>} />
          <Route path="/dashboard/seeker" element={<SeekerDashboard/>} />
          <Route path='/job' element={<JobDescription></JobDescription>} />
          <Route path='/signup' element={<SignupPage/>} />
        </Routes>
      </BrowserRouter>
    </>
  )
}

export default App
