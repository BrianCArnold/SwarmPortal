import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { environment } from 'src/environments/environment';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { StatusGroupCardComponent } from './status-group-card/status-group-card.component';
import { LinkGroupCardComponent } from './link-group-card/link-group-card.component';
import { StatusScreenComponent } from './status-screen/status-screen.component';
import { OAuthModule } from 'angular-oauth2-oidc';
import { ApiModule, Configuration, ConfigurationParameters } from './api';
import { CookieService } from 'ngx-cookie-service';


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
    StatusScreenComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    OAuthModule.forRoot(),
    ApiModule.forRoot(apiConfigFactory)
  ],
  providers: [CookieService],
  bootstrap: [AppComponent]
})
export class AppModule { }
