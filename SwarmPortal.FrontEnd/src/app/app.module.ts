import { HttpClient, HttpClientModule } from '@angular/common/http';
import { APP_INITIALIZER, NgModule } from '@angular/core';
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
import { NavigationComponent } from './navigation/navigation.component';
import { LoginCompleteComponent } from './login-complete/login-complete.component';
import { FormsModule } from '@angular/forms';
import { NgxMasonryModule } from 'ngx-masonry';
import { HttpService } from './services/http.service';


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
    ApiModule.forRoot(apiConfigFactory),
    NgxMasonryModule
  ],
  providers: [
    CookieService,
    {  
      provide: APP_INITIALIZER,
      useFactory: (http: HttpService) => () => http.SetupAuth(),
      deps: [HttpService],
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
