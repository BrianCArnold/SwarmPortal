import React from 'react';
import "reflect-metadata";

import './App.css';
import './styles.scss'


import 'ag-grid-community/styles/ag-grid.css'; 
import 'ag-grid-community/styles/ag-theme-alpine.css'; 
import Navigation from './Navigation';

import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Login from './Auth/Login';
import Home from './Home/Home';
import Logout from './Auth/Logout';
import AdminLinkGroups from './Admin/LinkGroups';
import AdminLinkRoles from './Admin/LinkRoles';
import AdminLinks from './Admin/Links';
import { Provider } from 'inversify-react';
import { container } from './services/ioc';

class App extends React.Component {
  timerID!: NodeJS.Timer;
  
  render(): React.ReactNode {
    return (
      <Provider container={container}>
        <Navigation></Navigation>
        <Router>
          <Routes>
            <Route path="/" element={<Home/>} />
            <Route path="Login" element={<Login/>} />
            <Route path="Logout" element={<Logout/>} />
            <Route path="Manage/Groups" element={<AdminLinkGroups/>} />
            <Route path="Manage/Roles" element={<AdminLinkRoles/>} />
            <Route path="Manage/Links" element={<AdminLinks/>} />
          </Routes>
        </Router>
      </Provider>
    );
  }
}

export default App;
