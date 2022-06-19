import { Injectable } from '@angular/core';
import {
  Router, Resolve,
  RouterStateSnapshot,
  ActivatedRouteSnapshot
} from '@angular/router';
import { Observable, of } from 'rxjs';
import { IStatusItem } from '../api';
import { HttpService } from '../services/http.service';

@Injectable({
  providedIn: 'root'
})
export class StatusesResolver implements Resolve<{ [key: string]: IStatusItem[]; }> {
  constructor(private http: HttpService) { }
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<{ [key: string]: IStatusItem[]; }> {
    return this.http.GetStatuses();
  }
}
