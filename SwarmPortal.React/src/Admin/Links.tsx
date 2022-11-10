import React from 'react';
import './Links.scss';
import { IApiClient } from '../services/Interfaces/IApiClient';
import { AgGridReact } from 'ag-grid-react';
import { ColDef } from 'ag-grid-community';
import { IGroup, ILink, IRole } from '../services/openapi';
import { resolve } from 'inversify-react';
import DeleteLinkButtonCellRenderer from '../Grid/DeleteLinkButtonCellRenderer';
import RoleLinkButtonCellRenderer from '../Grid/RoleLinkButtonCellRenderer';
import { linkRow } from '../Models/linkRow';

class AdminLinks extends React.Component<{}, { linkGroup: string, linkName: string, linkUrl: string, adminLinks: ILink[], adminRoles: IRole[], adminGroups: IGroup[], rowData: linkRow[] }> {
    @resolve("apiClient") private readonly client!: IApiClient;
    
    gridColumns!: ColDef<linkRow>[];
    
    

    private staticColumnDefs: ColDef<linkRow>[] = [
    { 
        field: 'id',
        headerName: 'Active',
        cellRenderer: DeleteLinkButtonCellRenderer,
        cellRendererParams: {
            onChange: async () => await this.loadData()
        },
        width: 74, 
        sortable: false, 
        filter: false
    },
    { field: 'group', sortable: true, filter: true },
    { field: 'name', sortable: true, filter: true },
    { field: 'url', sortable: true, filter: true },
    ];
    private generateColumnDefs(allRoles: IRole[]): ColDef<linkRow>[] {
    return [
        ...this.staticColumnDefs,
        ...this.GenerateRoleColumns(allRoles)
    ];
    }
    private GenerateRoleColumns(allRoles: IRole[]): ColDef<linkRow>[] {
        return allRoles.map(r => ({
            headerName: r.name || '',
            sortable: false,
            filter: false,
            resizable: true,
            width: 150,
            cellRenderer: RoleLinkButtonCellRenderer,
            cellRendererParams: {
                onChange: async () => await this.loadData(),
                role: r.name || ''
            },
          }
        ))
      }
    private UpdateLink(link: linkRow, role: string, newValue: boolean) {
        throw new Error('Method not implemented.');
    }
    private async loadData() {
        const allLinks = await this.client.admin.getAdminAllLinks();
        const allRoles = await this.client.admin.getAdminRoles().then(roles => roles.filter(r => r.enabled));
        const allGroups = await this.client.admin.getAdminGroups().then(groups => groups.filter(g => g.enabled));
        await this.setState({ adminLinks: allLinks, adminRoles: allRoles, adminGroups: allGroups });
        const createRoleDictionaryForLink = (linkRoles: string[]) =>  {
            return allRoles.reduce<{ [key: string]: boolean }>((result, role) => {
              result[role.name || ''] = linkRoles.includes(role.name || '');
              return result;
            }, {});
        }
        if (this.linkGroup == null && allGroups.length > 0) {
          this.linkGroup = allGroups[0].name||'';
        }
        this.rowData = allLinks.map(link => ({
          id: link.id || -1,
          url: link.url || '',
          name: link.name || '',
          roles: createRoleDictionaryForLink(link.roles?.map(r => r.name || '') || []),
          group: link.group?.name || '',
          enabled: link.enabled!
        }));
        this.gridColumns = this.generateColumnDefs(allRoles);
        // setTimeout(() => this.gridApi.sizeColumnsToFit(), 10);
    }
    componentDidMount() {
        this.loadData();
    }
    get rowData(): linkRow[] {
        if (this.state && this.state.rowData) {
            return this.state.rowData;
        } else {
            return [];
        }
    }
    set rowData(v: linkRow[]) {
        this.setState({ rowData: v });
    }
    get linkGroup(): string {
        if (this.state && this.state.linkGroup) {
            return this.state.linkGroup;
        } else {
            return "";
        }
    }
    set linkGroup(v: string) {
        this.setState({ linkGroup: v });
    }
    get linkName(): string {
        if (this.state && this.state.linkName) {
            return this.state.linkName;
        } else {
            return "";
        }
    }
    set linkName(v: string) {
        this.setState({ linkName: v });
    }
    get linkUrl(): string {
        if (this.state && this.state.linkUrl) {
            return this.state.linkUrl;
        } else {
            return "";
        }
    }
    set linkUrl(v: string) {
        this.setState({ linkUrl: v });
    }
    addLink() {
        this.client.admin.postAdminAddLink({ name: this.linkName, url: this.linkUrl, group: this.linkGroup, roles: [] }).then(async () => await this.loadData());
    }

    get isAddDisabled(): boolean {
        return this.linkGroup == '__initial_invalid' || !this.linkName || !this.linkUrl;
    }

    render(): React.ReactNode {
        return ( this.state && this.rowData &&
            <div className="container-fluid">
                <div className="row py-3 mr-0">
                    <div className="col-12">
                    <h2>Link Management</h2>
                    </div>
                </div>
                <div className="row">
                    <div className="col-12 ag-theme-alpine-dark gridSize">
                        <AgGridReact rowData={this.state.rowData}
                            columnDefs={this.gridColumns}
                            
                        ></AgGridReact>
                    </div>
                </div>
                <div className="row">
                    <div className="col-lg-2">
                    Group
                    </div>
                    <div className="col-lg-3">
                    Name
                    </div>
                    <div className="col-lg-5">
                    URL
                    </div>


                </div>
                <div className="row">
                    <div className="col-lg-2">
                    <select placeholder="Group" value={this.linkGroup} onChange={e => this.linkGroup = e.target.value} className="form-select">
                        <option value="__initial_invalid">Select Group</option>
                        {this.state.adminGroups.map(g => <option key={g.name} value={g.name||""}>{g.name}</option>)}
                    </select>
                    </div>
                    <div className="col-lg-3">
                        <input placeholder="SwarmPortal" className="form-control" value={this.linkName} onChange={e => this.linkName = e.target.value} />
                    </div>
                    <div className="col-lg-5">
                        <input placeholder="https://swarmportal.com" className="form-control" value={this.linkUrl} onChange={e => this.linkUrl = e.target.value} />
                    </div>
                    <div className="col-lg-2">
                    <button disabled={this.isAddDisabled} className="form-control btn btn-primary col-lg-2" onClick={() => this.addLink()}>Add Link</button>

                    </div>
                </div>
            </div>

        );
    }
}

export default AdminLinks;