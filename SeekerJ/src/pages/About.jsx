import React from "react";
import { motion } from "framer-motion";

const containerVariants = {
  hidden: { opacity: 0 },
  visible: { opacity: 1, transition: { staggerChildren: 0.12 } }
};

const itemVariants = {
  hidden: { opacity: 0, y: 18 },
  visible: {
    opacity: 1,
    y: 0,
    transition: { duration: 0.5, ease: "easeOut" }
  }
};

const About = () => {
  return (
    <motion.div
      className="container py-5"
      variants={containerVariants}
      initial="hidden"
      animate="visible"
    >
      {/* Header */}
      <motion.div className="row mb-5" variants={itemVariants}>
        <div className="col text-center">
          <h1 className="fw-bold">About Us</h1>
          <p className="text-muted mt-3 fs-5">
            A local-first professional hiring platform built for real outcomes.
          </p>
        </div>
      </motion.div>

      {/* Section 1 */}
      <div className="row align-items-center mb-5">
        <motion.div className="col-md-6" variants={itemVariants}>
          <h3 className="fw-semibold mb-3">
            Built for Real Hiring, In Real Places
          </h3>
          <p className="text-muted">
            Traditional hiring platforms prioritize reach over relevance. We do
            the opposite. Our platform focuses on connecting Seekers and Hirers
            within real geographic proximity, ensuring that every opportunity
            is practical, accessible, and meaningful.
          </p>
        </motion.div>

        <motion.div className="col-md-6" variants={itemVariants}>
          <motion.div
            className="bg-light rounded-4 p-4 shadow-sm"
            whileHover={{ y: -4 }}
            transition={{ duration: 0.25 }}
          >
            <p className="mb-2 fw-medium">What this means:</p>
            <ul className="text-muted mb-0">
              <li>No irrelevant job feeds</li>
              <li>No unnecessary relocation friction</li>
              <li>Higher response and hiring success rates</li>
            </ul>
          </motion.div>
        </motion.div>
      </div>

      {/* Section 2 */}
      <motion.div className="row mb-5" variants={itemVariants}>
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
      </motion.div>

      {/* Cards */}
      <div className="row">
        {[
          {
            title: "Seamless Profiles",
            text:
              "One structured profile that represents your skills, projects, and experience — no external links required."
          },
          {
            title: "Structured Hiring",
            text:
              "Clear application limits, intelligent screening, and centralized interview management."
          },
          {
            title: "One Platform",
            text:
              "From discovery to hiring decisions — everything happens in one place."
          }
        ].map((item, idx) => (
          <motion.div
            key={idx}
            className="col-md-4 mb-4"
            variants={itemVariants}
            whileHover={{ y: -6 }}
            transition={{ duration: 0.25 }}
          >
            <div className="h-100 border rounded-4 p-4">
              <h5 className="fw-semibold">{item.title}</h5>
              <p className="text-muted mb-0">{item.text}</p>
            </div>
          </motion.div>
        ))}
      </div>
    </motion.div>
  );
};

export default About;
