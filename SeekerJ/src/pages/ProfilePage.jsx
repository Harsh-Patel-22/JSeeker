import 'bootstrap/dist/css/bootstrap.css'
import './ProfilePage.css'

import BasicDetailsSection from '../components/profile/BasicDetailsSection';
import ResumeSection from '../components/profile/ResumeSection';
import EducationSection from '../components/profile/EducationSection';
import ExperienceSection from '../components/profile/ExperienceSection';
import TechnologiesSection from './TechnologiesSection';
import ProjectsSection from '../components/profile/ProjectsSection';
import ContactSection from '../components/profile/ContactSection';
import LanguagesSection from '../components/profile/LanguagesSection';

import { useEffect, useState } from 'react';
import { userService } from '../services/apiServices';

const ProfilePage = () => {
  const [userDetails, setUserDetails] = useState(null);
  const [technologiesList, setTechnologiesList] = useState([]);

  useEffect(() => {
    async function fetchProfile() {
      const response = await userService.getProfileDetails();

      const allTechs = [];
      response.data?.projectDetails?.forEach(project => {
        project.technologiesUsages?.forEach(tech => {
          if (!allTechs.includes(tech.name)) {
            allTechs.push(tech.name);
          }
        });
      });

      setTechnologiesList(allTechs);
      setUserDetails(response.data);
    }

    fetchProfile();
  }, []);

  return (
    <div className="container mt-5 mb-5">

      {/* ===== HEADER ===== */}
      <div className="card border-0 shadow-sm rounded-4 p-4 mb-4">
        <div className="row align-items-center">

          {/* LEFT: Profile info */}
          <div className="col-md-8">
            <BasicDetailsSection details={userDetails?.basicDetails} />
          </div>

          {/* RIGHT: Resume actions */}
          <div className="col-md-4 text-md-end mt-3 mt-md-0">
            <ResumeSection />
          </div>

        </div>
      </div>


      {/* ===== TABS ===== */}
      <div className="card border-0 shadow-sm rounded-4 p-4">
        <ul className="nav nav-tabs mb-4" role="tablist">
          <li className="nav-item">
            <button className="nav-link active" data-bs-toggle="tab" data-bs-target="#education">
              Education
            </button>
          </li>
          <li className="nav-item">
            <button className="nav-link" data-bs-toggle="tab" data-bs-target="#experience">
              Experience
            </button>
          </li>
          <li className="nav-item">
            <button className="nav-link" data-bs-toggle="tab" data-bs-target="#technologies">
              Technologies
            </button>
          </li>
          <li className="nav-item">
            <button className="nav-link" data-bs-toggle="tab" data-bs-target="#projects">
              Projects
            </button>
          </li>
          <li className="nav-item">
            <button className="nav-link" data-bs-toggle="tab" data-bs-target="#contact">
              Contact
            </button>
          </li>
          <li className="nav-item">
            <button className="nav-link" data-bs-toggle="tab" data-bs-target="#languages">
              Languages
            </button>
          </li>
        </ul>

        {/* ===== TAB CONTENT ===== */}
        <div className="tab-content">

          <div className="tab-pane fade show active" id="education">
            <EducationSection details={userDetails?.educationDetails} />
          </div>

          <div className="tab-pane fade" id="experience">
            <ExperienceSection details={userDetails?.workExperienceDetails} />
          </div>

          <div className="tab-pane fade" id="technologies">
            <TechnologiesSection technologies={technologiesList} />
          </div>

          <div className="tab-pane fade" id="projects">
            <ProjectsSection details={userDetails?.projectDetails} />
          </div>

          <div className="tab-pane fade" id="contact">
            <ContactSection details={userDetails?.contactDetails} />
          </div>

          <div className="tab-pane fade" id="languages">
            <LanguagesSection details={userDetails?.vocalLanguage} />
          </div>

        </div>
      </div>
    </div>
  );
};

export default ProfilePage;
