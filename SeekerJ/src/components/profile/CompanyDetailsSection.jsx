import { useEffect, useState } from "react";

const CompanyDetailsSection = ({ details }) => {
  useEffect(() => {
    console.log("Company Details:", details);
  }, [details]);
  return (
    <div className="card border-0 shadow-sm rounded-4 p-4 mb-4">
      <ul className="list-unstyled mb-0">
        <li className="mb-2">
          <strong>Company:</strong> {details?.companyName}
        </li>
        <li className="mb-2">
          <strong>Designation:</strong> {details?.designation}
        </li>

        <li className="mb-2">
          <strong>Website:</strong>{" "}
          <a
            href={`${details?.websiteLink}`}
            className="text-decoration-none"
          >
            {details?.websiteLink}
          </a>
        </li>
      </ul>
    </div>
  );
};

export default CompanyDetailsSection;
