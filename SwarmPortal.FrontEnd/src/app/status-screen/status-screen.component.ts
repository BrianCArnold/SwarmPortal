import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { OAuthService } from 'angular-oauth2-oidc';
import { first, firstValueFrom } from 'rxjs';
import { ILinkItem, IStatusItem, LinksService, StatusesService } from '../api';
import { HttpService } from '../services/http.service';

@Component({
  selector: 'app-status-screen',
  templateUrl: './status-screen.component.html',
  styleUrls: ['./status-screen.component.scss']
})
export class StatusScreenComponent implements OnInit {
  dictionaryToEnumerable<TItem>(dict: { [key: string]: TItem[]; }): { name: string; values: TItem[]; }[] {
    return Object.getOwnPropertyNames(dict)
      .map(p => ({ name: p, values: dict[p]}));
  }
  linkDict: { [key: string]: ILinkItem[]; } = {};
  statusDict: { [key: string]: IStatusItem[]; } = {};
  linkGroups: { name: string; values: ILinkItem[]; }[] = [];
  statusGroups: { name: string; values: IStatusItem[]; }[] = [];
  constructor(
    private http: HttpService) {
  }


  siteBackgrounds = ['primary', 'success', 'danger', 'warning'];


  async ngOnInit(): Promise<void> {
    this.statusDict = await firstValueFrom(this.http.Statuses.statusesAllGet());
    this.linkDict = await firstValueFrom(this.http.Links.linksAllGet());

    this.linkGroups = this.dictionaryToEnumerable(this.linkDict);
    this.statusGroups = this.dictionaryToEnumerable(this.statusDict);
  }
}
