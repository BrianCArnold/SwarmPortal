import { Component, OnInit } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { IGroup } from '../api/model/iGroup';
import { HttpService } from '../services/http.service';

@Component({
  selector: 'app-admin-groups',
  templateUrl: './admin-groups.component.html',
  styleUrls: ['./admin-groups.component.scss']
})
export class AdminGroupsComponent implements OnInit {
  groups: IGroup[] = [];
  groupName: string = "";

  constructor(private http: HttpService) { }

  async ngOnInit(): Promise<void> {
    this.groups = await firstValueFrom(this.http.Admin.adminGroupsGet())
  }
  async deleteGroup(group: IGroup) {
    this.groupName = group.name || '';
    await firstValueFrom(this.http.Admin.adminDeleteGroupGroupIdDelete(group.id || -1));
    this.groups = await firstValueFrom(this.http.Admin.adminGroupsGet())
  }
  async addGroup(group: string) {
    await firstValueFrom(this.http.Admin.adminAddGroupGroupPost(group));
    this.groupName = '';
    this.groups = await firstValueFrom(this.http.Admin.adminGroupsGet())
  }

}
