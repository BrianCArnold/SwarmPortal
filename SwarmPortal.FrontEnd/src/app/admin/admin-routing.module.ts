import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminGroupsComponent } from './admin-groups/admin-groups.component';
import { AdminLinksComponent } from './admin-links/admin-links.component';
import { AdminRolesComponent } from './admin-roles/admin-roles.component';


export const routes: Routes = [
  {
    path: 'groups',
    component: AdminGroupsComponent
  },
  {
    path: 'links',
    component: AdminLinksComponent
  },
  {
    path: 'roles',
    component: AdminRolesComponent
  }

];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule {
}
