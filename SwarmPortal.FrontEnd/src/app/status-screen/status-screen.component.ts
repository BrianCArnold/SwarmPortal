import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxMasonryOptions } from 'ngx-masonry';
import { ILinkItem, IStatusItem } from '../api';

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
  constructor(private activatedRoute: ActivatedRoute) {
    this.activatedRoute.data.subscribe(data => {
      this.linkGroups = this.dictionaryToEnumerable(data['links']);
      this.statusGroups = this.dictionaryToEnumerable(data['statuses']);
    })
  }
  ngOnDestroy(): void {
  }


  siteBackgrounds = ['primary', 'success', 'danger', 'warning'];
  masonryOptions: NgxMasonryOptions = {
  }

  async ngOnInit(): Promise<void> {
  }
  async delay(delayInms: number): Promise<void> {
    return new Promise(resolve  => {
      setTimeout(() => {
        resolve();
      }, delayInms);
    });
  }
}
