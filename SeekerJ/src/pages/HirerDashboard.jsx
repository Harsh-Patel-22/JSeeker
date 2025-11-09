import React, { useEffect, useState } from "react";
import { Container, Row, Col, Card, Spinner, Button, Table } from "react-bootstrap";
import { useNavigate } from "react-router-dom";

const HirerDashboard = () => {
  const [metrics, setMetrics] = useState(null);
  const [interviews, setInterviews] = useState(null);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate(); 
  useEffect(() => {
    // Simulate API call delay
    setTimeout(() => {
      const dummyMetrics = {
        newApplicationsToday: 12,
        activeJobs: 5,
        totalHires: 42,
        hiringRate: 26.7,
      };

      const dummyInterviews = [
        { applicant: "Riya Sharma", date: "2025-10-29", time: "10:00 AM", mode: "Online" },
        { applicant: "Kunal Mehta", date: "2025-10-29", time: "12:30 PM", mode: "In-Person" },
        { applicant: "Ananya Patel", date: "2025-10-29", time: "3:00 PM", mode: "Online" },
        { applicant: "Priya Desai", date: "2025-10-29", time: "4:15 PM", mode: "In-Person" },
        { applicant: "Aarav Shah", date: "2025-10-29", time: "5:00 PM", mode: "Online" },
        { applicant: "Sana Khan", date: "2025-10-29", time: "6:00 PM", mode: "Online" },
        { applicant: "Devansh Patel", date: "2025-10-29", time: "7:00 PM", mode: "Online" },
        { applicant: "Mihir Joshi", date: "2025-10-29", time: "8:00 PM", mode: "In-Person" },
        { applicant: "Neha Singh", date: "2025-10-29", time: "9:00 PM", mode: "Online" },
        { applicant: "Raj Mehta", date: "2025-10-29", time: "9:30 PM", mode: "In-Person" },
      ];

      setMetrics(dummyMetrics);
      setInterviews(dummyInterviews);
      setLoading(false);
    }, 900);
  }, []);

  return (
    <div className="py-5 bg-light">
      <Container fluid="lg">
        <h2 className="fw-bold text-primary mb-4">Hirer Dashboard</h2>

        {loading ? (
          <div className="text-center py-5">
            <Spinner animation="border" variant="primary" />
            <p className="text-muted mt-3">Loading dashboard data...</p>
          </div>
        ) : (
          <Row className="g-4">
            {/* Left Column - Metrics */}
            <Col lg={8}>
              <Row className="g-4">
                {[
                  {
                    title: "New Applications Today",
                    value: metrics.newApplicationsToday,
                  },
                  {
                    title: "Active Job Posts",
                    value: metrics.activeJobs,
                  },
                  {
                    title: "Total Hires Made",
                    value: metrics.totalHires,
                  },
                  {
                    title: "Hiring Conversion Rate",
                    value: `${metrics.hiringRate}%`,
                  },
                ].map((stat, idx) => (
                  <Col key={idx} md={6}>
                    <Card className="h-100 border-0 shadow-sm rounded-4">
                      <Card.Body className="text-center py-4">
                        <h2 className="fw-bold text-dark mb-1">
                          {stat.value}
                        </h2>
                        <p className="text-muted mb-0">{stat.title}</p>
                      </Card.Body>
                    </Card>
                  </Col>
                ))}
              </Row>

              {/* Placeholder for additional analytics */}
              <Card className="border-0 shadow-sm rounded-4 mt-4">
                <Card.Body className="p-4">
                  <h5 className="fw-semibold text-dark mb-3">
                    Applications Overview
                  </h5>
                  <p className="text-muted">
                    (Analytics chart can be added here — applications trend,
                    top performing job posts, etc.)
                  </p>
                </Card.Body>
              </Card>
            </Col>

            {/* Right Column - Today's Interviews */}
            <Col lg={4}>
              <Card className="border-0 shadow-sm rounded-4 h-100 d-flex flex-column">
                <Card.Body className="p-4 d-flex flex-column">
                  <div className="d-flex justify-content-between align-items-center mb-3">
                    <h5 className="fw-semibold text-dark mb-0">
                      Today's Interviews
                    </h5>
                    <Button variant="outline-primary" size="sm" onClick={(e) => {e.currentTarget.blur(); navigate('/interviews');}}>
                      View All
                    </Button>
                  </div>

                  {/* Scrollable Table */}
                  <div
                    className="flex-grow-1 overflow-auto"
                    style={{ maxHeight: "30vh" }}
                  >
                    {interviews && interviews.length > 0 ? (
                      <Table hover responsive className="align-middle mb-0">
                        <tbody>
                          {interviews.map((interview, idx) => (
                            <tr key={idx}>
                              <td>
                                <div className="fw-semibold text-dark">
                                  {interview.applicant}
                                </div>
                                <div className="text-muted small">
                                  {interview.date} • {interview.time}
                                </div>
                              </td>
                              <td className="text-center text-muted small">
                                {interview.mode}
                              </td>
                              <td className="text-end">
                                <Button
                                  variant="link"
                                  size="sm"
                                  className="text-primary text-decoration-none"
                                >
                                  View
                                </Button>
                              </td>
                            </tr>
                          ))}
                        </tbody>
                      </Table>
                    ) : (
                      <div className="text-center py-4 text-muted">
                        No interviews scheduled for today.
                      </div>
                    )}
                  </div>
                </Card.Body>
              </Card>
            </Col>
          </Row>
        )}
      </Container>
    </div>
  );
};

export default HirerDashboard;
