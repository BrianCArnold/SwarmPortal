import { Component, OnInit } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { IGroup } from '../../api/model/iGroup';
import { HttpService } from '../../services/http.service';

@Component({
  selector: 'app-admin-groups',
  templateUrl: './admin-groups.component.html',
  styleUrls: ['./admin-groups.component.scss']
})
export class AdminGroupsComponent implements OnInit {
  enabledGroups!: IGroup[];
  groupName: string = "";
  disabledGroups!: IGroup[];

  constructor(private http: HttpService) { }

  async ngOnInit(): Promise<void> {
    await this.loadGroups();
  }
  private async loadGroups() {
    this.enabledGroups = await firstValueFrom(this.http.Admin.adminEnabledGroupsWithNoLinksGet());
    this.disabledGroups = await firstValueFrom(this.http.Admin.adminDisabledGroupsGet());
  }

  async disableGroup(group: IGroup) {
    await firstValueFrom(this.http.Admin.adminDisableGroupGroupIdDelete(group.id || -1));
    await this.loadGroups();
  }
  async enableGroup(group: IGroup) {
    await firstValueFrom(this.http.Admin.adminEnableGroupGroupIdPut(group.id || -1));
    await this.loadGroups();
  }
  async addGroup(group: string) {
    await firstValueFrom(this.http.Admin.adminAddGroupGroupPost(group));
    this.groupName = '';
    await this.loadGroups();
  }

}
