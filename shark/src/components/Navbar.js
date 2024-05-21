import { Link } from "react-router-dom";

export default function Navbar(){
    return(
        <div className="nav" style={{
            height:40,
            display:'flex',
            justifyContent:'center',
            alignItems:'center',

        }}>
            <Link className="nav-item" to="/assignment">Assignment</Link>
            <Link to="/website">Website</Link>
            <Link to="/skeleton">Skeleton</Link>
            <Link to="/tools">Tools</Link>
        </div>
    );
}