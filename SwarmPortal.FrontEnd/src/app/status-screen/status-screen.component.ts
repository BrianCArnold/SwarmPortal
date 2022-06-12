import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { first, firstValueFrom } from 'rxjs';
import { ILinkItem, IStatusItem, LinksService, StatusesService } from '../api-client';

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
  constructor(private activatedRoute: ActivatedRoute) {
  }
  async ngOnInit(): Promise<void> {
    this.linkDict = this.activatedRoute.snapshot.data["links"];
    this.statusDict = this.activatedRoute.snapshot.data["statuses"];

    this.linkGroups = this.dictionaryToEnumerable(this.linkDict);
    this.statusGroups = this.dictionaryToEnumerable(this.statusDict);
  }
}
