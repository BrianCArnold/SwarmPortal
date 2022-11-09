import React from 'react';
import { IStatusItem } from './services/openapi';
import './StatusesCard.scss';

class StatusesCard extends React.Component<{header: string, links: IStatusItem[] }, {}> {


    render(): React.ReactNode {
        return (
        <div className="card mb-0 hostList">
          <div className="card-header" >
            {this.props.header}
          </div>
          <div className="card-body">
            <ul>
              {this.props.links.map(l => 
                <li>
                  <span className={(l.status as unknown as string).toLowerCase() || ''}>
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