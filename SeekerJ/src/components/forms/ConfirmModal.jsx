import { Modal, Button, Spinner, Fade } from "react-bootstrap";

const ConfirmModal = ({ show, onConfirm, onCancel, message, loading }) => {
  return (
    <Modal
      show={show}
      onHide={onCancel}
      centered
      backdrop="static"
      animation
    >
      <Fade in={show}>
        <div>
          <Modal.Header closeButton>
            <Modal.Title>Are you sure?</Modal.Title>
          </Modal.Header>
          <Modal.Body>
            <p className="mb-0">{message || "Do you want to continue this action?"}</p>
          </Modal.Body>
          <Modal.Footer>
            <Button variant="secondary" onClick={onCancel} disabled={loading}>
              Cancel
            </Button>
            <Button variant="success" onClick={onConfirm} disabled={loading}>
              {loading ? (
                <>
                  <Spinner
                    as="span"
                    animation="border"
                    size="sm"
                    role="status"
                    aria-hidden="true"
                    className="me-2"
                  />
                  Processing...
                </>
              ) : (
                "Sure"
              )}
            </Button>
          </Modal.Footer>
        </div>
      </Fade>
    </Modal>
  );
};

export default ConfirmModal;
