import 'bootstrap/dist/css/bootstrap.css'

const BasicDetailsSection = () => {
  return (
    <div className="card border-0 shadow-sm rounded-4 p-4 mb-4">
      <div className="d-flex flex-column flex-md-row align-items-center gap-4">
        <img
          src="https://via.placeholder.com/100"
          alt="Profile"
          className="rounded-circle"
          width="100"
          height="100"
        />
        <div>
          <h4 className="fw-bold mb-0">Harsh Patel</h4>
          <p className="text-muted mb-1">B.Tech CSE Student • Gujarat, India</p>
          <p className="text-muted small">
            Focused on Full Stack Development | Game Dev | YouTuber (About/Description)
          </p>
        </div>
      </div>
    </div>
  );
}

export default BasicDetailsSection;