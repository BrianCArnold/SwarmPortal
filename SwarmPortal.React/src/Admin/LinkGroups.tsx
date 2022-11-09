import React from 'react';
import { ApiClient } from '../services/apiClient';
import { IGroup } from '../services/openapi';

class AdminLinkGroups extends React.Component<{client: ApiClient}, { enabledGroups: IGroup[], disabledGroups: IGroup[], currentGroupName: string | undefined }> {

    get groupName(): string {
        if (this.state && this.state.currentGroupName) {
            return this.state.currentGroupName;
        } else {
            return "";
        }
    }
    set groupName(v: string) {
        this.setState({ currentGroupName: v });
    }
    async addGroup() {
        await this.props.client.admin.postAdminAddGroup(this.groupName);
        this.loadGroups();
    }

    loadGroups() {
        this.props.client.admin.getAdminDisabledGroups().then((response) => {
            this.setState({ disabledGroups: response });
        });
        this.props.client.admin.getAdminEnabledGroupsWithNoLinks().then((response) => {
            this.setState({ enabledGroups: response });
        });
    }

    componentDidMount() {
        this.loadGroups();
    }

    async enableGroup(group: IGroup) {
        await this.props.client.admin.putAdminEnableGroup(group.id || -1);
        this.loadGroups();
    }

    async disableGroup(group: IGroup) {
        await this.props.client.admin.deleteAdminDisableGroup(group.id || -1);
        this.loadGroups();
    }

    disabledGroups(): React.ReactNode[] {
        if (this.state?.enabledGroups){
            return this.state?.disabledGroups?.map((group) => (
                <li className='list-group-item'>
                <button className="btn btn-link"  onClick={() => this.enableGroup(group)}>➕</button> {group.name}
                </li>
            ));
        } else {
            return [];
        }
    }

    updateCurrentGroupName(event: React.ChangeEvent<HTMLInputElement>) {
        this.setState({ currentGroupName: event.target.value });
    }

    enabledGroups(): React.ReactNode[] {
        if (this.state?.enabledGroups){
            return this.state?.enabledGroups?.map((group) => (
                <li className='list-group-item'>
                    <button className="btn btn-link" onClick={() => this.disableGroup(group)}>❌</button> {group.name}
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
                    <h2>Link Groups Management</h2>
                    </div>
                </div>
                <div className="row">
                    <div className="col-6">
                    <h4>Existing Groups</h4>
                    </div>
                    <div className="col-6">
                    <h4>Disabled/Detected Groups</h4>
                    </div>
                </div>
                <div className="row">
                    <div className="col-6">
                    <ul className="list-group">
                        {this.enabledGroups()}
                    </ul>
                    </div>
                    <div className="col-6">
                    <ul className="list-group">
                        {this.disabledGroups()}
                    </ul>
                    </div>
                </div>
                <div className="row">
                    <div className="col-lg-5">
                    <input placeholder="Group Name" value={this.groupName} onChange={e => this.groupName = e.target.value} className="form-control"/>
                    </div>
                    <div className="col-lg-2">
                    <button onClick={() => this.addGroup()} className="form-control btn btn-primary col-lg-2">Add Group</button>
                    </div>
                    <div className="col-12">
                    </div>
                </div>
            </div>
        );
    }
}

export default AdminLinkGroups;