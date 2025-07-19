// contexts/ToastContext.jsx
import { createContext, useContext, useState } from 'react';

const ToastContext = createContext();

export const ToastProvider = ({ children }) => {
  const [toast, setToast] = useState({ show: false, message: '', success: true });

  const showToast = (message, success = true) => {
    setToast({ show: true, message, success });
    setTimeout(() => setToast({ ...toast, show: false }), 4000);
  };

  return (
    <ToastContext.Provider value={{ toast, showToast }}>
      {children}
      {toast.show && (
        <div
          className={`toast text-white position-fixed top-0 end-0 m-3 show ${toast.success ? 'bg-success' : 'bg-danger'}`}
          role="alert"
        >
          <div className="d-flex">
            <div className="toast-body">{toast.message}</div>
            <button
              className="btn-close btn-close-white me-2 m-auto"
              onClick={() => setToast({ ...toast, show: false })}
            />
          </div>
        </div>
      )}
    </ToastContext.Provider>
  );
};

export const useToast = () => useContext(ToastContext);
