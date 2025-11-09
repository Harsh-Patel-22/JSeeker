import 'bootstrap/dist/css/bootstrap.css'

const EducationSection = ({details}) => {
  return (
    <div className="card border-0 shadow-sm rounded-4 p-4 mb-4">
      <h5 className="fw-bold mb-3">Education</h5>

      {details && details.map((edu, index) => {
        return <div className="mb-3">
          {/* <h5 className="fw-bold mb-3">{edu.study}</h5> */}
          <p className="mb-1 fw-semibold">{edu.study}</p>
          <p className="small mb-1">{edu.instituteName}, {edu.state}</p>
          <p className="small mb-0">{edu.startDate.substring(0, 4)} - {edu.endDate.substring(0, 4)}</p>
        </div>
      })}
      {/* <div className="mb-3">
        <p className="mb-1 fw-semibold">YouTube Gaming Creator</p>
        <p className="text-muted small mb-1">Mobile Legends Channel • Apr 2022 – Present</p>
        <p className="text-muted small">
          Edited high-quality gameplays and built a growing audience by storytelling through thumbnails and titles.
        </p>
      </div> */}
    </div>
  )
    {details && details.map((edu, index) => {
      <div className="card border-0 shadow-sm rounded-4 p-4 mb-4">
        <h5 className="fw-bold mb-3">{edu.study}</h5>
        <p className="mb-1 fw-semibold">B.Tech in Computer Science and Engineering</p>
        <p className="text-muted small mb-0">XYZ University, Gujarat — 2022 to 2026</p>
      </div>
    }    
  )}
}

export default EducationSection; 