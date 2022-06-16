import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { OAuthService } from 'angular-oauth2-oidc';
import { firstValueFrom, interval, Observable, timer } from 'rxjs';
import { ILinkItem, IStatusItem } from '../api';
import { HttpService } from '../services/http.service';

@Component({
  selector: 'app-status-screen',
  templateUrl: './status-screen.component.html',
  styleUrls: ['./status-screen.component.scss']
})
export class StatusScreenComponent implements OnInit, OnDestroy {
  refreshTimer: any;
  ;
  dictionaryToEnumerable<TItem>(dict: { [key: string]: TItem[]; }): { name: string; values: TItem[]; }[] {
    return Object.getOwnPropertyNames(dict)
      .map(p => ({ name: p, values: dict[p]}));
  }
  linkGroups: { name: string; values: ILinkItem[]; }[] = [];
  statusGroups: { name: string; values: IStatusItem[]; }[] = [];
  constructor(
    private http: HttpService,
    private oauth: OAuthService) {
  }
  ngOnDestroy(): void {
    this.refreshTimer.unsubscribe();
  }


  siteBackgrounds = ['primary', 'success', 'danger', 'warning'];


  getStatuses(): Observable<{ [key: string]: Array<IStatusItem>; }> {
    return this.http.Identity != null ? this.http.Statuses.statusesAllGet() : this.http.Statuses.statusesPublicGet();
  }
  getLinks(): Observable<{ [key: string]: Array<IStatusItem>; }> {
    return this.http.Identity != null ? this.http.Links.linksAllGet() : this.http.Links.linksPublicGet();
  }
  async ngOnInit(): Promise<void> {
    await this.loadLinks()
    await this.loadStatuses();
    this.refreshTimer = interval(30000).subscribe(async _ => {
      await this.loadLinks();
      await this.loadStatuses();
    });
  }
  async loadLinks(): Promise<void> {
    const linkDict = await firstValueFrom(this.getLinks());
    this.linkGroups = this.dictionaryToEnumerable(linkDict);
  }
  async loadStatuses(): Promise<void> {
    const statusDict = await firstValueFrom(this.getStatuses());
    this.statusGroups = this.dictionaryToEnumerable(statusDict);
  }
  async delay(delayInms: number): Promise<void> {
    return new Promise(resolve  => {
      setTimeout(() => {
        resolve();
      }, delayInms);
    });
  }
}
