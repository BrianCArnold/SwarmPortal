import { Component, OnInit } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { HttpService } from '../services/http.service';

@Component({
  selector: 'app-admin-roles',
  templateUrl: './admin-roles.component.html',
  styleUrls: ['./admin-roles.component.scss']
})
export class AdminRolesComponent implements OnInit {
  roles: string[] = [];
  roleName: string = "";

  constructor(private http: HttpService) { }

  async ngOnInit(): Promise<void> {
    this.roles = await firstValueFrom(this.http.Admin.adminRolesGet())
  }
  async deleteRole(role: string) {
    this.roleName = role;
    await firstValueFrom(this.http.Admin.adminDeleteRoleRoleDelete(role));
    this.roles = await firstValueFrom(this.http.Admin.adminRolesGet())
  }
  async addRole(role: string) {
    await firstValueFrom(this.http.Admin.adminAddRoleRolePost(role));
    this.roleName = '';
    this.roles = await firstValueFrom(this.http.Admin.adminRolesGet())
  }
}
