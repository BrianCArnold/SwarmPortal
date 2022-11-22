import { Component, Input, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-link-group-card',
  templateUrl: './link-group-card.component.html',
  styleUrls: ['./link-group-card.component.scss']
})
export class LinkGroupCardComponent implements OnInit {

  constructor() { }

  async ngOnInit(): Promise<void> {

  }

  uriIconUrl(uri: string): string {
    return environment.apiRoot + "/Icon/" + encodeURIComponent(uri);
  }

  @Input()
  groupColorName: string = 'primary';
  @Input()
  groupName: string = "";
  @Input()
  groupLinks: any[] = [];
}
