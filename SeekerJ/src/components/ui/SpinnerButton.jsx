import { Spinner } from 'react-bootstrap';

const SpinnerButton = ({ loading, handleClick, children, type = "button", className = "btn btn-primary mt-3 w-100 d-flex align-items-center justify-content-center", ...props }) => (
  <button
    onClick={handleClick}
    type={type}
    className={className}
    disabled={loading}
    {...props}
  >
    {loading && <Spinner animation="border" size="sm" className="me-2" />}
    {loading ? 'Please wait...' : children}
  </button>
);

export default SpinnerButton;
