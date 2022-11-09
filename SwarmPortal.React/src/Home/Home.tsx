import React from 'react';
import LinksCard from './LinksCard';
import { ApiClient } from '../services/apiClient';
import { ILinkItem, IStatusItem } from '../services/openapi';
import StatusesCard from './StatusesCard';

class Home extends React.Component<{client: ApiClient}, { links: Record<string, ILinkItem[]>, status: Record<string, IStatusItem[]> }> {
    client: ApiClient;
    constructor(props: any) {
        super(props);
        this.client = props.client;
        
    }
    componentDidMount() {
        const links = this.client.isLoggedIn ? this.client.links.getLinksAll() : this.client.links.getLinksPublic();
        links.then(result => this.setState({ links: result }));
        const status = this.client.isLoggedIn ? this.client.statuses.getStatusesAll() : this.client.statuses.getStatusesPublic();
        status.then(result => this.setState({ status: result }));
        // this.client.links.getLinksAll
    }
    siteBackgrounds: string[] = ['primary', 'success', 'danger', 'warning'];
    links(): React.ReactNode[] {
        const result: React.ReactNode[] = [];
        if (this.state?.links) {
            var linkKeys = Object.keys(this.state.links)
            // linkKeys.sort((a,b) => a.localeCompare(b));
            for (var i = 0; i < linkKeys.length; i++) {
                const l = linkKeys[i];
                result.push(<div key={l} className="col-xxl-3 col-xl-4 col-lg-4 col-md-6 col-sm-12 masonry-item p-2">
                                <LinksCard header={l} links={this.state.links[l]} color={this.siteBackgrounds[i%this.siteBackgrounds.length]} />
                            </div>);
            }
        }
        return result;
    }

    statuses(): React.ReactNode[] {
        const result: React.ReactNode[] = [];
        if (this.state?.status) {
            var statusKeys = Object.keys(this.state.status);
            statusKeys.sort((a,b) => a.localeCompare(b));
            for (var i = 0; i < statusKeys.length; i++) {
                const s = statusKeys[i];
                result.push(<div key={s} className="col-xxl-3 col-xl-4 col-lg-4 col-md-6 col-sm-12 masonry-item p-2">
                                <StatusesCard header={s} links={this.state.status[s]} color={this.siteBackgrounds[i%this.siteBackgrounds.length]}></StatusesCard>
                            </div>);
            }
        }
        return result;
    }

    render(): React.ReactNode {
        return (
            <div className="container">
                <div className="row py-3 mr-0">
                    <div key="linksContainer" className="col-xxl-6 col-xl-12 my-xxl-0 my-3">
                        <div className="card bShadow">
                            <h4 className="card-header bg-light text-dark">
                            Links
                            </h4>
                            <div className="card-body p-3">
                                <div className="row">
                                    {this.links()}
                                </div>
                            </div>
                        </div>
                    </div>
                    <div key="statusContainer" className="col-xxl-6 col-xl-12 my-xxl-0 my-3">
                        <div className="card bShadow">
                            <h4 className="card-header bg-light text-dark">
                            Status
                            </h4>
                            <div className="card-body p-3">
                                <div className="row">
                                    {this.statuses()}
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}

export default Home;