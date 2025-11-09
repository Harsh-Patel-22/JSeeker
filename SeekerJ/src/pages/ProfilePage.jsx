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
import { useEffect, useState } from 'react';
import { userService } from '../services/apiServices';

const ProfilePage = () => {
    const [userDetails, setUserDetails] = useState(null); 
    const [technologiesList, setTechnologiesList] = useState([]);

    useEffect(() => {
        async function fetchProfile() {
          let response = await userService.getProfileDetails();
          console.log("Profile details fetched:", response.data);

          const allTechs = [];
          response.data?.projectDetails.map((project) => {
            project.technologiesUsages.map((tech) => {
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
        <BasicDetailsSection details = {userDetails?.basicDetails}/>
        <ResumeSection/>

        <CollapsibleSection title={"Education"}>
          <EducationSection details={userDetails?.educationDetails} />
        </CollapsibleSection>
        <CollapsibleSection title={"Experience"}>
          <ExperienceSection details={userDetails?.workExperienceDetails} />
        </CollapsibleSection>
        
        {console.log(technologiesList)}
        <CollapsibleSection title={"Technologies"}>
          <TechnologiesSection technologies={technologiesList} />
        </CollapsibleSection>
        <CollapsibleSection title={"Projects"}>
          {console.log("Projects Details:", userDetails?.projectDetails)}
          <ProjectsSection details={userDetails?.projectDetails} />
        </CollapsibleSection>
        <CollapsibleSection title={"Contact"}>
          <ContactSection details={userDetails?.contactDetails}/>
        </CollapsibleSection>
        <CollapsibleSection title={"Languages"}>
          <LanguagesSection details={userDetails?.vocalLanguage} />
        </CollapsibleSection>

    </div>
  );
}

export default ProfilePage;