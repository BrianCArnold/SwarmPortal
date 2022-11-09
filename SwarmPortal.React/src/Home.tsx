import React from 'react';
import { ApiClient } from './services/apiClient';
import { ILinkItem, IStatusItem } from './services/openapi';
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
    
    render(): React.ReactNode {
        return (
            <div className="container">
                <div className="row py-3 mr-0">

                    <div className="col-xxl-6 col-xl-12 my-xxl-0 my-3">

                    <div className="card bShadow">
                        <h4 className="card-header bg-light text-dark">
                        Links
                        </h4>
                        <div id="collapseLinks" className="card-body p-3">

                        <div className="row">
                            {this.state?.links && Object.keys(this.state.links).map(l => 
                                <div className="col-xxl-3 col-xl-4 col-lg-4 col-md-6 col-sm-12 masonry-item p-2">
                                    <div>
                                        {l}
                                    </div>
                                </div>
                            )}
                            {/* <div *ngFor="let group of linkGroups; let i = index" className="col-xxl-3 col-xl-4 col-lg-4 col-md-6 col-sm-12 masonry-item p-2">
                            <app-link-group-card [groupName]="group.name" [groupLinks]="group.values" [groupColorName]="siteBackgrounds[i%siteBackgrounds.length]">
                            </app-link-group-card>
                            </div> */}
                        </div>
                        </div>
                    </div>
                    </div>
                    <div className="col-xxl-6 col-xl-12 my-xxl-0 my-3">

                    <div className="card bShadow">
                        <h4 className="card-header bg-light text-dark">
                        Status
                        </h4>
                        <div className="card-body p-3">
                        <div className="row">
                            {this.state?.status && Object.keys(this.state.status).map(s => 
                                <div className="col-xxl-3 col-xl-4 col-lg-4 col-md-6 col-sm-12 masonry-item p-2">
                                    <StatusesCard header={s} links={this.state.status[s]}></StatusesCard>
                                </div>   
                            )}
                            {/* <div className="col-xxl-3 col-xl-4 col-lg-4 col-md-6 col-sm-12" *ngFor="let group of statusGroups; let i = index">
                            <app-status-group-card [groupName]="group.name" [groupStatuses]="group.values" [groupColorName]="siteBackgrounds[i%siteBackgrounds.length]">
                            </app-status-group-card>
                            </div> */}
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