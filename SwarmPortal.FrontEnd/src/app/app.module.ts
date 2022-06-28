import { HttpClientModule } from '@angular/common/http';
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
import { HttpService } from './services/http.service';
import { SecurePipe } from './pipes/secure.pipe';


export function apiConfigFactory (): Configuration {
  const params: ConfigurationParameters = {
    basePath: environment.apiRoot
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
    LoginCompleteComponent,
    SecurePipe
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
  providers: [
    CookieService,
    {
      provide: APP_INITIALIZER,
      useFactory: (http: HttpService) => async () => await http.SetupAuth(),
      deps: [HttpService],
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
