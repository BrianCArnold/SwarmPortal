import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminGroupsComponent } from './admin-groups/admin-groups.component';
import { AdminLinksComponent } from './admin-links/admin-links.component';
import { AdminRolesComponent } from './admin-roles/admin-roles.component';
import { AdminGuard } from './guards/admin.guard';


export const routes: Routes = [
  {
    path: 'groups',
    component: AdminGroupsComponent,
    canActivate: [AdminGuard]
  },
  {
    path: 'links',
    component: AdminLinksComponent,
    canActivate: [AdminGuard]
  },
  {
    path: 'roles',
    component: AdminRolesComponent,
    canActivate: [AdminGuard]
  }

];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule {
}
