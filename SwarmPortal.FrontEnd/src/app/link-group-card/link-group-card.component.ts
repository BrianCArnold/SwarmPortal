import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-link-group-card',
  templateUrl: './link-group-card.component.html',
  styleUrls: ['./link-group-card.component.scss']
})
export class LinkGroupCardComponent implements OnInit {

  constructor() { }

  async ngOnInit(): Promise<void> {

  }

  @Input()
  groupColorName: string = 'primary';
  @Input()
  groupName: string = "";
  @Input()
  groupLinks: any[] = [];
}
