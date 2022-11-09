import React from 'react';
import { ApiClient } from './services/apiClient';

class Logout extends React.Component<{client: ApiClient}, { }> {
    client: ApiClient;
    constructor(props: any) {
        super(props);
        this.client = props.client;
        ;
    }
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
            <div>
                <button className="btn btn-sm me-2" type="button">Back Home</button>
            </div>
        );
    }
}

export default Logout;