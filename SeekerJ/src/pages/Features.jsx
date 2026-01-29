import React from "react";
import { motion } from "framer-motion";

const containerVariants = {
  hidden: { opacity: 0 },
  visible: { opacity: 1, transition: { staggerChildren: 0.1 } }
};

const cardVariants = {
  hidden: { opacity: 0, y: 16 },
  visible: {
    opacity: 1,
    y: 0,
    transition: { duration: 0.45, ease: "easeOut" }
  }
};

const Features = () => {
  return (
    <motion.div
      className="container py-5"
      variants={containerVariants}
      initial="hidden"
      animate="visible"
    >
      {/* Header */}
      <motion.div className="row mb-5" variants={cardVariants}>
        <div className="col text-center">
          <h1 className="fw-bold">Platform Features</h1>
          <p className="text-muted fs-5 mt-3">
            Everything you need for efficient, local, and skill-driven hiring.
          </p>
        </div>
      </motion.div>

      {/* Feature Grid */}
      <div className="row g-4">
        {[
          {
            title: "Locality-First Job Discovery",
            description:
              "Jobs are prioritized based on search distance, ensuring relevance and accessibility."
          },
          {
            title: "Dual Role System",
            description:
              "Clear separation between Seekers and Hirers for a focused and distraction-free experience."
          },
          {
            title: "AI-Powered Application Rating",
            description:
              "Every application is analyzed against job requirements, skills, and real project data."
          },
          {
            title: "GitHub Project Intelligence",
            description:
              "Automatically fetch project descriptions, technologies, and contribution depth using GitHub APIs."
          },
          {
            title: "Native Auto-Generated Resume",
            description:
              "A standardized, editable resume created at registration and shared directly with Hirers."
          },
          {
            title: "Smart Interview Scheduling",
            description:
              "Built-in scheduling with rescheduling support until both parties agree on a time."
          },
          {
            title: "Outcome Tracking",
            description:
              "Track interview results with structured outcomes to improve platform-level metrics."
          },
          {
            title: "Professional & Distraction-Free UI",
            description:
              "No social noise, no vanity metrics — only tools that support decision-making."
          }
        ].map((feature, idx) => (
          <motion.div
            key={idx}
            className="col-md-6 col-lg-4"
            variants={cardVariants}
            whileHover={{ y: -6 }}
            transition={{ duration: 0.25 }}
          >
            <div className="h-100 border rounded-4 p-4 shadow-sm">
              <h5 className="fw-semibold mb-3">{feature.title}</h5>
              <p className="text-muted mb-0">{feature.description}</p>
            </div>
          </motion.div>
        ))}
      </div>
    </motion.div>
  );
};

export default Features;
