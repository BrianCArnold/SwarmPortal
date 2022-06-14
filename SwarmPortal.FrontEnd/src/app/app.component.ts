import { Component, OnInit } from '@angular/core';

import { OAuthService } from 'angular-oauth2-oidc';
import { JwksValidationHandler } from 'angular-oauth2-oidc-jwks';
import { first, firstValueFrom } from 'rxjs';
import { CookieService } from 'ngx-cookie-service';
import { AuthService, Configuration, LinksService } from './api';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = '@swarm-portal/frontend';

  constructor(private oauthService: OAuthService,
    private a: AuthService,
    private links: LinksService,
    private cookies: CookieService) {
  }
  async ngOnInit(): Promise<void> {
    var authConf = await firstValueFrom(this.a.authConfigGet());


    // var authConfiguration = await new AuthApi(config).authConfigGet();


    this.oauthService.configure({
      issuer: authConf.issuer || "",
      clientId: authConf.clientId || "",
      redirectUri: authConf.redirectUri || "",
      scope: authConf.scope || "",
      requireHttps: authConf.requireHttps,
      responseType: 'code'
    });
    this.oauthService.tokenValidationHandler =
      new JwksValidationHandler();

    await this.oauthService.loadDiscoveryDocumentAndTryLogin();

    /*

      Next Steps: Need to store the token in local storage.
      Check if it's not valid coming back from the server, then refresh it.
      Create API endpoints to add Roles and Links.
      Check swarm node labels to check for roles.
      Switch out to MySQL, Postgres, or MSSQL.

    */
    // this.oauthService.initCodeFlow();
  }
  login() {
    this.oauthService.initCodeFlow();

  }
}
