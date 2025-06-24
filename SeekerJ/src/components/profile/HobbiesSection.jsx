const HobbiesSection = () => {
  const interests = ["Gaming", "Blender Modeling", "Video Editing", "Coding Competitions", "Mobile Esports"];

  return (
    <div className="card border-0 shadow-sm rounded-4 p-4 mb-4">
      <h5 className="fw-bold mb-3">Interests & Hobbies</h5>
      <div className="d-flex flex-wrap gap-2">
        {interests.map((interest, index) => (
          <span key={index} className="badge bg-warning-subtle text-dark rounded-pill px-3 py-2">
            {interest}
          </span>
        ))}
      </div>
    </div>
  );
}

export default HobbiesSection;