import { Injectable } from '@angular/core';
import {
  Resolve,
  RouterStateSnapshot,
  ActivatedRouteSnapshot
} from '@angular/router';
import { Observable } from 'rxjs';
import { ILinkItem } from '../api';
import { HttpService } from '../services/http.service';

@Injectable({
  providedIn: 'root'
})
export class LinksResolver implements Resolve<{ [key: string]: ILinkItem[]; }> {
  constructor(private http: HttpService) { }
  resolve(_: ActivatedRouteSnapshot, __: RouterStateSnapshot): Observable<{ [key: string]: ILinkItem[]; }> {
    return this.http.GetLinks();
  }
}
