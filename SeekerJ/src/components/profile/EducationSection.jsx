import 'bootstrap/dist/css/bootstrap.css'

const EducationSection = () => {
  return ( // TODO - Add a .map -> have multiple eduation, school, college, etc.
    <div className="card border-0 shadow-sm rounded-4 p-4 mb-4">
      <h5 className="fw-bold mb-3">Education</h5>
      <p className="mb-1 fw-semibold">B.Tech in Computer Science and Engineering</p>
      <p className="text-muted small mb-0">XYZ University, Gujarat — 2022 to 2026</p>
    </div>
  );
}

export default EducationSection; 