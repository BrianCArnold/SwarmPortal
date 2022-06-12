import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { environment } from 'src/environments/environment';
import { ApiModule, Configuration, ConfigurationParameters } from './api-client';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';


export function apiConfigFactory(): Configuration {
  const params: ConfigurationParameters = {
    basePath: environment.apiRoot
  }
  return new Configuration(params);
}


@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ApiModule.forRoot(apiConfigFactory),
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
