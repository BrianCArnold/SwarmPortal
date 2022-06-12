import { Injectable } from '@angular/core';
import {
  Router, Resolve,
  RouterStateSnapshot,
  ActivatedRouteSnapshot
} from '@angular/router';
import { Observable, of } from 'rxjs';
import { ILinkItem, LinksService } from '../api-client';

@Injectable({
  providedIn: 'root'
})
export class LinksResolver implements Resolve<{ [key: string]: Array<ILinkItem>; }> {
  constructor(public linksService: LinksService){}
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<{ [key: string]: Array<ILinkItem>; }> {
    return this.linksService.linksAllGet();
  }
}
