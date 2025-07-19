import { Spinner } from 'react-bootstrap';

const SpinnerButton = ({ loading, children, ...props }) => (
  <button
    className="btn btn-primary w-100 d-flex align-items-center justify-content-center"
    disabled={loading}
    {...props}
  >
    {loading && <Spinner animation="border" size="sm" className="me-2" />}
    {loading ? 'Please wait...' : children}
  </button>
);

export default SpinnerButton;
