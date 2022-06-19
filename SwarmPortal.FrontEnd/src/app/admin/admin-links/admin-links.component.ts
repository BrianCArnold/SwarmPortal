import { Component, OnInit } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { GridApi, CellClickedEvent, ColDef, GridReadyEvent, ValueSetterParams, ValueGetterParams } from 'ag-grid-community';
import { GridLinkItem } from './GridLinkItem';
import { ILink, IRole, IGroup } from 'src/app/api';
import { CheckboxEditor } from '../checkbox/checkbox.component';
import { CheckboxRenderer } from '../cell-renderers/checkbox/checkbox.component';
import { HttpService } from 'src/app/services/http.service';

@Component({
  selector: 'app-admin-links',
  templateUrl: './admin-links.component.html',
  styleUrls: ['./admin-links.component.scss']
})
export class AdminLinksComponent implements OnInit {
  allLinks: ILink[] = [];
  allRoles: IRole[] = [];
  allGroups: IGroup[] = [];
  rowData: GridLinkItem[] = [];

  gridColumns: ColDef[] = [];
  linkGroup: string | null = null;
  linkName: string = '';
  linkUrl: string = '';
  gridApi!: GridApi;
  get isAddDisabled(): boolean {
    return this.linkGroup == '__initial_invalid' || !this.linkName || !this.linkUrl;
  }

  private UpdateLink(link: GridLinkItem, role: string, newValue: boolean) {
    //Yeah, newValue is a boolean, but let's just be explicit
    if (newValue === true) {
      firstValueFrom(this.http.Admin.adminAddLinkRoleLinkIdRolePost(link.id, role));
    }
    else {
      firstValueFrom(this.http.Admin.adminDeleteLinkRoleLinkIdRoleDelete(link.id, role));
    }
  }
  onGridReady(event: GridReadyEvent) {
    this.gridApi = event.api;
  }

  private GenerateRoleColumns(): ColDef[] {
    return this.allRoles.map(r => ({
        headerName: r.name || '',
        valueGetter: (params: ValueGetterParams) => {
          console.log(params.data);
          return params.data.roles[r.name || ''];
        },
        valueSetter: (params: ValueSetterParams) => {
          const data = <GridLinkItem>params.data;
          data.roles[r.name || ''] = <boolean>params.newValue;
          //I believe 'true' means successful update, need to check
          return true;
        },
        sortable: false,
        filter: false,
        editable: true,
        resizable: true,
        cellEditor: CheckboxEditor,
        cellRenderer: CheckboxRenderer,
        onCellValueChanged: event => {
            this.UpdateLink(event.data, r.name || '', event.newValue);
        },
      }
    ))
  }
  private staticColumnDefs: ColDef[] = [
    { valueGetter: _ => '❌', width: 60, sortable: true, filter: true, onCellClicked: (event: CellClickedEvent) => { this.deleteLink(event.data) } },
    { field: 'group', sortable: true, filter: true },
    { field: 'name', sortable: true, filter: true },
    { field: 'url', sortable: true, filter: true },
  ];
  private generateColumnDefs(): ColDef[] {
    return [
      ...this.staticColumnDefs,
      ...this.GenerateRoleColumns()
    ];
  }
  constructor(private http: HttpService) { }

  createRoleDictionaryForLink(linkRoles: string[]): { [key: string]: boolean } {
    return this.allRoles.reduce<{ [key: string]: boolean }>((result, role) => {
      result[role.name || ''] = linkRoles.includes(role.name || '');
      return result;
    }, {});
  }

  async ngOnInit(): Promise<void> {
    await this.loadData();
  }
  private async loadData() {
    [this.allLinks, this.allRoles, this.allGroups] = await Promise.all([
      firstValueFrom(this.http.Admin.adminAllLinksGet()),
      firstValueFrom(this.http.Admin.adminRolesGet()),
      firstValueFrom(this.http.Admin.adminGroupsGet())
    ]);
    if (this.linkGroup == null && this.allGroups.length > 0) {
      this.linkGroup = this.allGroups[0].name||'';
    }
    this.rowData = this.allLinks.map(link => ({
      id: link.id || -1,
      url: link.url || '',
      name: link.name || '',
      roles: this.createRoleDictionaryForLink(link.roles?.map(r => r.name || '') || []),
      group: link.group?.name || '',
    }));
    this.gridColumns = this.generateColumnDefs();
    setTimeout(() => this.gridApi.sizeColumnsToFit(), 10);
  }

  async addLink() {
    await firstValueFrom(this.http.Admin.adminAddLinkPost({
      group: this.linkGroup,
      roles: [],
      name: this.linkName,
      url: this.linkUrl
    }));
    this.linkGroup = '';
    this.linkName = '';
    this.linkUrl = '';
    await this.loadData();
  }
  async deleteLink(link: ILink) {
    this.linkGroup = link.group?.name || '';
    this.linkName = link.name || '';
    this.linkUrl = link.url || '';
    await firstValueFrom(this.http.Admin.adminDeleteLinkLinkIdDelete(link.id||-1));
    await this.loadData();
  }
}
