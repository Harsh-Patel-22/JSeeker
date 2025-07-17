import { useState } from 'react';
import { api } from '../services/APIClient';
import { useAuth } from '../contexts/AuthContext';
import { useNavigate } from 'react-router-dom';
import { jwtDecode } from 'jwt-decode';

const AuthForm = ({ mode = 'login' }) => {
  const [formData, setFormData] = useState({ username: '', password: '', role: '', email: '' });
  const [status, setStatus] = useState({ show: false, message: '', success: false });
  const [progress, setProgress] = useState(0);

  const { login } = useAuth();
  const navigate = useNavigate();

  const handleChange = (e) => {
    setFormData(prev => ({ ...prev, [e.target.name]: e.target.value }));
  };

  async function handleSubmit(e) {
    e.preventDefault();
    try {
      const apiPath = mode === 'signup' ? 'auth/register' : 'auth/login';
      const response = await api.post(apiPath, formData);

      // Save token and user info
      login(response.data);
      setStatus({ show: true, message: `${mode === 'signup' ? 'Signup' : 'Login'} Successful!`, success: true });

      // Animate progress bar
      let progressValue = 0;
      const interval = setInterval(() => {
        progressValue += 20;
        setProgress(progressValue);
        if (progressValue >= 100) {
          clearInterval(interval);

          // Decode token to get role and redirect
          const decoded = jwtDecode(response.data);
          const role = decoded.role?.toLowerCase();
          // if (role === 'admin') navigate('/admin/dashboard');
          if (role === 'hirer') navigate('/dashboard/hirer');
          else if (role === 'seeker') navigate('/dashboard/seeker');
          // else navigate('/dashboard');
        }
      }, 300);
    } catch (err) {
      console.error(err);
      setStatus({ show: true, message: err.response?.data?.message || 'Authentication Failed', success: false });
    }
  }

  return (
    <div className="auth-form-container shadow p-4 rounded bg-white position-relative">
      <h3 className="text-center mb-4">{mode === 'signup' ? 'Create an Account' : 'Welcome Back'}</h3>

      <form onSubmit={handleSubmit}>
        {mode !== 'signup' ? (
          <div className="mb-3">
            <label className="form-label">Email or Username</label>
            <input name="username" type="text" className="form-control" onChange={handleChange} required />
          </div>
        ) : (
          <>
            <div className="mb-3">
              <label className="form-label">Username</label>
              <input name="username" type="text" className="form-control" onChange={handleChange} required />
            </div>
            <div className="mb-3">
              <label className="form-label">Email</label>
              <input name="email" type="email" className="form-control" onChange={handleChange} required />
            </div>
          </>
        )}

        <div className="mb-3">
          <label className="form-label">Password</label>
          <input name="password" type="password" className="form-control" onChange={handleChange} required />
        </div>

        {mode === 'signup' && (
          <div className="mb-3">
            <label className="form-label">Confirm Password</label>
            <input name="confirmPassword" type="password" className="form-control" required />
          </div>
        )}

        <div className="mb-3">
          <label className="form-label">Role</label>
          <input name="role" type="text" className="form-control" onChange={handleChange} required />
        </div>

        <button type="submit" className="btn btn-primary w-100">
          {mode === 'signup' ? 'Sign Up' : 'Log In'}
        </button>
      </form>

      {/* Toast Message */}
      {status.show && (
        <div
          className={`toast align-items-center text-white ${status.success ? 'bg-success' : 'bg-danger'} position-absolute top-0 end-0 m-3 show`}
          role="alert"
        >
          <div className="d-flex">
            <div className="toast-body">
              {status.message}
            </div>
            <button
              type="button"
              className="btn-close btn-close-white me-2 m-auto"
              aria-label="Close"
              onClick={() => setStatus({ ...status, show: false })}
            ></button>
          </div>
        </div>
      )}

      {/* Progress Bar */}
      {status.success && progress < 100 && (
        <div className="mt-3">
          <div className="progress">
            <div
              className="progress-bar progress-bar-striped progress-bar-animated"
              role="progressbar"
              style={{ width: `${progress}%` }}
              aria-valuenow={progress}
              aria-valuemin="0"
              aria-valuemax="100"
            >
              Redirecting...
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default AuthForm;
