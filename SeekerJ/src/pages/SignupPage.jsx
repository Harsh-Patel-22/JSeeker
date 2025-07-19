import AuthForm from '../components/AuthForm';
import { Link } from 'react-router-dom';
import { ToastProvider } from '../contexts/ToastContext';

const SignupPage = () => {
  return (
    <div className="auth-page d-flex justify-content-center align-items-center vh-100">
      <div className="w-100" style={{ maxWidth: '420px' }}>
        <ToastProvider>
          <AuthForm mode="signup" />
        </ToastProvider>
        <p className="text-center mt-3">
          Already have an account? <Link to="/">Log in</Link>
        </p>
      </div>
    </div>
  );
}

export default SignupPage;