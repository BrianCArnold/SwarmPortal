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
  private _identity: IdentityClaims | null = null;
  private _token: string | null = null;

  _isAuthUpdated: boolean = false;
  public get Auth(): AuthService {
    if (!this._isAuthUpdated && this.IsLoggedIn) {
      this.auth.defaultHeaders = this.auth.defaultHeaders.set('Authorization', 'Bearer ' + this.token);
      this._isAuthUpdated = true;
    }
    return this.auth;
  }


  _isLinksUpdated: boolean = false;
  public get Links(): LinksService {
    if (!this._isLinksUpdated && this.IsLoggedIn) {
      this.linksService.defaultHeaders = this.linksService.defaultHeaders.set('Authorization', 'Bearer ' + this.token);
      this._isLinksUpdated = true;
    }
    return this.linksService;
  }

  _isStatusesUpdated: boolean = false;
  public get Statuses(): StatusesService {
    if (!this._isStatusesUpdated && this.IsLoggedIn) {
      this.statusesService.defaultHeaders = this.statusesService.defaultHeaders.set('Authorization', 'Bearer ' + this.token);
      this._isStatusesUpdated = true;
    }
    return this.statusesService;
  }

  constructor(private oauth: OAuthService,
    private auth: AuthService,
    private linksService: LinksService,
    private statusesService: StatusesService) { }

  public get token(): string {
    if (this.IsLoggedIn && this._token != null) {
      return this._token;
    } else {
      throw new Error("User not logged in");
    }
  }
  public get identity(): IdentityClaims {
    if (this.IsLoggedIn && this._identity != null) {
      return this._identity;
    } else {
      throw new Error("User not logged in");
    }
  }

  public async LogIn(){
    if (!this.IsLoggedIn) {
      this.oauth.initCodeFlow();
    }
  }
  public get IsLoggedIn(): boolean {

    if (this._identity == null && this._token == null) {//Not set in memory
      const identityFromStorage = localStorage.getItem('identity');
      const tokenFromStorage = localStorage.getItem('token');
      if (identityFromStorage != null && tokenFromStorage != null) {
        const identityObj = JSON.parse(identityFromStorage);
        if (identityObj.exp > Date.now() / 1000) {
          localStorage.removeItem('identity');
          localStorage.removeItem('token');
          return false;
        }
        else {
          this._identity = identityObj;
          this._token = tokenFromStorage;
          return true;
        }
      }
      else {
        this._identity = <IdentityClaims>this.oauth.getIdentityClaims();
        this._token = this.oauth.getAccessToken();
        if (this._identity != null && this._token != null) {
          localStorage.setItem('identity', JSON.stringify(this._identity));
          localStorage.setItem('token', this._token);
          return true;
        } else {
          return false;
        }
      }
    } else { //already set in memory.
      return true;
    }
  }


  private _isAuthLoaded: boolean = false;
  private async loadAuth(): Promise<boolean> {
    if (!this._isAuthLoaded) {
      await this.configureOAuth();
      this.oauth.tokenValidationHandler = new JwksValidationHandler();
      await this.oauth.loadDiscoveryDocument();
      console.warn("Assuming that the discovery document is loaded.");
      this._isAuthLoaded = true;
    }
    return this._isAuthLoaded;
  }

  private _authConfig: AuthConfig | null = null;
  private async configureOAuth() {
    if (this._authConfig == null) {
      this._authConfig = await this.getAuthConfig();
      this.oauth.configure({
        issuer: this._authConfig.issuer || "",
        clientId: this._authConfig.clientId || "",
        redirectUri: this._authConfig.redirectUri || "",
        scope: this._authConfig.scope || "",
        requireHttps: this._authConfig.requireHttps,
        responseType: 'code'
      });
    }
  }

  private async getAuthConfig() {
    return await firstValueFrom(this.auth.authConfigGet());
  }
}
