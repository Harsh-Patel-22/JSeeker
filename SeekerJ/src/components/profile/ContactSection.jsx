import { useEffect, useState } from "react";

const ContactSection = ({ details }) => {
  const [githubUrl, setGithubUrl] = useState("https://www.github.com/");
  useEffect(() => {
    if (details?.githubProfileLink) {
      setGithubUrl(`https://www.github.com/${details.githubProfileLink}`);
    }
  }, [details]);
  return (
    <div className="card border-0 shadow-sm rounded-4 p-4 mb-4">
      <h5 className="fw-bold mb-3">Contact</h5>
      <ul className="list-unstyled mb-0">
        <li className="mb-2">
          <strong>Email:</strong>{" "}
          <a
            href={`mailto:${details?.email}`}
            className="text-decoration-none"
          >
            {details?.email}
          </a>
        </li>

        <li className="mb-2">
          <strong>GitHub:</strong>{" "}
          <a
            href={githubUrl}
            target="_blank"
            rel="noopener noreferrer"
            className="text-decoration-none"
          >
            {githubUrl}
          </a>
        </li>

        <li className="mb-2">
          <strong>LinkedIn:</strong>{" "}
          <a
            href={details?.linkedInProfileLink}
            target="_blank"
            rel="noopener noreferrer"
            className="text-decoration-none"
          >
            {details?.linkedInProfileLink}
          </a>
        </li>

        <li className="mb-2">
          <strong>Phone:</strong>{" "}
          <a
            href={`tel:${details?.phoneNumber}`}
            className="text-decoration-none"
          >
            {details?.phoneNumber}
          </a>
        </li>
      </ul>
    </div>
  );
};

export default ContactSection;
