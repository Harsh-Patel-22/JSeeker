import BaseForm from "./BaseForm";
import { useState, useCallback } from "react";
import { useNavigate } from "react-router";
import { useToast, ToastProvider } from "../../contexts/ToastContext";
import { useProgressRedirect } from "../../hooks/useProgressRedirect";
import { userService } from "../../services/apiServices";

const States = {
    GithubUsernameInput: "GithubUsernameInput",
    RepoNamesInput: "RepoNamesInput",
    Completed: "Completed"
};

const GithubForm = () => {
    const { showToast } = useToast();
    const navigate = useNavigate();

    const [loading, setLoading] = useState(false);
    const [startRedirect, setStartRedirect] = useState(false);

    const [fillingState, setFillingState] = useState(States.GithubUsernameInput);

    const onComplete = useCallback(() => {
        navigate("/dashboard/hirer");
    }, [navigate]);

    const progress = useProgressRedirect(onComplete, startRedirect ? 50 : null);

    const fields = [
            { name: "githubUsername", label: "Enter your Github Username (Case Sensitive)", type: "text", required: true, showRequired: false},
    ];

    const repoFields = [
        { name: "repoNames", label: "Enter your Repository Names (Comma Separated, Case Sensitive)", type: "text", required: true, showRequired: false},
    ];

    const validate = (data) => {
        const errors = {};
        // if (mode === "signup") {
        //   if (data.password !== data.confirmPassword) {
        //     errors.confirmPassword = "Passwords do not match";
        //   }
        //   if (data.phoneNumber && data.phoneNumber.length !== 10) {
        //     errors.phoneNumber = "Phone number must be 10 digits";
        //   }
        // }
        return errors;
    };

    const handleUsernameSubmit = async (formData) => {
        setLoading(true);
        try {
            console.log(formData.githubUsername);
            // const res = await userService.updateGithubUsername(formData.githubUsername);
        
            if(true || res.status === HttpStatusCode.Ok){
                showToast("Github Linked Successfully!", true);
                setFillingState(States.RepoNamesInput);
                // setStartRedirect(true);
            }
        } catch (err) {
        showToast(err.response?.data?.message || "Linking Failed", false);
        } finally {
        setLoading(false);
        }
    };

    return <div className="auth-page d-flex justify-content-center align-items-center vh-100">
        <div className="w-100" style={{ maxWidth: '420px' }}>
            <ToastProvider>
                
            {fillingState == States.GithubUsernameInput && <BaseForm
                title="Github Linking"
                fields={fields}
                validate={validate}
                onSubmit={handleUsernameSubmit}
                loading={loading}
                redirectProgress={startRedirect ? progress : null}
            />}

            {fillingState == States.RepoNamesInput && 
            <>
                <div className="d-flex justify-content-center">
                    <button className="btn btn-primary mb-3 w-100" onClick={() => setFillingState(States.Completed)}>Auto select the best</button>
                </div>

                <div className="d-flex justify-content-center">
                <h4>OR</h4>
                </div>
                <BaseForm
                    title="Repository Names"
                    subtitle="At max 3 repos"
                    fields={repoFields}
                    validate={validate}
                    onSubmit={handleUsernameSubmit}
                    loading={loading}
                    redirectProgress={startRedirect ? progress : null
                    }
                />
            </>
            }
            </ToastProvider>
        </div>
        </div>
};

export default GithubForm;