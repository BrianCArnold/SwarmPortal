import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { HttpService } from '../services/http.service';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {

  constructor(private http: HttpService){}
  canActivate(
    _: ActivatedRouteSnapshot,
    __: RouterStateSnapshot): boolean {
    return this.http.Identity == null ? false : this.http.Identity?.roles.includes('admin');
  }

}
