import { HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { JwksValidationHandler } from 'angular-oauth2-oidc-jwks';
import { firstValueFrom, Observable } from 'rxjs';
import { AdminService, AuthConfig, AuthService, ILinkItem, IStatusItem, LinksService, StatusesService } from '../api';
import { IdentityClaims } from './IdentityClaims';

@Injectable({
  providedIn: 'root'
})
export class HttpService {

  public get Admin(): AdminService {
    return this.attachHeaders(this.admin);
  }
  public get Status(): StatusesService{
    return this.attachHeaders(this.statusesService);
  } 
  public get Link(): LinksService{
    return this.attachHeaders(this.linksService);
  }
  
  public GetStatuses(): Observable<{ [key: string]: IStatusItem[]; }> {
    return this.Identity != null ? this.Status.statusesAllGet() : this.Status.statusesPublicGet();
  }
  public GetLinks(): Observable<{ [key: string]: ILinkItem[]; }> {
    return this.Identity != null ? this.Link.linksAllGet() : this.Link.linksPublicGet();
  }

  private _authConfiguredAndLoaded: boolean = false;

  public async SetupAuth(): Promise<void> {
    if (!this._authConfiguredAndLoaded) {
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

  private _isTokenLoadAttempted = false;
  private _token: string | null = null;
  public get Token(): string | null {
    if (!this._isTokenLoadAttempted) {
      var accToken = this.oauth.getAccessToken();
      if (accToken) {
        localStorage.setItem('token', accToken);
      }
      this._token = accToken;
      this._isTokenLoadAttempted = true;
    }
    return this._token;
  }

  
  private _isIdentityLoadAttempted = false;
  private _identity: IdentityClaims | null = null;
  public get Identity(): IdentityClaims | null {
    if (!this._isIdentityLoadAttempted) {
      var claims = <IdentityClaims>this.oauth.getIdentityClaims();
      if (claims) {
        localStorage.setItem('identity', JSON.stringify(claims));
      }
      this._identity = claims;
      this._isIdentityLoadAttempted = true;
    }
    return this._identity;
  }
}
