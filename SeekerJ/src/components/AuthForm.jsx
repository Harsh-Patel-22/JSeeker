import { useState } from 'react';
import { jwtDecode } from 'jwt-decode';
import { useNavigate } from 'react-router-dom';
import SpinnerButton from './ui/SprinnerButton';
import RoleSelector from './forms/RoleSelector';
import { useAuth } from '../contexts/AuthContext';
import { useToast } from '../contexts/ToastContext';
import { useProgressRedirect } from '../hooks/useProgressRedirect';
import { authService } from '../services/authService';

const AuthForm = ({ mode = 'login' }) => {
  const [formData, setFormData] = useState({ username: '', password: '', role: '', email: '' });
  const [loading, setLoading] = useState(false);
  const [startRedirect, setStartRedirect] = useState(false);

  const progress = useProgressRedirect(() => {
    const decoded = jwtDecode(localStorage.getItem('token'));
    const role = decoded.role?.toLowerCase();
    navigate(role === 'hirer' ? '/dashboard/hirer' : '/dashboard/seeker');
  }, startRedirect ? 200 : null);

  const { login } = useAuth();
  const { showToast } = useToast();
  const navigate = useNavigate();

  const handleChange = (e) => {
    setFormData(prev => ({ ...prev, [e.target.name]: e.target.value }));
  };

  async function handleSubmit(e) {
    e.preventDefault();
    setLoading(true);
    try {
      const res = await (mode === 'signup' ? authService.register(formData) : authService.login(formData));
      login(res.data); // store token
      showToast(`${mode === 'signup' ? 'Signup' : 'Login'} successful!`, true);
      setStartRedirect(true);
    } catch (err) {
      showToast(err.response?.data?.message || 'Authentication Failed', false);
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className="auth-form-container shadow p-4 rounded bg-white position-relative">
      <h3 className="text-center mb-4">{mode === 'signup' ? 'Create an Account' : 'Welcome Back'}</h3>

      <form onSubmit={handleSubmit}>
        {mode !== 'signup' ? (
          <div className="mb-3">
            <label className="form-label">Email or Username</label>
            <input name="username" type="text" className="form-control" onChange={handleChange} required disabled={loading} />
          </div>
        ) : (
          <>
            <div className="mb-3">
              <label className="form-label">Username</label>
              <input name="username" type="text" className="form-control" onChange={handleChange} required disabled={loading} />
            </div>
            <div className="mb-3">
              <label className="form-label">Email</label>
              <input name="email" type="email" className="form-control" onChange={handleChange} required disabled={loading} />
            </div>
          </>
        )}

        <div className="mb-3">
          <label className="form-label">Password</label>
          <input name="password" type="password" className="form-control" onChange={handleChange} required disabled={loading} />
        </div>

        {mode === 'signup' && (
          <div className="mb-3">
            <label className="form-label">Confirm Password</label>
            <input name="confirmPassword" type="password" className="form-control" required disabled={loading} />
          </div>
        )}

        <RoleSelector value={formData.role} onChange={handleChange} disabled={loading} />

        <SpinnerButton loading={loading}>{mode === 'signup' ? 'Sign Up' : 'Log In'}</SpinnerButton>
      </form>

      {startRedirect && progress < 100 && (
        <div className="mt-3">
          <div className="progress">
            <div
              className="progress-bar progress-bar-striped progress-bar-animated bg-success"
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
