import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminGroupsComponent } from './admin-groups/admin-groups.component';
import { AdminLinksComponent } from './admin-links/admin-links.component';
import { AdminRolesComponent } from './admin-roles/admin-roles.component';
import { StatusScreenComponent } from './status-screen/status-screen.component';

const routes: Routes = [
  {
    path: '',
    component: StatusScreenComponent
  },
  {
    path: 'admin/links',
    component: AdminLinksComponent
  },
  {
    path: 'admin/roles',
    component: AdminRolesComponent
  },
  {
    path: 'admin/groups',
    component: AdminGroupsComponent
  },

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
