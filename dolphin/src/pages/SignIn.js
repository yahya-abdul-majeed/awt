import LoginPage from "./LoginPage";
import RegisterPage from "./RegisterPage";

function SignIn(){
    return(<div id="signin" style={{
        display:'grid',
        gridTemplateColumns:'1fr 1fr',
        margin:50,
    }}>
        <LoginPage/>
        <RegisterPage/>
    </div>);
}

export default SignIn;