import { resolve } from 'inversify-react';
import React from 'react';
import { IApiClient } from '../services/Interfaces/IApiClient';

class Logout extends React.Component {
    @resolve("apiClient") private readonly client!: IApiClient;

    componentDidMount() {
        this.client.processLogOut().then(r => {
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

export default Logout;