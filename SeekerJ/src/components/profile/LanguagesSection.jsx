const LanguagesSection = () => {
  const languages = [
    { name: "English", level: "Fluent" },
    { name: "Hindi", level: "Native" },
    { name: "Gujarati", level: "Native" }
  ];

  return (
    <div className="card border-0 shadow-sm rounded-4 p-4 mb-4">
      <h5 className="fw-bold mb-3">Languages</h5>
      <ul className="list-unstyled mb-0">
        {languages.map((lang, index) => (
          <li key={index}>
            <span className="fw-semibold">{lang.name}</span> —{" "}
            <span className="text-muted">{lang.level}</span>
          </li>
        ))}
      </ul>
    </div>
  );
}

export default LanguagesSection;