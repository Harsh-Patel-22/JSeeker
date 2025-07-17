import AuthForm from '../components/AuthForm';
import { Link } from 'react-router-dom';

const LoginPage = () => {
  return (
    <div className="auth-page d-flex justify-content-center align-items-center vh-100">
      <div className="w-100" style={{ maxWidth: '420px' }}>
        <AuthForm mode="login" />
        <p className="text-center mt-3">
          Don't have an account? <Link to="/signup">Sign up</Link>
        </p>
      </div>
    </div>
  );
}

export default LoginPage;