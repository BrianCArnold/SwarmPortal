import React from 'react';
import { ApiClient } from './services/apiClient';

class Home extends React.Component<{client: ApiClient}, { }> {
    client: ApiClient;
    constructor(props: any) {
        super(props);
        this.client = props.client;
        ;
    }
    componentDidMount() {
    }
    
    render(): React.ReactNode {
        return (
            <div>
                Home
            </div>
        );
    }
}

export default Home;