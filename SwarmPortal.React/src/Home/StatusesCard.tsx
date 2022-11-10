import React from 'react';
import { IStatusItem } from '../services/openapi';
import './StatusesCard.scss';

class StatusesCard extends React.Component<{header: string, links: IStatusItem[], color: string }, {}> {

  statuses(): React.ReactNode[] {
    return this.props.links.map(l => (
      <li key={l.name}>
        <span title={l.name||""} className={"m-2 my-1 btn btn-sm text-dark border-0 border-bottom btn-outline-"+this.props.color+" border-"+this.props.color+ " "+ l.status?.toLowerCase()} >
          {l.name}
        </span>
      </li>
    ));
  }

  render(): React.ReactNode {
    return (
      <div className="card mb-0 hostList">
        <div className={"card-header border-"+this.props.color+" text-muted"} >
          {this.props.header}
        </div>
        <div className="d-grid d-md-block my-1">
          <ul>
            {this.statuses()}
          </ul>
        </div>
      </div>
    );
  }
}
export default StatusesCard;