import AuthForm from '../components/forms/AuthForm';
import { Link } from 'react-router-dom';
import { ToastProvider } from '../contexts/ToastContext';
const LoginPage = () => {
  return (
    <div className="auth-page d-flex justify-content-center align-items-center vh-100">
      <div className="w-100" style={{ maxWidth: '420px' }}>
        <ToastProvider>
          <AuthForm mode="login" />
        </ToastProvider>
        <p className="text-center mt-3">
          Don't have an account? <Link to="/signup">Sign up</Link>
        </p>
      </div>
    </div>
  );
}

export default LoginPage;