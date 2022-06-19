import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminGuard } from './guards/admin.guard';
import { LoginCompleteComponent } from './login-complete/login-complete.component';
import { LinksResolver } from './resolvers/links.resolver';
import { StatusesResolver } from './resolvers/statuses.resolver';
import { StatusScreenComponent } from './status-screen/status-screen.component';

const routes: Routes = [
  {
    path: '',
    component: StatusScreenComponent,
    resolve: {
      statuses: StatusesResolver,
      links: LinksResolver
    }
  },
  {
    path: 'Login',
    component: LoginCompleteComponent
  },
  { 
    path: 'admin',
    loadChildren: () => import('./admin/admin.module').then(m => m.AdminModule),
  }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
