import { useState } from "react";
import SpinnerButton from "../ui/SpinnerButton";

// TODO - Add a showRequired bool in the fields...

export const renderField = (field, handleChange, loading, formData, readOnly, disable) => {
    switch (field.type) {
      case "select":
        return (<select
                  name={field.name}
                  className="form-control"
                  onChange={handleChange}
                  value={field.options?.[formData[field.name]]?.value}
                  disabled={readOnly || disable || loading}
                  required={field.required}
                  >
                  <option value="">Select...</option>
                  {field.options?.map((opt) => (
                    <option key={opt.value} value={opt.value}>
                      {opt.label}
                    </option>
                  ))}
                </select>
                );

      case "radio":
        return (
          <div className="space-x-6">
            {field.options?.map((opt) => (
              <label key={opt.value} className="inline-flex items-center gap-2">
                <input type="radio" value={opt.value} name={field.name} onChange={handleChange} disabled={readOnly || disable || loading}/>
                <span style={{marginRight: "10px"}}>{opt.label}</span>
              </label>
            ))}
          </div>
        );

      case "modal":
        return (
          <>
            {field.content({onSubmit: handleChange})}
          </>
        );
        
      case "date":
        return (
          <input
            name={field.name}
            type="date"
            className="form-control"
            value={formData[field.name]}
            onChange={handleChange}
            disabled={readOnly || disable || loading}
            required={field.required}
          />
        );
      case "time":
        return (
          <input
            name={field.name}
            type="time"
            className="form-control"
            value={formData[field.name]}
            onChange={handleChange}
            disabled={readOnly || disable || loading}
            required={field.required}
          />
        );

      case "textarea":
        return (
          <textarea
            name={field.name}
            className="form-control"
            value={formData[field.name]}
            onChange={handleChange}
            disabled={readOnly || disable || loading}
            required={field.required}
            rows={4}
          />
        );
      case "text":
      case "email":
      case "password":
        return (
          <input
            name={field.name}
            type={field.type}
            className="form-control"
            value={formData[field.name]}
            onChange={handleChange}
            disabled={readOnly || disable || loading}
            required={field.required}
          />
        ); 
      // default:
      //   return (
      //           <input
      //             name={field.name}
      //             type={field.type}
      //             className="form-control"
      //             value={formData[field.name]}
      //             onChange={handleChange}
      //             disabled={loading}
      //             required={field.required}
      //           />
      //         )
    }
  };

export const renderFields = (fields, errors, onChange, loading, formData, readOnly, disabled) => {
  const rendered = [];
    for (let i = 0; i < fields.length; i++) {
      const field = fields[i];

      // Two-column layout
      if (field.twoColumn && i + 1 < fields.length && fields[i + 1].twoColumn) {
        rendered.push(
          <div className="row" key={`row-${i}`}>
            {[field, fields[i + 1]].map((f, idx) => (
              <div className="col-md-6 mb-3" key={f.name}>
                <span style={{ color: "red" }}>
                  {f.required && f.showRequired && "* "}
                </span>
                <label className="form-label">{f.label}</label>
                {renderField(f, onChange, loading, formData, readOnly, disabled)}
                {errors && errors[f.name] && (
                  <div className="text-danger small">{errors[f.name]}</div>
                )}
              </div>
            ))}
          </div>
        );
        i++;
      } else {
        // Default single full-width field
        if(field.type === "section"){
        rendered.push(
          <>
            <h5 className="mt-5" style={{textAlign : "center"}}>{field.label}</h5>
            <hr />
          </>
        );
        }
        else{
          if(field.type === undefined) continue;
          if(field.type === "modal"){
            rendered.push(
              <>
                {renderField(field, onChange, loading, formData, readOnly, disabled)}
              </>
            );
          }
          else{
            // console.log(field.type);
            rendered.push(
              <div className="mb-3" key={field.name}>
                <span style={{ color: "red" }}>
                  {field.required  && field.showRequired && "* "}
                </span>
                <label className="form-label">{field.label}</label>
                
                {renderField(field, onChange, loading, formData, readOnly, disabled)}
                {errors && errors[field.name] && (
                  <div className="text-danger small">{errors[field.name]}</div>
                )}
              </div>
            );
          }
        }
      }
    }
    return rendered;
};

const BaseForm = ({
  title,
  subtitle,
  fields,
  initialData = null,
  onSubmit,
  onChange,  
  validate,
  loading,
  redirectProgress,
  showSubmit = true,
  readOnly = false,
  disabled = false,
}) => {
  
  const [formData, setFormData] = useState(() => {
    if (initialData) return initialData;

    return fields.reduce((acc, f) => {
      if (!f.name) return acc;
      acc[f.name] = f.default || "";
      return acc;
    }, {});
  });

  const [errors, setErrors] = useState({});

  const handleChange = (e) => {
    const { name, value } = e.target;

    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));

    if (onChange) {
      onChange({
        ...formData,
        [name]: value,
      });
    }
  };


  const handleSubmit = async (e) => {
    console.log("BaseForm handleSubmit called with data:", formData);
    e.preventDefault();
    const validationErrors = validate ? validate(formData) : {};
    setErrors(validationErrors);
    if (Object.keys(validationErrors).length === 0) {
      await onSubmit(formData);
    }
  };

  
  return (
    <div className="auth-form-container shadow p-4 rounded bg-white position-relative">
      {subtitle ? (
        <>
          <h3 className="text-center">{title}</h3>
          <p className="text-center mb-4">{subtitle}</p>
        </>
        ) : <h3 className="text-center mb-4">{title}</h3>}
      

      <form onSubmit={handleSubmit}>
        {renderFields(fields, errors, handleChange, loading, formData, readOnly, disabled)}
        {/* <button type="submit">Submit</button> */}
        {showSubmit && <SpinnerButton loading={loading} type="submit">Submit</SpinnerButton>}
      </form>

      {redirectProgress !== null && redirectProgress < 100 && (
        <div className="mt-3">
          <div className="progress">
            <div
              className="progress-bar progress-bar-striped progress-bar-animated bg-success"
              style={{ width: `${redirectProgress}%` }}
            >
              Redirecting...
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default BaseForm;
