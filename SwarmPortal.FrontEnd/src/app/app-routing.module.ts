import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminGroupsComponent } from './admin-groups/admin-groups.component';
import { AdminLinksComponent } from './admin-links/admin-links.component';
import { AdminRolesComponent } from './admin-roles/admin-roles.component';
import { AdminGuard } from './guards/admin.guard';
import { LoginCompleteComponent } from './login-complete/login-complete.component';
import { StatusScreenComponent } from './status-screen/status-screen.component';

const routes: Routes = [
  {
    path: '',
    component: StatusScreenComponent
  },
  {
    path: 'Login',
    component: LoginCompleteComponent
  },
  {
    path: 'admin/links',
    component: AdminLinksComponent,
    canActivate: [AdminGuard]
  },
  {
    path: 'admin/roles',
    component: AdminRolesComponent,
    canActivate: [AdminGuard]
  },
  {
    path: 'admin/groups',
    component: AdminGroupsComponent,
    canActivate: [AdminGuard]
  },

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
