import { Link } from "react-router-dom";

function DrawerContent({currentGroup,userClickedInDrawer,userSubmissions,setIsOpen,handleUserInDrawerClick}){

    return(
    <div className="drawer-main-div">
        <div style={{display:'flex',justifyContent:'flex-end'}}>
            <svg className="drawer-svg" onClick={()=>setIsOpen(false)} xmlns="http://www.w3.org/2000/svg" x="0px" y="0px" width="20" height="20" viewBox="0 0 48 48">
                <linearGradient id="hbE9Evnj3wAjjA2RX0We2a_OZuepOQd0omj_gr1" x1="7.534" x2="27.557" y1="7.534" y2="27.557" gradientUnits="userSpaceOnUse"><stop offset="0" stop-color="#f44f5a"></stop><stop offset=".443" stop-color="#ee3d4a"></stop><stop offset="1" stop-color="#e52030"></stop></linearGradient><path fill="url(#hbE9Evnj3wAjjA2RX0We2a_OZuepOQd0omj_gr1)" d="M42.42,12.401c0.774-0.774,0.774-2.028,0-2.802L38.401,5.58c-0.774-0.774-2.028-0.774-2.802,0	L24,17.179L12.401,5.58c-0.774-0.774-2.028-0.774-2.802,0L5.58,9.599c-0.774,0.774-0.774,2.028,0,2.802L17.179,24L5.58,35.599	c-0.774,0.774-0.774,2.028,0,2.802l4.019,4.019c0.774,0.774,2.028,0.774,2.802,0L42.42,12.401z"></path><linearGradient id="hbE9Evnj3wAjjA2RX0We2b_OZuepOQd0omj_gr2" x1="27.373" x2="40.507" y1="27.373" y2="40.507" gradientUnits="userSpaceOnUse"><stop offset="0" stop-color="#a8142e"></stop><stop offset=".179" stop-color="#ba1632"></stop><stop offset=".243" stop-color="#c21734"></stop></linearGradient><path fill="url(#hbE9Evnj3wAjjA2RX0We2b_OZuepOQd0omj_gr2)" d="M24,30.821L35.599,42.42c0.774,0.774,2.028,0.774,2.802,0l4.019-4.019	c0.774-0.774,0.774-2.028,0-2.802L30.821,24L24,30.821z"></path>
            </svg>
        </div>
        <h4>{currentGroup?.name}</h4>
        <h4>{currentGroup?.description}</h4>
        {
        currentGroup?.users?.map((user,index)=>{
            return <>

            <div className="seconddiv-contest-groupuser" onClick={()=>handleUserInDrawerClick(user.id)}>
                <div>
                    <b>{user.username}</b><br/>
                    {user.email}
                </div>
            </div>

            {
                (userClickedInDrawer && userClickedInDrawer === user.id && userSubmissions?.length > 0) &&
                <div style={{height:'auto',maxHeight:200,overflowY:'scroll'}}>
                    {

                        userSubmissions?.map((submission,index)=>{
                        return (
                            <Link to={`/submission/${submission.id}`} target="_blank">
                            <div className="drawer-submission-item" >
                                <p>{submission.created_at}</p>
                                <p>Accepted</p>
                            </div>
                            </Link>
                        )
                        })
                    }
                </div>

            }

            </>
        })
        }
    </div>
    )
}

export default DrawerContent;