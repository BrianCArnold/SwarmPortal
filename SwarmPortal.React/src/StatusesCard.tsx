import React from 'react';
import { IStatusItem, Status } from './services/openapi';
import './StatusesCard.scss';

class StatusesCard extends React.Component<{header: string, links: IStatusItem[], color: string }, {}> {


    render(): React.ReactNode {
        return (
        <div className="card mb-0 hostList">
          <div className={"card-header border-"+this.props.color+" text-muted"} >
            {this.props.header}
          </div>
          <div className="card-body">
            <ul>
              {this.props.links.map(l => 
                <li>
                  <span className={(l.status || Status.UNKNOWN).toLowerCase() || ''}>
                    {l.name}
                  </span>
                </li>)}
              
            </ul>
          </div>
        </div>
        );
    }
}
export default StatusesCard;