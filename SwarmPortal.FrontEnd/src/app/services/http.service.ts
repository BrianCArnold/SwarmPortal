import { Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { JwksValidationHandler } from 'angular-oauth2-oidc-jwks';
import { firstValueFrom } from 'rxjs';
import { AuthConfig, AuthService, LinksService, StatusesService } from '../api';
import { IdentityClaims } from './IdentityClaims';

@Injectable({
  providedIn: 'root'
})
export class HttpService {

  _isAuthUpdated: boolean = false;
  public get Auth(): AuthService {
    if (!this._isAuthUpdated && this.Token != null) {
      this.auth.defaultHeaders = this.auth.defaultHeaders.set('Authorization', 'Bearer ' + this.Token);
      this._isAuthUpdated = true;
    }
    return this.auth;
  }

  public SetAuth(token: string, identity: IdentityClaims) {
    console.log(token);
    console.log(identity);
    this.Identity = identity;
    this.Token = token;
  }


  _isLinksUpdated: boolean = false;
  public get Links(): LinksService {
    if (!this._isLinksUpdated && this.Token != null) {
      this.linksService.defaultHeaders = this.linksService.defaultHeaders.set('Authorization', 'Bearer ' + this.Token);
      this._isLinksUpdated = true;
    }
    return this.linksService;
  }

  _isStatusesUpdated: boolean = false;
  public get Statuses(): StatusesService {
    if (!this._isStatusesUpdated && this.Token != null) {
      this.statusesService.defaultHeaders = this.statusesService.defaultHeaders.set('Authorization', 'Bearer ' + this.Token);
      this._isStatusesUpdated = true;
    }
    return this.statusesService;
  }

  constructor(private oauth: OAuthService,
    private auth: AuthService,
    private linksService: LinksService,
    private statusesService: StatusesService) { }

  public get Token(): string{
    return localStorage.getItem('token') || '';
  }
  public set Token(v: string | null) {
    localStorage.setItem('token', v || '');
  }
  public get Identity(): IdentityClaims | null {
    return JSON.parse(localStorage.getItem('identity')||'') || null;
  }
  public set Identity(v: IdentityClaims | null) {
    localStorage.setItem('identity', JSON.stringify(v));
  }
}
