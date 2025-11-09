import 'bootstrap/dist/css/bootstrap.css'

const ExperienceSection = ({details}) => {
  return (
    <div className="card border-0 shadow-sm rounded-4 p-4 mb-4">
      <h5 className="fw-bold mb-3">Experience</h5>
      {details && details.map((we, index) => {
        return <div className="mb-3">
          <p className="mb-1 fw-semibold">{we.role}</p>
          <p className="text-muted small mb-1">{we.companyName} • {we.startDate} – {we.endDate}</p>
          <p className="text-muted small">{we.description}</p>
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
  );
}

export default ExperienceSection;