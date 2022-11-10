import { ICellRendererParams } from 'ag-grid-community';
import { resolve } from 'inversify-react';
import React from 'react';
import Form from 'react-bootstrap/Form';
import { IApiClient } from '../services/Interfaces/IApiClient';
import { ILink } from '../services/openapi';

class DeleteLinkButtonCellRenderer extends React.Component<ICellRendererParams<ILink>> {
    @resolve("apiClient") private readonly client!: IApiClient;
    rowData: ILink | undefined;
    onChange: () => Promise<void>;

    constructor(props: ICellRendererParams<ILink>) {
        super(props);
        this.rowData = props.data;
        this.onChange = (props as any).onChange;
    }
    handleClick() {
        if (this.rowData?.id) {
            if (this.rowData.enabled) {
                this.client.admin.deleteAdminDisableLink(this.rowData?.id || -1).then(() => this.onChange());
            } else {
                this.client.admin.putAdminEnableLink(this.rowData?.id || -1).then(() => this.onChange());
            }
            
        }
    }

    render(): React.ReactNode {
        return (
            <Form>
                <Form.Check className='mt-2' type="switch" id="custom-switch" label="" checked={this.rowData?.enabled} onChange={() => this.handleClick()} />
            </Form>
        );
    }
}
export default DeleteLinkButtonCellRenderer;