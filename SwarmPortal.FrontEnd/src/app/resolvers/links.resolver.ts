import { Injectable } from '@angular/core';
import {
  Router, Resolve,
  RouterStateSnapshot,
  ActivatedRouteSnapshot
} from '@angular/router';
import { Observable, of } from 'rxjs';
import { ILinkItem, IStatusItem } from '../api';
import { HttpService } from '../services/http.service';

@Injectable({
  providedIn: 'root'
})
export class LinksResolver implements Resolve<{ [key: string]: ILinkItem[]; }> {
  constructor(private http: HttpService) { }
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<{ [key: string]: ILinkItem[]; }> {
    return this.http.GetLinks();
  }
}
