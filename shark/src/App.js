import {BrowserRouter as Router,Routes,Route} from "react-router-dom";
import Assignment from "./pages/Assignment";
import Website from "./pages/Website";
import Skeleton from "./pages/Skeleton";
import Tools from "./pages/Tools";
import Navbar from "./components/Navbar";

function App() {
  return (
    <div>
      <Router>
        <Navbar/>
        <Routes>
          <Route path="/assignment" element={<Assignment/>}></Route>
          <Route path="/website" element={<Website/>}></Route>
          <Route path="/skeleton" element={<Skeleton/>}></Route>
          <Route path="/tools" element={<Tools/>}></Route>
        </Routes>
      </Router>
    </div>
  );
}

export default App;
