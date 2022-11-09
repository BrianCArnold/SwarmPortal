import React from 'react';

import './App.css';
import './styles.scss'
import { ApiClient } from './services/apiClient';
import { ILinkItem } from './services/openapi';
import Navigation from './Navigation';

import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Login from './Auth/Login';
import Home from './Home/Home';
import Logout from './Auth/Logout';

class App extends React.Component<{}, { links: Record<string, ILinkItem[]> }> {
  client: ApiClient;
  timerID!: NodeJS.Timer;
  links: Record<string, ILinkItem[]> = {};
  constructor(props: any) {
    super(props);
    this.client = new ApiClient();
  }


  componentDidMount() {
    this.client.links.getLinksPublic().then((response) => {
      this.setState({ links: response });
    });
  }

  render(): React.ReactNode {
    return (
      <div>
      <Navigation client={this.client}></Navigation>
      <Router>
        <Routes>
          <Route path="/" element={<Home client={this.client} />} />
          <Route path="Login" element={<Login client={this.client} />} />
          <Route path="Logout" element={<Logout client={this.client} />} />

        </Routes>
      </Router>
      </div>
    );
  }
}

export default App;
