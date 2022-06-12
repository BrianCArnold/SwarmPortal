import { Component, Input, OnInit } from '@angular/core';
import { ILinkItem } from '../api-client';

@Component({
  selector: 'app-link-group-card',
  templateUrl: './link-group-card.component.html',
  styleUrls: ['./link-group-card.component.scss']
})
export class LinkGroupCardComponent implements OnInit {

  constructor() {
    this.groupName = "";
    this.groupLinks = [];
  }

  ngOnInit(): void {
  }

  @Input()
  groupName: string;
  @Input()
  groupLinks: ILinkItem[];
}
