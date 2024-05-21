import { useState } from "react";
import { requests } from "../data/Requests";
import { useNavigate } from "react-router-dom";

function RegisterPage(){

    const [username,setUsername] = useState();
    const [email,setEmail] = useState();
    const [password,setPassword] = useState();
    const [confirmPassword,setConfirmPassword] = useState();
    // const [token,setToken] = useState(null)
    // const navigate = useNavigate();


    async function handleSubmit(e){
        e.preventDefault()
        if(password != confirmPassword){
            console.log("password must match")
            return;
        }
        try{
            var response = await requests.Register({username,email,password})
            if(response.ok){
                // let tkn = await response.text();
                // setToken(tkn,console.log(tkn))
                // navigate("/login");
            }
        }
        catch(err){
            console.log(err)
        }
    }
    function handleUsername(e){
        setUsername(e.target.value)
    }
    function handleEmail(e){
        setEmail(e.target.value)
    }
    function handlePassword(e){
        setPassword(e.target.value)
    }
    function handleConfirmPassword(e){
        setConfirmPassword(e.target.value)
    }
    return(
        <div style={{
            display:'flex',
            flexDirection:'column',
            // alignItems:'center'
        }}>
            <h3>Register</h3>
            <form onSubmit={handleSubmit}>
                <label for="username">Username:</label><br/>
                <input value={username} onChange={handleUsername} type="text" id="username" name="username"/><br/>
                <label for="email">Email:</label><br/>
                <input value={email} onChange={handleEmail} type="email" id="email" name="email"/><br/>
                <label for="password">Password:</label><br/>
                <input  value={password} onChange={handlePassword} type="password" id="password" name="password"/><br/>
                <label for="confirm_password">Confirm Password:</label><br/>
                <input  value={confirmPassword} onChange={handleConfirmPassword} type="password" id="confirm_password" name="confirm_password"/><br/><br/>
                <input type="submit" value="Register"/>
            </form>
        </div>
    );
}

export default RegisterPage;