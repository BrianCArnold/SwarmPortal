import React from 'react';
import { ApiClient } from './services/apiClient';

class Login extends React.Component<{client: ApiClient}, { }> {
    client: ApiClient;
    constructor(props: any) {
        super(props);
        this.client = props.client;
        ;
    }
    componentDidMount() {
        this.client.processLogIn().then(r => {
            console.log(r);
            setTimeout(() => {
                window.location.href = "/";
            }, 2000);
        });
    }
    
    render(): React.ReactNode {
        return (
            <p>success, redirecting home...</p>
        );
    }
}

export default Login;