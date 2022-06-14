import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { OAuthService } from 'angular-oauth2-oidc';
import { first, firstValueFrom } from 'rxjs';
import { ILinkItem, IStatusItem, LinksService, StatusesService } from '../api';
import { UserService } from '../services/user.service';

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
    private linksService: LinksService,
    private statusesService: StatusesService,
    private userService: UserService) {
  }


  siteBackgrounds = ['primary', 'success', 'danger', 'warning'];


  async ngOnInit(): Promise<void> {
    if (this.userService.IsLoggedIn) {
      this.statusesService.defaultHeaders = this.statusesService.defaultHeaders.set('Authorization', 'Bearer ' + this.userService.token);
      this.linksService.defaultHeaders = this.linksService.defaultHeaders.set('Authorization', 'Bearer ' + this.userService.token);
    }
    this.statusDict = await firstValueFrom(this.statusesService.statusesAllGet());
    this.linkDict = await firstValueFrom(this.linksService.linksAllGet());

    this.linkGroups = this.dictionaryToEnumerable(this.linkDict);
    this.statusGroups = this.dictionaryToEnumerable(this.statusDict);
  }
}
