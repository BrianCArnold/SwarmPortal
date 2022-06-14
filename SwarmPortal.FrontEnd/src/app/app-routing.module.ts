import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StatusScreenComponent } from './status-screen/status-screen.component';

const routes: Routes = [
  {
    path: '',
    component: StatusScreenComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
