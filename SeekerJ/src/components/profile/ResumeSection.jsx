import 'bootstrap/dist/css/bootstrap.css'

const ResumeSection = () => {
  return (
    <div className="card border-0 shadow-sm rounded-4 p-4 mb-4">
      <div className="d-flex justify-content-between align-items-center">
        <h5 className="fw-bold mb-0">Resume</h5>
        <div className="d-flex gap-2">
          <a href="/resume.pdf" className="btn btn-outline-primary btn-sm" target="_blank" rel="noopener noreferrer">
            View Resume
          </a>
          <button className="btn btn-primary btn-sm">Create Resume</button>
        </div>
      </div>
    </div>
  );
}

export default ResumeSection;
