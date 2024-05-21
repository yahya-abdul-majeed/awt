import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App';
import {Route, BrowserRouter as Router} from 'react-router-dom'
// import 'C:/Users/yahya/Desktop/dolphin/node_modules/rc-easyui/dist/themes/default/easyui.css';
// import 'C:/Users/yahya/Desktop/dolphin/node_modules/rc-easyui/dist/themes/icon.css';
import 'C:/Users/yahya/Desktop/dolphin/node_modules/rc-easyui/dist/themes/react.css';
  
const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <React.StrictMode>
    <Router>
      <App />
    </Router>
  </React.StrictMode>
);

