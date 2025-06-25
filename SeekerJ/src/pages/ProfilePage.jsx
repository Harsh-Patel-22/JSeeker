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
import CollapsibleSection from '../components/CollapsibleSection';

const ProfilePage = () => {
    return (
    <div className="container mt-5 mb-5">
        <BasicDetailsSection />
        <ResumeSection/>

        <CollapsibleSection title={"Education"}>
          <EducationSection />
        </CollapsibleSection>
        <CollapsibleSection title={"Experience"}>
          <ExperienceSection />
        </CollapsibleSection>
        <CollapsibleSection title={"Technologies"}>
          <TechnologiesSection />
        </CollapsibleSection>
        <CollapsibleSection title={"Projects"}>
          <ProjectsSection />
        </CollapsibleSection>
        <CollapsibleSection title={"Contact"}>
          <ContactSection />
        </CollapsibleSection>
        <CollapsibleSection title={"Languages"}>
          <LanguagesSection />
        </CollapsibleSection>
        <CollapsibleSection title={"Hobbies"}>
          <HobbiesSection />
        </CollapsibleSection>

    </div>
  );
}

export default ProfilePage;