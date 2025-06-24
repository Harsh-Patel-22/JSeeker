const ContactSection = () => {
  return (
    <div className="card border-0 shadow-sm rounded-4 p-4 mb-4">
      <h5 className="fw-bold mb-3">Contact</h5>
      <ul className="list-unstyled mb-0">
        <li className="mb-2">
          <strong>Email:</strong>{" "}
          <a href="mailto:harsh.patel@example.com" className="text-decoration-none">
            harsh.patel@example.com
          </a>
        </li>
        <li className="mb-2">
          <strong>GitHub:</strong>{" "}
          <a href="https://github.com/yourusername" target="_blank" rel="noopener noreferrer" className="text-decoration-none">
            github.com/yourusername
          </a>
        </li>
        <li className="mb-2">
          <strong>LinkedIn:</strong>{" "}
          <a href="https://linkedin.com/in/yourprofile" target="_blank" rel="noopener noreferrer" className="text-decoration-none">
            linkedin.com/in/yourprofile
          </a>
        </li>
        <li className="mb-2">
          <strong>YouTube:</strong>{" "}
          <a href="https://youtube.com/@yourchannel" target="_blank" rel="noopener noreferrer" className="text-decoration-none">
            youtube.com/@yourchannel
          </a>
        </li>
      </ul>
    </div>
  );
}

export default ContactSection;