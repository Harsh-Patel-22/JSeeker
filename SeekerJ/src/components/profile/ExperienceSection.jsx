import 'bootstrap/dist/css/bootstrap.css'

const ExperienceSection = () => {
  return (
    <div className="card border-0 shadow-sm rounded-4 p-4 mb-4">
      <h5 className="fw-bold mb-3">Experience</h5>
      <div className="mb-3">
        <p className="mb-1 fw-semibold">Freelance Developer</p>
        <p className="text-muted small mb-1">Self-employed • Jan 2024 – Present</p>
        <p className="text-muted small">
          Worked on micro-job platforms, resume tools, and interactive web experiences using ASP.NET Core and React.
        </p>
      </div>
      <div>
        <p className="mb-1 fw-semibold">YouTube Gaming Creator</p>
        <p className="text-muted small mb-1">Mobile Legends Channel • Apr 2022 – Present</p>
        <p className="text-muted small">
          Edited high-quality gameplays and built a growing audience by storytelling through thumbnails and titles.
        </p>
      </div>
    </div>
  );
}

export default ExperienceSection;