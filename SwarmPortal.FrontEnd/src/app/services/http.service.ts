import { HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { JwksValidationHandler } from 'angular-oauth2-oidc-jwks';
import { firstValueFrom } from 'rxjs';
import { AdminService, AuthConfig, AuthService, LinksService, StatusesService } from '../api';
import { IdentityClaims } from './IdentityClaims';

@Injectable({
  providedIn: 'root'
})
export class HttpService {

  public get Admin(): AdminService {
    return this.attachHeaders(this.admin);
  }

  public get Auth(): AuthService {
    return this.attachHeaders(this.auth);
  }

  public get Links(): LinksService {
    return this.attachHeaders(this.linksService);
  }

  public get Statuses(): StatusesService {
    return this.attachHeaders(this.statusesService);
  }

  public async SetupAuth(): Promise<void> {
    const _authConfig = await firstValueFrom(this.auth.authConfigGet());;
    this.oauth.configure({
      issuer: _authConfig.issuer || "",
      clientId: _authConfig.clientId || "",
      redirectUri: _authConfig.redirectUri + "/Login" || "",
      scope: _authConfig.scope || "",
      requireHttps: _authConfig.requireHttps,
      responseType: 'code'
    });
    this.oauth.tokenValidationHandler = new JwksValidationHandler();
    await this.oauth.loadDiscoveryDocumentAndTryLogin();
    var accToken = this.oauth.getAccessToken();
    var claims = <IdentityClaims>this.oauth.getIdentityClaims();
    if (accToken && claims) {
      this.Identity = claims;
      this.Token = accToken;
    }
  }

  private attachHeaders<TService extends {defaultHeaders: HttpHeaders}>(service: TService): TService {
    service.defaultHeaders = service.defaultHeaders.set('Authorization', 'Bearer ' + this.Token);
    return service;
  }

  constructor(private oauth: OAuthService,
    private auth: AuthService,
    private linksService: LinksService,
    private statusesService: StatusesService,
    private admin: AdminService) { }

  public get Token(): string{
    return localStorage.getItem('token') || '';
  }
  public set Token(v: string | null) {
    localStorage.setItem('token', v || '');
  }
  public get Identity(): IdentityClaims | null {
    var identityJson = localStorage.getItem('identity');
    if (identityJson) {
      return JSON.parse(identityJson);
    }
    else {
      return null;
    }
  }
  public set Identity(v: IdentityClaims | null) {
    localStorage.setItem('identity', JSON.stringify(v));
  }
}
