import { link } from 'fs';
import React from 'react';
import { ILinkItem } from './services/openapi';

class LinksCard extends React.Component<{header: string, links: ILinkItem[], color: string }, {}> {


  

    render(): React.ReactNode {
        return (
        <div className="card mb-0 shad">
          <h6 className={"card-header text-muted border-"+this.props.color} >
            {this.props.header}
          </h6>
          <div className="d-grid d-md-block my-1">
            {this.props.links.map(l => (
              <a title={l.url||""} href={l.url||""} className={"m-2 my-1 btn btn-sm text-dark border-0 border-bottom btn-outline-"+this.props.color+" border-"+this.props.color} >
                {l.name}
              </a>
            ))}
{/*           
            <a title="link.url" *ngFor="let link of groupLinks" className="m-2 my-1 btn btn-sm text-dark border-0 border-bottom" [ngClass]="'btn-outline-' + groupColorName + ' border-' + groupColorName + ''"  [href]="link.url" target="_blank">
              <img style="width: 24px; height: 24px;" [src]="uriIconUrl(link.url) | secure | async" />
              {{link.name}}
            </a> */}
          </div>
        </div>)
      
      
    }
}
export default LinksCard;