import { Component, OnInit } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { IRole } from '../api/model/iRole';
import { HttpService } from '../services/http.service';

@Component({
  selector: 'app-admin-roles',
  templateUrl: './admin-roles.component.html',
  styleUrls: ['./admin-roles.component.scss']
})
export class AdminRolesComponent implements OnInit {
  roles: IRole[] = [];
  roleName: string = "";
  detectedRoles!: string[];

  constructor(private http: HttpService) {
  }




  async ngOnInit(): Promise<void> {
    await this.loadRoles();
  }
  private async loadRoles() {
    this.roles = await firstValueFrom(this.http.Admin.adminRolesGet());
    const existingRoles = this.roles.map(ro => ro.name);
    this.detectedRoles = (this.http.Identity?.roles || []).filter(r => !existingRoles.includes(r));
  }

  async deleteRole(role: IRole) {
    this.roleName = role.name || '';
    await firstValueFrom(this.http.Admin.adminDeleteRoleRoleIdDelete(role.id || -1));
    await this.loadRoles();
  }
  async addRole(role: string) {
    await firstValueFrom(this.http.Admin.adminAddRoleRolePost(role));
    this.roleName = '';
    await this.loadRoles();
  }
}
