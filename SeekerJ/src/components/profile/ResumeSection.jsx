import 'bootstrap/dist/css/bootstrap.css'
import { useAuth } from '../../contexts/AuthContext';
import ResumeButton from '../ui/ResumeButton';
import '../profile/ProfilePage.css';
import { useNavigate } from 'react-router';

const ResumeSection = () => {
  const { user } = useAuth();
  const navigate = useNavigate();

  return (
    <div className="text-md-end">
      
      {/* Label */}
      {/* <div className="text-uppercase text-muted fw-semibold mb-1" style={{ fontSize: '0.75rem' }}>
        Resume
      </div> */}

      {/* Action container */}
      <div className="card border-0 shadow-sm rounded-4 p-4 mb-4">
        <div className="row align-items-center">

          {/* LEFT: Profile info */}
          <div className="col-md-4">
          </div>

          {/* RIGHT: Resume actions */}
          <div className="col-md-8 text-md-end mt-3 mt-md-0">
            <div className="d-inline-flex flex-column align-items-center"></div>
              <div className="text-uppercase text-muted fw-semibold mb-1"> Resume</div>
                <div className="d-inline-flex align-items-center border rounded px-2 py-1 gap-2">
                  {/* View */}
                  <ResumeButton
                    targetClientId={user?.clientId}
                    className="btn btn-sm p-0 border-0 bg-transparent shadow-none text-secondary icon-action view"
                    style={{ boxShadow: "none" }}
                  >
                    <i className="bi bi-eye fs-5" title="View Resume"></i>
                  </ResumeButton>

                  <span className="text-muted">|</span>

                  {/* Edit */}
                  <i
                    className="bi bi-pencil fs-5 text-secondary icon-action edit"
                    role="button"
                    title="Create/Edit Resume"
                    onClick={() => {navigate('/resume')}}
                  ></i>

                </div>
              </div>
          </div>

        </div>
      </div>
  );
};

export default ResumeSection;
