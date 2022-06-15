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

  public SetAuth(token: string, identity: IdentityClaims) {
    console.log(token);
    console.log(identity);
    this.Identity = identity;
    this.Token = token;
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
    return JSON.parse(localStorage.getItem('identity')||'') || null;
  }
  public set Identity(v: IdentityClaims | null) {
    localStorage.setItem('identity', JSON.stringify(v));
  }
}
