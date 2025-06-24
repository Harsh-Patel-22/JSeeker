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
import HobbiesSection from '../components/profile/HobbiesSection';

const ProfilePage = () => {
    return (
    <div className="container mt-5 mb-5">
        <BasicDetailsSection />
        <ResumeSection/>
        <EducationSection />
        <ExperienceSection />
        <TechnologiesSection />
        <ProjectsSection />
        <ContactSection />
        <LanguagesSection />
        <HobbiesSection />
    </div>
  );
}

export default ProfilePage;