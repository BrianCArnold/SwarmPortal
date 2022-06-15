import { Component, OnInit } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { HttpService } from '../services/http.service';
import { IdentityClaims } from '../services/IdentityClaims';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.scss']
})
export class NavigationComponent implements OnInit {

  constructor(private http: HttpService,
    private oauth: OAuthService) { }

  get Identity(): IdentityClaims | null {
    return this.http.Identity;
  }
  get Roles(): string[] {
    return this.http.Identity?.roles || [];
  }
  get isLoggedIn(): boolean {
    return this.http.Identity != null;
  }
  ngOnInit(): void {
  }
  login() {
    this.oauth.initCodeFlow();

  }
  logout() {
    this.oauth.logOut();
  }

}
