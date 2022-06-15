import { Component, Input, OnInit } from '@angular/core';
// import { ILinkItem, IStatusItem } from '../api-client';

@Component({
  selector: 'app-status-group-card',
  templateUrl: './status-group-card.component.html',
  styleUrls: ['./status-group-card.component.scss']
})
export class StatusGroupCardComponent implements OnInit {

  ngOnInit(): void {
  }
  @Input()
  groupColorName: string = 'primary';
  @Input()
  groupName: string = "";
  @Input()
  groupStatuses: any[] = [];
}
