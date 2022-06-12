import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LinksResolver } from './resolvers/links.resolver';
import { StatusesResolver } from './resolvers/statuses.resolver';
import { StatusScreenComponent } from './status-screen/status-screen.component';

const routes: Routes = [
  {
    path: '',
    component: StatusScreenComponent,
    resolve: {
      links: LinksResolver,
      statuses: StatusesResolver
    }
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
