import React from 'react';
import './Links.scss';
import { ApiClient } from '../services/apiClient';
import { AgGridReact, AgGridColumn } from 'ag-grid-react';
import { ColDef, CellClickedEvent, ValueGetterParams, ValueSetterParams, NewValueParams } from 'ag-grid-community';
import { IGroup, ILink, IRole } from '../services/openapi';

type linkRow = { id: number; url: string; name: string; roles: { [key: string]: boolean; }; group: string; enabled: boolean; };

class AdminLinks extends React.Component<{client: ApiClient}, { linkGroup: string, linkName: string, linkUrl: string, adminLinks: ILink[], adminRoles: IRole[], adminGroups: IGroup[], rowData: linkRow[] }> {
    disableLink(data: linkRow) {
        throw new Error('Method not implemented.');
    }
    enableLink(data: linkRow) {
        throw new Error('Method not implemented.');
    }
    
    gridColumns!: ColDef<linkRow>[];
    
    

    private staticColumnDefs: ColDef<linkRow>[] = [
    { 
        valueGetter: p => {
            console.log(p);
            if (p.data?.enabled) return '❌'; else return '➕';
        }, 
        width: 60, 
        sortable: true, 
        filter: true,
        onCellClicked: (event: CellClickedEvent<linkRow>) => {
            if (event.data) {
                if (event.data.enabled) {
                    this.disableLink(event.data)
                } else { 
                    this.enableLink(event.data) 
                } 
            }
        }
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
            valueGetter: (params: ValueGetterParams<linkRow>) => {
                if (params.data){
                    return params.data.roles[r.name || ''];
                } else {
                    return '';
                }
            },
            valueSetter: (params: ValueSetterParams<linkRow>) => {
                if (params.data) {
                    const data = params.data;
                    data.roles[r.name || ''] = params.newValue;
                    //I believe 'true' means successful update, need to check
                    return true;
                } else {
                    return false;
                }
            },
            sortable: false,
            filter: false,
            editable: true,
            resizable: true,
            // cellEditor: CheckboxEditor,
            // cellRenderer: CheckboxRenderer,
            onCellValueChanged: (event: NewValueParams<linkRow>) => {
                this.UpdateLink(event.data, r.name || '', event.newValue);
            },
          }
        ))
      }
    private UpdateLink(link: linkRow, role: string, newValue: boolean) {
        throw new Error('Method not implemented.');
    }
    private async loadData() {
        const allLinks = await this.props.client.admin.getAdminAllLinks();
        const allRoles = await this.props.client.admin.getAdminRoles().then(roles => roles.filter(r => r.enabled));
        const allGroups = await this.props.client.admin.getAdminGroups().then(groups => groups.filter(g => g.enabled));
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
    async loadData_new(){
        this.setState({ 
            adminLinks: await this.props.client.admin.getAdminAllLinks(), 
            adminRoles: await this.props.client.admin.getAdminRoles().then(roles => roles.filter(r => r.enabled)), 
            adminGroups: await this.props.client.admin.getAdminGroups().then(groups => groups.filter(g => g.enabled)) 
        });
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

    render(): React.ReactNode {
        return ( this.state && this.rowData &&
            <div className="container">
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
                    {/* <select placeholder="Group" [(ngModel)]="linkGroup" className="form-select">
                        <option *ngFor="let group of allGroups" [value]="group.name">{{group.name}}</option>
                    </select> */}
                    </div>
                    <div className="col-lg-3">
                        <input placeholder="SwarmPortal" className="form-control" value={this.linkName} onChange={e => this.linkName = e.target.value} />
                    </div>
                    <div className="col-lg-5">
                        <input placeholder="https://swarmportal.com" className="form-control" value={this.linkUrl} onChange={e => this.linkUrl = e.target.value} />
                    </div>
                    <div className="col-lg-2">
                    {/* <button [disabled]="isAddDisabled" className="form-control btn btn-primary col-lg-2" (click)="addLink()">Add Link</button> */}

                    </div>
                </div>
            </div>

        );
    }
}

export default AdminLinks;