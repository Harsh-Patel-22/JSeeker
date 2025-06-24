const ProjectsSection = () => {
  const projects = [
    {
      name: "Micro-Jobs Platform",
      tech: ["ASP.NET Core", "React", "JWT", "SQL Server"],
      timeframe: "Feb 2025 – Apr 2025",
      github: "https://github.com/yourrepo/microjobs"
    },
    {
      name: "File Sharing App",
      tech: ["Python", "Tkinter", "Socket Programming"],
      timeframe: "Mar 2025",
      github: "https://github.com/yourrepo/fileshare"
    },
    {
      name: "University Management System",
      tech: ["HTML", "CSS", "JavaScript", "Java Servlets"],
      timeframe: "Jan 2025 – Feb 2025",
      github: "https://github.com/yourrepo/ums"
    }
  ];

  return (
    <div className="card border-0 shadow-sm rounded-4 p-4 mb-4">
      <h5 className="fw-bold mb-3">Projects</h5>
      <div className="row g-4">
        {projects.map((project, idx) => (
          <div className="col-md-6" key={idx}>
            <div className="card h-100 border-0 shadow-sm rounded-3 p-3">
              <h6 className="fw-semibold mb-1">{project.name}</h6>
              <div className="d-flex flex-wrap gap-2 mb-2">
                {project.tech.map((t, i) => (
                  <span key={i} className="badge bg-success-subtle text-success fw-medium rounded-pill px-2 py-1">
                    {t}
                  </span>
                ))}
              </div>
              <p className="text-muted small mb-2">{project.timeframe}</p>
              <a href={project.github} target="_blank" rel="noopener noreferrer" className="btn btn-outline-dark btn-sm">
                View on GitHub
              </a>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}

export default ProjectsSection;