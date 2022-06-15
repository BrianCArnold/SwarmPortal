import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { OAuthService } from 'angular-oauth2-oidc';
import { first, firstValueFrom, Observable } from 'rxjs';
import { ILinkItem, IStatusItem } from '../api';
import { HttpService } from '../services/http.service';

@Component({
  selector: 'app-status-screen',
  templateUrl: './status-screen.component.html',
  styleUrls: ['./status-screen.component.scss']
})
export class StatusScreenComponent implements OnInit {
  ;
  dictionaryToEnumerable<TItem>(dict: { [key: string]: TItem[]; }): { name: string; values: TItem[]; }[] {
    return Object.getOwnPropertyNames(dict)
      .map(p => ({ name: p, values: dict[p]}));
  }
  linkDict: { [key: string]: ILinkItem[]; } = {};
  statusDict: { [key: string]: IStatusItem[]; } = {};
  linkGroups: { name: string; values: ILinkItem[]; }[] = [];
  statusGroups: { name: string; values: IStatusItem[]; }[] = [];
  constructor(
    private http: HttpService,
    private oauth: OAuthService) {
  }


  siteBackgrounds = ['primary', 'success', 'danger', 'warning'];


  getStatuses(): Observable<{ [key: string]: Array<IStatusItem>; }> {
    return this.http.Identity != null ? this.http.Statuses.statusesAllGet() : this.http.Statuses.statusesPublicGet();
  }
  getLinks(): Observable<{ [key: string]: Array<IStatusItem>; }> {
    return this.http.Identity != null ? this.http.Links.linksAllGet() : this.http.Links.linksPublicGet();
  }
  async ngOnInit(): Promise<void> {
    //This is to allow the login/logout to finish happening.
    await this.delay(10);
    this.statusDict = await firstValueFrom(this.getStatuses());
    this.linkDict = await firstValueFrom(this.getLinks());

    this.linkGroups = this.dictionaryToEnumerable(this.linkDict);
    this.statusGroups = this.dictionaryToEnumerable(this.statusDict);
  }
  async delay(delayInms: number): Promise<void> {
    return new Promise(resolve  => {
      setTimeout(() => {
        resolve();
      }, delayInms);
    });
  }
}
