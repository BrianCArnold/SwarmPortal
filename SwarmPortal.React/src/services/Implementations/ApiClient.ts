import type { OpenAPIConfig } from "../openapi/core/OpenAPI";
import jwt_decode from "jwt-decode";
import { OidcClientSettings, OidcClient } from 'oidc-client-ts';
import { internalClient } from "../openapi";
import { NewtonsoftRefReconstructingHttpRequest } from "./ObjectGraphReconstructingRequest";
import { JwtToken } from "../../Models/JwtToken";
import { IApiClient } from "../Interfaces/IApiClient";

const tokenKey = "swarmportalAuth";

export class ApiClient extends internalClient implements IApiClient {

  private _authConfiguredAndLoaded: boolean = false;
  private oidcConfig!: OidcClientSettings;
  private oidcClient!: OidcClient;
  constructor(config?: Partial<OpenAPIConfig>) {
    const token = localStorage.getItem(tokenKey);
    const headers: Record<string, string> = {};
    if (token) {
      headers["Authorization"] = `Bearer ${token}`;
    }
    const url = "http://localhost:5109";
    //change conf to commented line for beta.encora.it building
    const conf = { ...{ BASE: url, HEADERS: headers }, ...config };
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
        scope: _authConfig.scope || "",
        response_type: 'code'
      };
      this.oidcClient = new OidcClient(this.oidcConfig);
      return this.oidcClient;
    } else {
        return this.oidcClient;
    }
  }
  get token(): JwtToken {
    return jwt_decode<JwtToken>(localStorage.getItem(tokenKey) || "");
  }
  get roles(): string[] {
    console.log(this.token.roles);
    return this.token.roles;
  }
  get isLoggedIn(): boolean {
    return !!localStorage.getItem(tokenKey);
  }
  async login(){
    const client = await this.GetOidcClient();
    const signinRequest = await client.createSigninRequest({});
    window.location.href = signinRequest.url;
  }
  async processLogIn() {
    const client = await this.GetOidcClient();
    const signinResponse = await client.processSigninResponse(window.location.href);
    localStorage.setItem(tokenKey, signinResponse.access_token);
  }
  async logOut() {
    const client = await this.GetOidcClient();
    const signoutRequest = await client.createSignoutRequest({
        post_logout_redirect_uri: "http://localhost:3000/Logout"
    });
    window.location.href = signoutRequest.url;
  }
  async processLogOut() {
    const client = await this.GetOidcClient();
    await client.processSignoutResponse(window.location.href);
    localStorage.removeItem(tokenKey);
  }

}
