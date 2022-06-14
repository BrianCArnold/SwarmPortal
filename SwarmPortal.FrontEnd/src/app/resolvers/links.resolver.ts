import { Injectable } from '@angular/core';
import {
  Router, Resolve,
  RouterStateSnapshot,
  ActivatedRouteSnapshot
} from '@angular/router';
import { OAuthService } from 'angular-oauth2-oidc';
import { CookieService } from 'ngx-cookie-service';
import { Observable, of } from 'rxjs';
import { ILinkItem, LinksService } from '../api';

@Injectable({
  providedIn: 'root'
})
export class LinksResolver implements Resolve<{ [key: string]: Array<ILinkItem>; }> {
  constructor(public linksService: LinksService, private oauthService: OAuthService, private cookies: CookieService){}
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<{ [key: string]: Array<ILinkItem>; }> {

    var token = this.oauthService.getAccessToken();
    console.log(token);

    this.cookies.set("Bearer", token);

    return this.linksService.linksAllGet();
  }
}
