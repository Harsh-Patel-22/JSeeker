import 'bootstrap/dist/css/bootstrap.css'
const TechnologiesSection = () => {
  const technologies = [
    "React", "ASP.NET Core", "Unity", "Shader Graph",
    "C#", "Blender", "Socket Programming", "MongoDB"
  ];

  return (
    <div className="card border-0 shadow-sm rounded-4 p-4 mb-4">
      <h5 className="fw-bold mb-3">Technologies & Interests</h5>
      <div className="d-flex flex-wrap gap-2">
        {technologies.map((tech, index) => (
          <span key={index} className="badge bg-primary-subtle text-primary fw-semibold rounded-pill px-3 py-2">
            {tech}
          </span>
        ))}
      </div>
    </div>
  );
}

export default TechnologiesSection;