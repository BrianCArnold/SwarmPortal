import React from 'react';
import { ILinkItem } from '../services/openapi';
import './LinksCard.scss';

class LinksCard extends React.Component<{header: string, links: ILinkItem[], color: string }, {}> {

  links(): React.ReactNode[] {
    return this.props.links.map(l => (
      <a key={l.url} title={l.url||""} href={l.url||""} className={"m-2 my-1 btn btn-sm text-dark border-0 border-bottom btn-outline-"+this.props.color+" border-"+this.props.color} >
        <img className='linkImage' alt={l.name||""} src={this.uriIconUrl(l.url||"")} />
        {l.name}
      </a>
    ));
  }
  

  uriIconUrl(uri: string): string {
    return "http://localhost:5109" + "/Icon/" + encodeURIComponent(uri);
  }

  render(): React.ReactNode {
    return (
      <div className="card mb-0 shad">
        <h6 className={"card-header text-muted border-"+this.props.color} >
          {this.props.header}
        </h6>
        <div className="d-grid d-md-block my-1">
          {this.links()}
        </div>
      </div>
    );
  }
}
export default LinksCard;