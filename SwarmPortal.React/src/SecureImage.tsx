
import { resolve } from 'inversify-react';
import React from 'react';
import { IApiClient } from './services/Interfaces/IApiClient';
import { IApiConfiguration } from './services/Interfaces/IApiConfiguration';

// This is necessary because the backend requires the relevant Forward Auth cookie and Auth Token to be passed through to the API.
class SecureImage extends React.Component<{ src: string, alt: string, className?: string }, { srcData: string }> {
    @resolve("apiClient") private readonly client!: IApiClient;
    @resolve("apiConfig") private readonly config!: IApiConfiguration;
    constructor(props: { src: string, alt: string, className?: string }) {
        super(props);
        this.state = { srcData: this.tinyBlankImage };
    }
    componentDidMount() {
        const reqUrl = this.config.BASE + "/Icon/" + this.props.src;
        const init: RequestInit = {};
        if (this.client.isLoggedIn) {
            init.headers = { "Authorization": "Bearer " + this.client.rawToken, ...init.headers };
        }
        const reader = new FileReader();
        reader.onloadend = () => this.setState({srcData: (reader.result as string)});
        fetch(reqUrl, init).then(r => r.blob()).then(b => reader.readAsDataURL(b));
    }
    private tinyBlankImage = 'data:image/gif;base64,R0lGODlhAQABAAAAACH5BAEKAAEALAAAAAABAAEAAAICTAEAOw==';
    render(): React.ReactNode {
        return <img src={this.state.srcData} alt={this.props.alt} className={this.props.className} />;
    }
}

export default SecureImage;