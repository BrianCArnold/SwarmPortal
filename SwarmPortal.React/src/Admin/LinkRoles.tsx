import React from 'react';
import { ApiClient } from '../services/apiClient';
import { IRole } from '../services/openapi';

class AdminLinkRoles extends React.Component<{client: ApiClient}, { enabledRoles: IRole[], disabledRoles: IRole[], currentRoleName: string | undefined }> {
   
    get roleName(): string {
        if (this.state && this.state.currentRoleName) {
            return this.state.currentRoleName;
        } else {
            return "";
        }
    }
    set roleName(v: string) {
        this.setState({ currentRoleName: v });
    }
    async addRole() {
        await this.props.client.admin.postAdminAddRole(this.roleName);
        this.loadRoles();
    }

    loadRoles() {
        this.props.client.admin.getAdminDisabledRoles().then((response) => {
            this.setState({ disabledRoles: response });
        });
        this.props.client.admin.getAdminEnabledRolesWithNoLinks().then((response) => {
            this.setState({ enabledRoles: response });
        });
    }

    componentDidMount() {
        this.loadRoles();
    }

    async enableRole(role: IRole) {
        await this.props.client.admin.putAdminEnableRole(role.id || -1);
        this.loadRoles();
    }

    async disableRole(role: IRole) {
        await this.props.client.admin.deleteAdminDisableRole(role.id || -1);
        this.loadRoles();
    }

    disabledRoles(): React.ReactNode[] {
        if (this.state?.enabledRoles){
            return this.state?.disabledRoles?.map((role) => (
                <li className='list-group-item'>
                <button className="btn btn-link"  onClick={() => this.enableRole(role)}>➕</button> {role.name}
                </li>
            ));
        } else {
            return [];
        }
    }

    updateCurrentRoleName(event: React.ChangeEvent<HTMLInputElement>) {
        this.setState({ currentRoleName: event.target.value });
    }

    enabledRoles(): React.ReactNode[] {
        if (this.state?.enabledRoles){
            return this.state?.enabledRoles?.map((role) => (
                <li className='list-group-item'>
                    <button className="btn btn-link" onClick={() => this.disableRole(role)}>❌</button> {role.name}
                </li>
            ));
        } else {
            return [];
        }
    }

    render(): React.ReactNode {
        return (
            <div className="container">
                <div className="row">
                    <div className="col-12">
                    <h2>Link Roles Management</h2>
                    </div>
                </div>
                <div className="row">
                    <div className="col-6">
                    <h4>Existing Roles</h4>
                    </div>
                    <div className="col-6">
                    <h4>Disabled/Detected Roles</h4>
                    </div>
                </div>
                <div className="row">
                    <div className="col-6">
                    <ul className="list-group">
                        {this.enabledRoles()}
                    </ul>
                    </div>
                    <div className="col-6">
                    <ul className="list-group">
                        {this.disabledRoles()}
                    </ul>
                    </div>
                </div>
                <div className="row">
                    <div className="col-lg-5">
                    <input placeholder="Role Name" value={this.roleName} onChange={e => this.roleName = e.target.value} className="form-control"/>
                    </div>
                    <div className="col-lg-2">
                    <button onClick={() => this.addRole()} className="form-control btn btn-primary col-lg-2">Add Role</button>
                    </div>
                    <div className="col-12">
                    </div>
                </div>
            </div>
        );
    }
}

export default AdminLinkRoles;