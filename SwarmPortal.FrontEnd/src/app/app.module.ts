import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { environment } from 'src/environments/environment';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { StatusGroupCardComponent } from './status-group-card/status-group-card.component';
import { LinkGroupCardComponent } from './link-group-card/link-group-card.component';
import { StatusScreenComponent } from './status-screen/status-screen.component';
import { OAuthModule } from 'angular-oauth2-oidc';
import { ApiModule, Configuration, ConfigurationParameters } from './api';
import { CookieService } from 'ngx-cookie-service';
import { AdminLinksComponent } from './admin-links/admin-links.component';
import { AdminRolesComponent } from './admin-roles/admin-roles.component';
import { AdminGroupsComponent } from './admin-groups/admin-groups.component';
import { NavigationComponent } from './navigation/navigation.component';
import { LoginCompleteComponent } from './login-complete/login-complete.component';
import { FormsModule } from '@angular/forms';


export function apiConfigFactory (): Configuration {
  const params: ConfigurationParameters = {
    basePath: environment.apiRoot,
    // withCredentials: true,
    // credentials: {
    //   accessToken: () => localStorage.getItem('access_token') || ''
    // }
  }
  return new Configuration({...params});
}


@NgModule({
  declarations: [
    AppComponent,
    StatusGroupCardComponent,
    LinkGroupCardComponent,
    StatusScreenComponent,
    AdminLinksComponent,
    AdminRolesComponent,
    AdminGroupsComponent,
    NavigationComponent,
    LoginCompleteComponent
  ],
  imports: [
    FormsModule,
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    HttpClientModule,
    OAuthModule.forRoot(),
    ApiModule.forRoot(apiConfigFactory)
  ],
  providers: [CookieService],
  bootstrap: [AppComponent]
})
export class AppModule { }
