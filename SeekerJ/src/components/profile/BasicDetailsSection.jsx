import 'bootstrap/dist/css/bootstrap.css'

const BasicDetailsSection = ({details}) => {
  return (
    <div className="card border-0 shadow-sm rounded-4 p-4 mb-4">
      <div className="d-flex flex-column flex-md-row align-items-center gap-4">
        <img
          src="https://www.pngmart.com/files/23/Profile-PNG-Photo.png"
          alt="Profile"
          className="rounded-circle"
          width="100"
          height="100"
        />
        <div>
          <h4 className="fw-bold mb-3 mt-3">{details?.firstName} {details?.lastName}</h4>
          <p className="text-muted mb-0">{details?.aboutLine} </p>
          <p className="text-muted small">{details?.companyAddress != undefined ? `${details?.companyAddress?.state}, ${details?.companyAddress?.country}` : `${details?.state}, ${details?.country}`}</p>
          {/* <p className="text-muted small">
            Focused on Full Stack Development | Game Dev | YouTuber (About/Description)
          </p> */}
        </div>
      </div>
    </div>
  );
}

export default BasicDetailsSection;