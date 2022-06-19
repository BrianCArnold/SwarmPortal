import { Component, OnInit } from '@angular/core';

import { OAuthService } from 'angular-oauth2-oidc';
import { JwksValidationHandler } from 'angular-oauth2-oidc-jwks';
import { first, firstValueFrom, throttleTime } from 'rxjs';
import { CookieService } from 'ngx-cookie-service';
import { AuthConfig, AuthService, Configuration, LinksService } from './api';
import { HttpService } from './services/http.service';
import { IdentityClaims } from './models/IdentityClaims';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = '@swarm-portal/frontend';

  constructor(private http: HttpService,
    private auth: AuthService,
    private oauth: OAuthService) {
  }

  async ngOnInit(): Promise<void> {

    
  }
}
