import { useContext, useEffect, useRef, useState } from 'react';
import {requests} from '../data/Requests'
import { UserContext } from '../Contexts';
import { Utility } from '../Utility';
import RegisterPage from './RegisterPage';

function LoginPage(){

    const {user,setUser} = useContext(UserContext);

    const [email,setEmail] = useState('yahya.zf2@gmail.com')
    const [password,setPassword] = useState('yahya')
    const [token,setToken] = useState(null)

    async function handleSubmit(e){
        e.preventDefault()
        try{
            requests.Login({email,password})
            .then(token=>{
                setToken(token,console.log(token));
                var data = Utility.parseJwt(token);
                setUser({...data,token});
                localStorage.setItem("user",JSON.stringify({...data,token}));
            })
        }catch(err){
            console.log("login failed");
        }
    }
    function handleEmail(e){
        setEmail(e.target.value)
    }
    function handlePassword(e){
        setPassword(e.target.value)
    }

    return(
        <div style={{
            display:'flex',
            flexDirection:'column',
            alignItems:'flex-end',
            marginRight:100
        }}>
        <h3 style={{marginRight:110}}>Login</h3>
            <form onSubmit={handleSubmit}>
                <label for="email">Email:</label><br/>
                <input value={email} onChange={handleEmail} type="text" id="email" name="email"/><br/>
                <label  for="password">Password:</label><br/>
                <input value={password} onChange={handlePassword} type="password" id="password" name="password"/><br/><br/>
                <input type="submit" value="Login" />
            </form>
        </div>
    );

}

export default LoginPage;