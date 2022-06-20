import { Component, OnInit } from '@angular/core';
import { first, firstValueFrom } from 'rxjs';
import { IRole } from 'src/app/api';
import { HttpService } from 'src/app/services/http.service';

@Component({
  selector: 'app-admin-roles',
  templateUrl: './admin-roles.component.html',
  styleUrls: ['./admin-roles.component.scss']
})
export class AdminRolesComponent implements OnInit {
  roleName: string = "";
  detectedRoles!: string[];
  allRoles!: IRole[];
  enabledRoles!: IRole[];
  disabledRoles!: IRole[];

  constructor(private http: HttpService) {
  }

  async ngOnInit(): Promise<void> {
    await this.loadRoles();
  }
  private async loadRoles() {
    this.allRoles = await firstValueFrom(this.http.Admin.adminRolesGet());
    this.enabledRoles = await firstValueFrom(this.http.Admin.adminEnabledRolesWithNoLinksGet());
    this.disabledRoles = await firstValueFrom(this.http.Admin.adminDisabledRolesGet());
    const existingRoles = this.allRoles.map(ro => ro.name);
    this.detectedRoles = (this.http.Identity?.roles || []).filter(r => !existingRoles.includes(r));
  }

  async disableRole(role: IRole) {
    await firstValueFrom(this.http.Admin.adminDisableRoleRoleIdDelete(role.id || -1));
    await this.loadRoles();
  }
  async enableRole(role:IRole) {
    await firstValueFrom(this.http.Admin.adminEnableRoleRoleIdPut(role.id || -1));
    await this.loadRoles();
  }
  async addRole(role: string) {
    await firstValueFrom(this.http.Admin.adminAddRoleRolePost(role));
    this.roleName = '';
    await this.loadRoles();
  }
}
