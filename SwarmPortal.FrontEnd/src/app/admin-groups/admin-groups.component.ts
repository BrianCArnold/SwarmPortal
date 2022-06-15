import { Component, OnInit } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { HttpService } from '../services/http.service';

@Component({
  selector: 'app-admin-groups',
  templateUrl: './admin-groups.component.html',
  styleUrls: ['./admin-groups.component.scss']
})
export class AdminGroupsComponent implements OnInit {
  groups: string[] = [];
  groupName: string = "";

  constructor(private http: HttpService) { }

  async ngOnInit(): Promise<void> {
    this.groups = await firstValueFrom(this.http.Admin.adminGroupsGet())
  }
  async deleteGroup(group: string) {
    this.groupName = group;
    await firstValueFrom(this.http.Admin.adminDeleteGroupGroupDelete(group));
    this.groups = await firstValueFrom(this.http.Admin.adminGroupsGet())
  }
  async addGroup(group: string) {
    await firstValueFrom(this.http.Admin.adminAddGroupGroupPost(group));
    this.groupName = '';
    this.groups = await firstValueFrom(this.http.Admin.adminGroupsGet())
  }

}
