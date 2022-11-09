import type { OpenAPIConfig } from "./openapi/core/OpenAPI";
import jwt_decode, { JwtPayload } from "jwt-decode";
import { OidcClientSettings, OidcClient } from 'oidc-client-ts';
import { internalClient } from "./openapi";
import { ObjectGraphReconstructionRequest } from "./ObjectGraphReconstructingRequest";

export interface JwtToken extends JwtPayload {
    acr: string;
    at_hash: string;
    aud: string;
    auth_time: number;
    azp: string;
    email: string;
    email_verified: boolean;
    exp: number;
    family_name: string;
    given_name: string;
    iat: number;
    iss: string;
    jti: string;
    name: string;
    nonce: string;
    preferred_username: string;
    roles: string[];
    session_state: string;
    sub: string;
    typ: string;
}
const tokenKey = "swarmportalAuth";
export class ApiClient extends internalClient {

  headers: Record<string, string>;
  private _authConfiguredAndLoaded: boolean = false;
  oidcConfig!: OidcClientSettings;
  oidcClient!: OidcClient;
  constructor(config?: Partial<OpenAPIConfig>) {
    const token = localStorage.getItem(tokenKey);
    const headers: Record<string, string> = {};
    if (token) {
      headers["Authorization"] = `Bearer ${token}`;
    }
    const url = "http://localhost:5109";
    //change conf to commented line for beta.encora.it building
    const conf = { ...{ BASE: url, HEADERS: headers }, ...config };
    super(conf, ObjectGraphReconstructionRequest);
    this.headers = headers;
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
