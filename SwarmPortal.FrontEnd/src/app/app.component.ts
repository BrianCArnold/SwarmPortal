import { Component, OnInit } from '@angular/core';
import { first, firstValueFrom } from 'rxjs';
import { ILinkItem, IStatusItem, LinksService, StatusesService } from './api-client';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = '@swarm-portal/frontend';
  linkDictionary: { [key: string]: ILinkItem[]; } = {};
  get linkGroups(): {
    name: string;
    values: ILinkItem[];
  }[] {
    return Object.getOwnPropertyNames(this.linkDictionary)
      .map(p => ({ name: p, values: this.linkDictionary[p]}));
  }
  statusDictionary: { [key: string]: IStatusItem[]; } = {};
  get statusGroups(): {
    name: string;
    values: IStatusItem[];
  }[] {
    return Object.getOwnPropertyNames(this.statusDictionary)
      .map(p => ({ name: p, values: this.statusDictionary[p]}));
  }
  constructor(public linksService: LinksService,
    public statusService: StatusesService) {
  }
  async ngOnInit(): Promise<void> {
    this.linkDictionary = await firstValueFrom(this.linksService.linksAllGet());
    this.statusDictionary = await firstValueFrom(this.statusService.statusesAllGet());
  }
}
