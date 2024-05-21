import { useContext, useEffect, useState } from "react";
import { UserContext } from "../Contexts";
import { Link } from "react-router-dom";
import { Utility } from "../Utility";

function Navbar(){
    const {user} = useContext(UserContext)
    // const [isAdmin,setIsAdmin] = useState(false);

    const isAdmin = user?.roles?.includes('Level-2');


    // useEffect(()=>{
    //     console.log("Navbar useEffect")
    //     if(user?.roles.includes('Level-2')){
    //         setIsAdmin(true);
    //     }

    // },[])

    return(
        <div style={{
            display:'flex',
            justifyContent:'space-between',
            height:'50px',
            // background: 'rgba(0, 0, 255, 0.5)',
            backgroundColor:"lightgray",
            alignItems:'center',
            padding: '10px',
            }}>
                <div>
                    {/* <Link to="/login" style={{paddingRight:'10px',textDecoration:'none'}}>[Login]</Link>
                    <Link to="/register" style={{paddingRight:'10px',textDecoration:'none'}}>[Register]</Link> */}
                    <Link to="/signin" style={{paddingRight:'10px',textDecoration:'none'}}>[SignIn]</Link>
                    <Link to="/editor/1" style={{paddingRight:'10px',textDecoration:'none'}}>[Editor]</Link>
                    <Link to="/contests" style={{paddingRight:'10px',textDecoration:'none'}}>[Contests]</Link>
                    <Link to="/queue" style={{paddingRight:'10px',textDecoration:'none'}}>[Queue]</Link>
                    <Link to="/admin" style={{paddingRight:'10px',textDecoration:'none'}}>[Admin]</Link>
                </div>
                <div>
                    <b>Logged in as: <Link to={`/profile/${user?.id}`} style={{paddingRight:'10px',textDecoration:'none'}}>[ {user && Utility.capitalizeFirstLetter(user?.name)}| {user ? (isAdmin?'Admin':'Student'):''}]</Link></b>
                </div>

        </div>
    );

}

export default Navbar;