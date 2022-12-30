import jwt_decode from "jwt-decode";
import { OidcClientSettings, OidcClient } from 'oidc-client-ts';
import { internalClient } from "../openapi";
import { NewtonsoftRefReconstructingHttpRequest } from "./ObjectGraphReconstructingRequest";
import { JwtToken } from "../../Models/JwtToken";
import { IApiClient } from "../Interfaces/IApiClient";
import { IApiConfiguration } from "../Interfaces/IApiConfiguration";
import { injectable } from 'inversify';

//A couple parts taken from a project I'm helping another developer with, Not entirely my code.
@injectable()
export class ApiClient extends internalClient implements IApiClient {
  get tokenKey(): string {
    return tokenKey;
  }
  private _authConfiguredAndLoaded: boolean = false;
  private oidcConfig!: OidcClientSettings;
  private oidcClient!: OidcClient;
  constructor(
    private config: IApiConfiguration
  ) {
    const token = localStorage.getItem(tokenKey);
    const headers: Record<string, string> = {};
    if (token) {
      headers["Authorization"] = `Bearer ${token}`;
    }
    const conf = { ...{ HEADERS: headers }, ...config };
    super(conf, NewtonsoftRefReconstructingHttpRequest);
  }

  private async GetOidcClient(): Promise<OidcClient> {
    if (!this._authConfiguredAndLoaded) {
      const _authConfig = await this.auth.getAuthConfig();
      console.log(_authConfig);
      this.oidcConfig = {
        authority: _authConfig.authority || "",
        client_id: _authConfig.clientId || "",
        redirect_uri: _authConfig.redirectUri + "/Login" || "",
        post_logout_redirect_uri: _authConfig.redirectUri + "/Logout" || "",
        scope: _authConfig.scope || "",
        response_type: 'code'
      };
      this.oidcClient = new OidcClient(this.oidcConfig);
      return this.oidcClient;
    } else {
        return this.oidcClient;
    }
  }
  
  get rawToken(): string {
    return localStorage.getItem(this.tokenKey) || "";
  }
  get token(): JwtToken {
    const result = jwt_decode<JwtToken>(localStorage.getItem(this.tokenKey) || "");
    if (result && result.exp) {
      const epoch = new Date(0);
      epoch.setUTCSeconds(result.exp);
      if (new Date().getTime() > epoch.getTime()) {
        return result;
      } else {
        return jwt_decode<JwtToken>('');
      }
    }
    return result;
  }
  get roles(): string[] {
    console.log(this.token.roles);
    return this.token.roles;
  }
  get isLoggedIn(): boolean {
    return !!localStorage.getItem(this.tokenKey);
  }
  async login(){
    const client = await this.GetOidcClient();
    const signinRequest = await client.createSigninRequest({});
    window.location.href = signinRequest.url;
  }
  async processLogIn() {
    const client = await this.GetOidcClient();
    const signinResponse = await client.processSigninResponse(window.location.href);
    localStorage.setItem(this.tokenKey, signinResponse.access_token);
  }
  async logOut() {
    const client = await this.GetOidcClient();
    const signoutRequest = await client.createSignoutRequest();
    window.location.href = signoutRequest.url;
  }
  async processLogOut() {
    const client = await this.GetOidcClient();
    await client.processSignoutResponse(window.location.href);
    localStorage.removeItem(this.tokenKey);
  }

}
const tokenKey = "swarmportalAuth";
