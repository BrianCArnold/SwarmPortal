import { resolve } from 'inversify-react';
import React from 'react';
import { IApiClient } from '../services/Interfaces/IApiClient';

class Login extends React.Component {
    @resolve("apiClient") private readonly client!: IApiClient;

    componentDidMount() {
        this.client.processLogIn().then(r => {
            console.log(r);
            // setTimeout(() => {
            //     window.location.href = "/";
            // }, 2000);
        });
    }
    
    render(): React.ReactNode {
        return (
            <p>success, redirecting home...</p>
        );
    }
}

export default Login;