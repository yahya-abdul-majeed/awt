import {Routes,Route} from "react-router-dom";
import EditorPage from "./pages/EditorPage";
import AdminPage from "./pages/AdminPage";
import ContestsPage from "./pages/ContestsPage";
import Navbar from "./components/Navbar";
import ProfilePage from "./pages/ProfilePage";
import {useEffect, useMemo, useState } from "react";
import { UserContext } from "./Contexts";
import QueuePage from "./pages/QueuePage";
import SubmissionDetailPage from "./pages/SubmissionDetailPage";
import SignIn from "./pages/SignIn";

function App() {
  const [user,setUser] = useState();
  console.log("user form app: ",user);

  useEffect(()=>{
    console.log("App useEffct running...")
    var localUser = localStorage.getItem("user");
    if(localUser != null){
      setUser(JSON.parse(localUser))
    }
  },[])

  const ProviderValue = useMemo(()=>({user,setUser}),[user,setUser]);


  return (
    <div id="app">
      <UserContext.Provider value={ProviderValue}>
        <Navbar/>
        <Routes>
          <Route path="/editor/:assignment_id" element={<EditorPage/>}></Route>
          <Route path="/profile/:id" element={<ProfilePage user={user}/>}></Route>
          {/* <Route path="/login" element={<LoginPage/>}></Route>
          <Route path="/register" element={<RegisterPage/>}></Route> */}
          <Route path="/signin" element={<SignIn/>}></Route>
          <Route path="/queue" element={<QueuePage/>}></Route>
          <Route path="/contests" element={<ContestsPage/>}></Route>
          <Route path="/admin" element={<AdminPage/>}></Route>
          <Route path="/submission/:id" element={<SubmissionDetailPage/>}></Route>
        </Routes>
      </UserContext.Provider>

    </div>
  );
}

export default App;
