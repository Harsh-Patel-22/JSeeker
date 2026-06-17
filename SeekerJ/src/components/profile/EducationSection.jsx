import 'bootstrap/dist/css/bootstrap.css'

const EducationSection = ({details}) => {
  return (
    <div className="card border-0 shadow-sm rounded-4 p-4 mb-4">
      <h5 className="fw-bold mb-3">Education</h5>

      {details && details.map((edu, index) => {
        return <div className="mb-3">
          <p className="mb-1 fw-semibold">{edu.study.toUpperCase()}</p>
          <p className="small mb-1">{edu.instituteName}, {edu.state}</p>
          <p className="small mb-0">{edu.startDate.substring(0, 4)} - {edu.endDate.substring(0, 4)}</p>
        </div>
      })}
    </div>
  ) 
}

export default EducationSection; 