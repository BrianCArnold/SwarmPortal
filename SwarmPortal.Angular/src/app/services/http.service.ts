import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { OAuthService, OAuthStorage } from 'angular-oauth2-oidc';
import { JwksValidationHandler } from 'angular-oauth2-oidc-jwks';
import { firstValueFrom, Observable } from 'rxjs';
import { AdminService, AuthService, IconService, ILinkItem, IStatusItem, LinksService, StatusesService } from '../api';
import { IdentityClaims } from '../models/IdentityClaims';
import { SwarmStorage } from './SwarmStorage';

@Injectable({
  providedIn: 'root'
})
export class HttpService {

  private get _noIconDefault(): string | null {
    return this.storage.getItem('noIconDefault');
  }
  private set _noIconDefault(v: string | null) {
    if (v == null) {
      this.storage.removeItem('noIconDefault');
    } else {
      this.storage.setItem('noIconDefault', v);
    }
  }
  public GetNoIconDefault(): Observable<string> {
    return new Observable<string>((observer) => {
      if (this._noIconDefault == null) {
        const reader = new FileReader();
        reader.onloadend = _ => {
          let res = reader.result;
          if (typeof(res) === 'string') {
            this._noIconDefault = res;
            observer.next(res);
          }
        };
        this.http
          .get(this.linksService.configuration.basePath + "/Icon/Failure", { responseType: 'blob' })
          .subscribe(response => reader.readAsDataURL(response));
      }
      else {
        observer.next(this._noIconDefault);
      }
    });
  }

  public get Admin(): AdminService {
    return this.attachHeaders(this.admin);
  }
  public get Status(): StatusesService{
    return this.attachHeaders(this.statusesService);
  }
  public get Link(): LinksService{
    return this.attachHeaders(this.linksService);
  }
  public get Icon(): IconService{
    return this.attachHeaders(this.iconService);
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
  public async processLogin(): Promise<void> {
    firstValueFrom(this.attachHeaders(this.auth).authProcessLoginGet());
  }

  private attachHeaders<TService extends {defaultHeaders: HttpHeaders}>(service: TService): TService {
    service.defaultHeaders = service.defaultHeaders.set('Authorization', 'Bearer ' + this.Token);
    return service;
  }

  constructor(private oauth: OAuthService,
    private auth: AuthService,
    private linksService: LinksService,
    private statusesService: StatusesService,
    private iconService: IconService,
    private admin: AdminService,
    private http: HttpClient,
    private storage: SwarmStorage) { }

  private _isTokenLoadAttempted = false;
  private _token: string | null = null;
  public get Token(): string | null {
    if (!this._isTokenLoadAttempted) {
      var accToken = this.oauth.getAccessToken();
      if (accToken) {
        this.storage.setItem('token', accToken);
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
        this.storage.setItem('identity', JSON.stringify(claims));
      }
      this._identity = claims;
      this._isIdentityLoadAttempted = true;
    }
    return this._identity;
  }
}
