import React from 'react';
import logo from './logo.svg';
import './App.css';
import './styles.scss'
import { ApiClient } from './services/apiClient';
import { ILinkItem } from './services/openapi';

class App extends React.Component<{}, { links: Record<string, ILinkItem[]> }> {
  client: ApiClient;
  timerID!: NodeJS.Timer;
  links: Record<string, ILinkItem[]> = {};
  constructor(props: any) {
    super(props);
    this.setState({ links: {"a": []} });
    this.client = new ApiClient();
  }


  componentDidMount() {
    this.client.links.getLinksPublic().then((response) => {
      this.setState({ links: response });
    });
  }

  render(): React.ReactNode {
    return (
      <div className="App">
        <ul>
          
          {this.state?.links && Object.keys(this.state.links).map((key) => {
            return (
            <li>
              {key}
              <ul>
                {this.state.links[key].map((link) => link.name)}
              </ul>
            </li>
            );
          })}
        </ul>
      </div>
    );
  }
}

export default App;
