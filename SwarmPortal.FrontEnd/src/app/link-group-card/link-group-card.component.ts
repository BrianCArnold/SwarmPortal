import { Component, Input, OnInit } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import * as URLParse from 'url-parse';
import { LinksService } from '../api';
// import { ILinkItem } from '../api-client';

@Component({
  selector: 'app-link-group-card',
  templateUrl: './link-group-card.component.html',
  styleUrls: ['./link-group-card.component.scss']
})
export class LinkGroupCardComponent implements OnInit {

  constructor(private links: LinksService) { }

  async ngOnInit(): Promise<void> {

  }

  async doPersonal(){
    var personal = await firstValueFrom( this.links.linksPersonalGet());
    console.log("PERSONAL");
    console.log(personal);
  }

  @Input()
  groupColorName: string = 'primary';
  @Input()
  groupName: string = "";
  @Input()
  groupLinks: any[] = [];
}
