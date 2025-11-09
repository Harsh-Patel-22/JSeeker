import React, { useEffect, useState } from "react";
import { Container, Row, Col, Card, Spinner } from "react-bootstrap";
import {
  PeopleFill,
  BriefcaseFill,
  CalendarCheckFill,
  Bullseye,
  GraphUpArrow,
} from "react-bootstrap-icons";
import { miscApiService } from "../services/apiServices";

const MetricsLandingPage = () => {
  const [metrics, setMetrics] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    async function fetchMetrics() {
      try {
        const response = await miscApiService.getMetrics();
        console.log("Metrics fetched:", response.data);
        setMetrics(response.data);
      } catch (error) {
        console.error("Error fetching metrics:", error);
      } finally {
        setLoading(false);
      }
    }

    fetchMetrics();
  }, []);

  const stats = [
    {
      title: "Total Users",
      value: metrics?.totalUsers?.toLocaleString() || 0,
      icon: <PeopleFill size={32} />,
      color: "#0d6efd",
    },
    {
      title: "Total Jobs Posted",
      value: metrics?.totalJobsPosted?.toLocaleString() || 0,
      icon: <BriefcaseFill size={32} />,
      color: "#6610f2",
    },
    {
      title: "Average Jobs Posted Daily",
      value: metrics?.averageJobsPostedDaily?.toFixed(2) || 0,
      icon: <CalendarCheckFill size={32} />,
      color: "#198754",
    },
    {
      title: "Successful Job Landings",
      value: metrics?.numberOfSuccessfulJobLandings?.toLocaleString() || 0,
      icon: <Bullseye size={32} />,
      color: "#fd7e14",
    },
    {
      title: "Job Landing Success Rate",
      value: `${metrics?.jobLandingSuccessRate?.toFixed(2) || 0}%`,
      icon: <GraphUpArrow size={32} />,
      color: "#20c997",
    },
  ];

  return (
    <div className="py-5 bg-light">
      <Container>
        <div className="text-center mb-5">
          <h2 className="fw-bold display-6 text-primary mb-2">
            Our Platform in Numbers
          </h2>
          <p className="text-muted fs-5">
            A quick look at the growth and success driven by our community
          </p>
        </div>

        {loading ? (
          <div className="text-center py-5">
            <Spinner animation="border" variant="primary" className="mb-3" />
            <p className="text-muted fs-5">Fetching metrics...</p>
          </div>
        ) : (
          <Row className="g-4">
            {stats.map((stat, idx) => (
              <Col key={idx} md={6} lg={4} xl={3}>
                <Card
                  className="h-100 text-center border-0 shadow-sm rounded-4 p-3 metric-card"
                  style={{
                    transition: "transform 0.3s ease, box-shadow 0.3s ease",
                  }}
                  onMouseEnter={(e) => {
                    e.currentTarget.style.transform = "translateY(-6px)";
                    e.currentTarget.style.boxShadow =
                      "0 8px 24px rgba(0, 0, 0, 0.1)";
                  }}
                  onMouseLeave={(e) => {
                    e.currentTarget.style.transform = "translateY(0)";
                    e.currentTarget.style.boxShadow =
                      "0 4px 12px rgba(0, 0, 0, 0.05)";
                  }}
                >
                  <Card.Body>
                    <div
                      className="mb-3 d-inline-flex align-items-center justify-content-center rounded-circle p-3"
                      style={{
                        backgroundColor: `${stat.color}15`,
                        color: stat.color,
                      }}
                    >
                      {stat.icon}
                    </div>
                    <h3 className="fw-bold text-dark">{stat.value}</h3>
                    <h6 className="text-muted">{stat.title}</h6>
                  </Card.Body>
                </Card>
              </Col>
            ))}
          </Row>
        )}
      </Container>
    </div>
  );
};

export default MetricsLandingPage;
