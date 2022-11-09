    import React from 'react';
import { ApiClient } from './services/apiClient';

class Navigation extends React.Component<{client: ApiClient}, { }> {
    client: ApiClient;
    constructor(props: {client: ApiClient}) {
        super(props);
        this.client = props.client;
        console.log(this.client);
    }

    async logIn() {
        await this.client.login();
    }
    async logOut() {
        await this.client.logOut();
    }
    render(): React.ReactNode {
        return (
        <nav className="navbar navbar-expand-md navbar-light bg-light">
            <div className="container-fluid justify-content-start">
                <a className="navbar-brand" href="/">SwarmPortal</a>
                <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                <span className="navbar-toggler-icon"></span>
                </button>
                <div className="collapse navbar-collapse" id="navbarSupportedContent">
                <ul className="navbar-nav me-auto mb-2 mb-md-0">
                    <li className="nav-item">
                    <a className="nav-link" href="/">Home</a>
                    </li>
                    
                    {/* {this.client.isLoggedIn && this.client.
                    <li class="nav-item dropdown" *ngIf="isLoggedIn && Roles.includes('admin')">
                    <a class="nav-link dropdown-toggle" href="#" id="navbarDropdown" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                        Admin
                    </a>
                    <ul class="dropdown-menu" aria-labelledby="navbarDropdown">
                        <li><a class="dropdown-item" routerLink="admin/links" >Manage Links</a></li>
                        <li><a class="dropdown-item" routerLink="admin/groups" >Manage Link Groups</a></li>
                        <li><a class="dropdown-item" routerLink="admin/roles" >Manage Roles</a></li>
                    </ul>
                    </li> */}
                </ul>
                </div>
            </div>
            <form className="container-fluid justify-content-end">
                {this.client.isLoggedIn && <button className="btn btn-sm me-2" type="button">Welcome {this.client.token.given_name}</button>}
                {this.client.isLoggedIn && <button className="btn btn-primary btn-sm me-2" type="button" onClick={() => this.logOut()}>Logout</button>}
                {!this.client.isLoggedIn && <button className="btn btn-primary btn-sm me-2" type="button" onClick={() => this.logIn()} >Login</button>} 
                
            </form>
        </nav>
        );


    }    
}

export default Navigation;