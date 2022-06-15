import { Component, OnInit } from '@angular/core';

import { OAuthService } from 'angular-oauth2-oidc';
import { JwksValidationHandler } from 'angular-oauth2-oidc-jwks';
import { first, firstValueFrom, throttleTime } from 'rxjs';
import { CookieService } from 'ngx-cookie-service';
import { AuthConfig, AuthService, Configuration, LinksService } from './api';
import { HttpService } from './services/http.service';
import { IdentityClaims } from './services/IdentityClaims';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = '@swarm-portal/frontend';
  get Identity(): IdentityClaims | null {
    return this.http.Identity;
  }

  constructor(private http: HttpService,
    private auth: AuthService,
    private oauth: OAuthService) {
  }
  get isLoggedIn(): boolean {
    return this.http.Identity != null;
  }

  async ngOnInit(): Promise<void> {

    const _authConfig = await firstValueFrom(this.auth.authConfigGet());;
    this.oauth.configure({
      issuer: _authConfig.issuer || "",
      clientId: _authConfig.clientId || "",
      redirectUri: _authConfig.redirectUri || "",
      scope: _authConfig.scope || "",
      requireHttps: _authConfig.requireHttps,
      responseType: 'code'
    });
    this.oauth.tokenValidationHandler = new JwksValidationHandler();
    await this.oauth.loadDiscoveryDocumentAndTryLogin();
    this.http.SetAuth(this.oauth.getAccessToken(), <IdentityClaims>this.oauth.getIdentityClaims());
  }
  async login() {
    this.oauth.initCodeFlow();

  }
}
