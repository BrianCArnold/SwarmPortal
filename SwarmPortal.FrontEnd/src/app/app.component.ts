import { Component, OnInit } from '@angular/core';

import { OAuthService } from 'angular-oauth2-oidc';
import { JwksValidationHandler } from 'angular-oauth2-oidc-jwks';
import { first, firstValueFrom } from 'rxjs';
import { CookieService } from 'ngx-cookie-service';
import { AuthService, Configuration, LinksService } from './api';
import { UserService } from './services/user.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = '@swarm-portal/frontend';

  constructor(private userService: UserService) {
  }
  async ngOnInit(): Promise<void> {
    if (this.userService.IsLoggedIn) {
      console.log(this.userService.identity);
      console.log(this.userService.token);
    }
    else {
      console.log("Not logged in");
    }

    // var authConfiguration = await new AuthApi(config).authConfigGet();



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
    this.userService.LogIn();

  }
}
