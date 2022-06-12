import { Injectable } from '@angular/core';
import {
  Router, Resolve,
  RouterStateSnapshot,
  ActivatedRouteSnapshot
} from '@angular/router';
import { Observable, of } from 'rxjs';
import { IStatusItem, StatusesService } from '../api-client';

@Injectable({
  providedIn: 'root'
})
export class StatusesResolver implements Resolve<{ [key: string]: Array<IStatusItem>; }> {
  constructor(public statusesService: StatusesService){}
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<{ [key: string]: Array<IStatusItem>; }> {
    return this.statusesService.statusesAllGet();
  }
}
