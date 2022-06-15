import { Component, OnInit } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { ILinkItem } from '../api';
import { HttpService } from '../services/http.service';

@Component({
  selector: 'app-admin-links',
  templateUrl: './admin-links.component.html',
  styleUrls: ['./admin-links.component.scss']
})
export class AdminLinksComponent implements OnInit {
  allLinks: ILinkItem[] = [];
  allRoles: string[] = [];
  allGroups: string[] = [];

  linkGroup: string = "";
  linkRoles: string[] = [];
  linkName: string = "";
  linkUrl: string = "";
  constructor(private http: HttpService) { }

  async ngOnInit(): Promise<void> {
    this.allLinks = await firstValueFrom(this.http.Admin.adminAllLinksGet());
    this.allRoles = await firstValueFrom(this.http.Admin.adminRolesGet());
    this.allGroups = await firstValueFrom(this.http.Admin.adminGroupsGet());
  }
  async addLink() {
    await firstValueFrom(this.http.Admin.adminAddLinkPost({
      group: this.linkGroup,
      roles: this.linkRoles,
      name: this.linkName,
      url: this.linkUrl
    }));
    this.linkGroup = '';
    this.linkRoles = [];
    this.linkName = '';
    this.linkUrl = '';
    this.allLinks = await firstValueFrom(this.http.Admin.adminAllLinksGet());
  }
  async deleteLink(link: ILinkItem) {
    this.linkGroup = link.group || '';
    this.linkRoles = link.roles || [];
    this.linkName = link.name || '';
    this.linkUrl = link.url || '';
    await firstValueFrom(this.http.Admin.adminDeleteLinkLinkGroupLinkNameDelete(link.name || '', link.group || ''));
    this.allLinks = await firstValueFrom(this.http.Admin.adminAllLinksGet());
  }
}
