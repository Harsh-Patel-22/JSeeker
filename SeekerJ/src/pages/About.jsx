const About = () => {
  return (
    <div className="container py-5">
      {/* Header */}
      <div className="row mb-5">
        <div className="col text-center">
          <h1 className="fw-bold">About Us</h1>
          <p className="text-muted mt-3 fs-5">
            A local-first professional hiring platform built for real outcomes.
          </p>
        </div>
      </div>

      {/* Section 1 */}
      <div className="row align-items-center mb-5">
        <div className="col-md-6">
          <h3 className="fw-semibold mb-3">
            Built for Real Hiring, In Real Places
          </h3>
          <p className="text-muted">
            Traditional hiring platforms prioritize reach over relevance. We do
            the opposite. Our platform focuses on connecting Seekers and Hirers
            within real geographic proximity, ensuring that every opportunity
            is practical, accessible, and meaningful.
          </p>
        </div>
        <div className="col-md-6">
          <div className="bg-light rounded-4 p-4 shadow-sm">
            <p className="mb-2 fw-medium">What this means:</p>
            <ul className="text-muted mb-0">
              <li>No irrelevant job feeds</li>
              <li>No unnecessary relocation friction</li>
              <li>Higher response and hiring success rates</li>
            </ul>
          </div>
        </div>
      </div>

      {/* Section 2 */}
      <div className="row mb-5">
        <div className="col">
          <h3 className="fw-semibold mb-3">
            Skills, Projects, and Proof — Not Presentation
          </h3>
          <p className="text-muted">
            We believe hiring should be based on what you can do, not how well
            you can design a resume. By integrating directly with GitHub’s
            fine-grained APIs, we automatically fetch real project data,
            technologies used, and contribution depth.
          </p>
        </div>
      </div>

      {/* Section 3 */}
      <div className="row">
        <div className="col-md-4 mb-4">
          <div className="h-100 border rounded-4 p-4">
            <h5 className="fw-semibold">Seamless Profiles</h5>
            <p className="text-muted mb-0">
              One structured profile that represents your skills, projects, and
              experience — no external links required.
            </p>
          </div>
        </div>

        <div className="col-md-4 mb-4">
          <div className="h-100 border rounded-4 p-4">
            <h5 className="fw-semibold">Structured Hiring</h5>
            <p className="text-muted mb-0">
              Clear application limits, intelligent screening, and centralized
              interview management.
            </p>
          </div>
        </div>

        <div className="col-md-4 mb-4">
          <div className="h-100 border rounded-4 p-4">
            <h5 className="fw-semibold">One Platform</h5>
            <p className="text-muted mb-0">
              From discovery to hiring decisions — everything happens in one
              place.
            </p>
          </div>
        </div>
      </div>
    </div>
  );
};

export default About;
