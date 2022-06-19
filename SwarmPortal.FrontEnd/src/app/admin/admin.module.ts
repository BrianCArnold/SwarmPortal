import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminGroupsComponent } from './admin-groups/admin-groups.component';
import { AdminRolesComponent } from './admin-roles/admin-roles.component';
import { AdminLinksComponent } from './admin-links/admin-links.component';
import { AdminRoutingModule } from './admin-routing.module';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';
import { AgGridModule } from 'ag-grid-angular';
import { OAuthModule } from 'angular-oauth2-oidc';
import { ApiModule } from '../api';
import { apiConfigFactory } from '../app.module';
import { CheckboxEditor } from './checkbox/checkbox.component';
import { DeleteRenderer } from './cell-renderers/delete/delete.component';
import { CheckboxRenderer } from './cell-renderers/checkbox/checkbox.component';


@NgModule({
  declarations: [
    AdminGroupsComponent,
    AdminRolesComponent,
    AdminLinksComponent,
    CheckboxEditor,
    CheckboxRenderer,
    DeleteRenderer
  ],
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,
    AgGridModule.withComponents([CheckboxEditor, CheckboxRenderer, DeleteRenderer]),
    OAuthModule.forRoot(),
    AdminRoutingModule
  ]
})
export class AdminModule { }
