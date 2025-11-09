import { useState } from "react";
import {renderFields} from "./BaseForm";
import SpinnerButton from "../ui/SpinnerButton";

const BaseModal = ({
  title,
  fields,
  handleSubmit,
  validate,
  loading = false,
  redirectProgress = null,
  triggerText = "Open Modal",
  size = "lg", // sm, md, lg, xl,
  hasMultipleEntries = false
}) => {
  const [show, setShow] = useState(false);

  const handleClose = () => setShow(false);
  const handleShow = () => setShow(true);

  const [entries, setEntries] = useState([
    fields.reduce((acc, f) => ({ ...acc, [f.name]: f.default || "" }), {})
  ]);
  const [expandedIndex, setExpandedIndex] = useState(0);

  const [errors, setErrors] = useState([]);

  const handleChange = (e, idx) => {
    const { name, value } = e.target; 
    setEntries((prev) =>
      prev.map((entry, i) => (i === idx ? { ...entry, [name]: value } : entry))
    );
  };

  const handleFinalSubmit = () => {
    const validationErrors = validate ? validate(entries) : {};
  if (Object.keys(validationErrors).length > 0) {
    setErrors(validationErrors);
    return;
  }

  handleSubmit(entries);
  setShow(false); 
};


  // const handleSubmit = (data) => {
  //   handleSubmit(data);
  //   setShow(false); // auto-close after submit
  // };

  return (
    <>
      {/* Trigger Button */}
      <button type="button" className="btn btn-primary" onClick={handleShow} style={{display: 'block', margin: '0px auto 0px auto'}}>
        {triggerText}
      </button>

      {/* Bootstrap Modal */}
      <div
        className={`modal fade ${show ? "show d-block" : ""}`}
        tabIndex="-1"
        style={{ backgroundColor: show ? "rgba(0,0,0,0.5)" : "transparent" }}
      >
        <div className={`modal-dialog modal-${size}`}>
          <div className="modal-content">
            <div className="modal-header">
              <h5 className="modal-title">{title}</h5>
              <button type="button" className="btn-close" onClick={handleClose}></button>
            </div>

            <div className="modal-body">
              {hasMultipleEntries ? (
                <>
                  <div className="accordion" id="multiEntryForm">
                  {entries.map((entry, idx) => (
                    <div className="accordion-item" key={idx}>
                      <h2 className="accordion-header">
                        <button
                          className={`accordion-button ${expandedIndex === idx ? "" : "collapsed"}`}
                          type="button"
                          onClick={() => setExpandedIndex(expandedIndex === idx ? -1 : idx)}
                          >
                          {/* Entry {idx + 1} */}
                          {entry[fields[0].name] || `${fields[0].label} ${idx + 1}`}
                        </button>
                      </h2>
                      <div className={`accordion-collapse collapse ${expandedIndex === idx ? "show" : ""}`}>
                        <div className="accordion-body">
                          {renderFields(fields, errors[idx], (e) => handleChange(e, idx), loading, entry)}
                        </div>
                      </div>
                    </div>
                  ))}
                </div>

                <button
                type="button"
                className="btn btn-outline-primary mt-3 mb-3"
                onClick={() => {
                  setEntries([
                    ...entries,
                    fields.reduce((acc, f) => ({ ...acc, [f.name]: f.default || "" }), {})
                  ]);
                  setExpandedIndex(entries.length); // open the new one
                }}
                >
                + Add Another
                </button>

                <button
                type="button"
                className="btn btn-sm btn-outline-danger ms-2"
                onClick={() => {
                  if(entries.length <= 1) return;
                  setEntries((prev) => prev.filter((_, i) => i !== expandedIndex));
                  setExpandedIndex(entries.length);
                }}
                >
                Delete
                </button>
              </>
                ) : (
                  <>
                  {renderFields(fields, errors[0], (e) => handleChange(e, 0), loading, entries[0])}
                  </>
                  )
              }

              
            <SpinnerButton loading={loading} handleClick={handleFinalSubmit}>Submit</SpinnerButton>

            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export const BaseModalWithoutButton = ({
  title,
  fields,
  handleSubmit,
  validate,
  show,
  setShow,
  loading = false,
  redirectProgress = null,
  size = "lg", // sm, md, lg, xl,
  hasMultipleEntries = false
}) => {
  // const [show, setShow] = useState(true);

  const handleClose = () => setShow(false);
  // const handleShow = () => setShow(true);

  const [entries, setEntries] = useState([
    fields.reduce((acc, f) => ({ ...acc, [f.name]: f.default || "" }), {})
  ]);
  const [expandedIndex, setExpandedIndex] = useState(0);

  const [errors, setErrors] = useState([]);

  const handleChange = (e, idx) => {
    const { name, value } = e.target; 
    setEntries((prev) =>
      prev.map((entry, i) => (i === idx ? { ...entry, [name]: value } : entry))
    );
  };

  const handleFinalSubmit = () => {
    const validationErrors = validate ? validate(entries) : {};
  if (Object.keys(validationErrors).length > 0) {
    setErrors(validationErrors);
    return;
  }

  handleSubmit(entries);
  // setShow(false); 
};


  // const handleSubmit = (data) => {
  //   handleSubmit(data);
  //   setShow(false); // auto-close after submit
  // };

  return (
    <>
      {/* Trigger Button */}
      {/* <button type="button" className="btn btn-primary" onClick={handleShow} style={{display: 'block', margin: '0px auto 0px auto'}}>
        {triggerText}
      </button> */}

      {/* Bootstrap Modal */}
      <div
        className={`modal fade ${show ? "show d-block" : ""}`}
        tabIndex="-1"
        style={{ backgroundColor: show ? "rgba(0,0,0,0.5)" : "transparent" }}
      >
        <div className={`modal-dialog modal-${size}`}>
          <div className="modal-content">
            <div className="modal-header">
              <h5 className="modal-title">{title}</h5>
              <button type="button" className="btn-close" onClick={handleClose}></button>
            </div>

            <div className="modal-body">
              {hasMultipleEntries ? (
                <>
                  <div className="accordion" id="multiEntryForm">
                  {entries.map((entry, idx) => (
                    <div className="accordion-item" key={idx}>
                      <h2 className="accordion-header">
                        <button
                          className={`accordion-button ${expandedIndex === idx ? "" : "collapsed"}`}
                          type="button"
                          onClick={() => setExpandedIndex(expandedIndex === idx ? -1 : idx)}
                          >
                          {/* Entry {idx + 1} */}
                          {entry[fields[0].name] || `${fields[0].label} ${idx + 1}`}
                        </button>
                      </h2>
                      <div className={`accordion-collapse collapse ${expandedIndex === idx ? "show" : ""}`}>
                        <div className="accordion-body">
                          {renderFields(fields, errors[idx], (e) => handleChange(e, idx), loading, entry)}
                        </div>
                      </div>
                    </div>
                  ))}
                </div>

                <button
                type="button"
                className="btn btn-outline-primary mt-3 mb-3"
                onClick={() => {
                  setEntries([
                    ...entries,
                    fields.reduce((acc, f) => ({ ...acc, [f.name]: f.default || "" }), {})
                  ]);
                  setExpandedIndex(entries.length); // open the new one
                }}
                >
                + Add Another
                </button>

                <button
                type="button"
                className="btn btn-sm btn-outline-danger ms-2"
                onClick={() => {
                  if(entries.length <= 1) return;
                  setEntries((prev) => prev.filter((_, i) => i !== expandedIndex));
                  setExpandedIndex(entries.length);
                }}
                >
                Delete
                </button>
              </>
                ) : (
                  <>
                  {renderFields(fields, errors[0], (e) => handleChange(e, 0), loading, entries[0])}
                  </>
                  )
              }

              
            <SpinnerButton loading={loading} handleClick={handleFinalSubmit}>Submit</SpinnerButton>

            </div>
          </div>
        </div>
      </div>
    </>
  );
};



export default BaseModal;
