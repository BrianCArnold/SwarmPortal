import { ICellRendererParams } from 'ag-grid-community';
import { resolve } from 'inversify-react';
import React from 'react';
import Form from 'react-bootstrap/Form';
import { linkRow } from '../Models/linkRow';
import { IApiClient } from '../services/Interfaces/IApiClient';

class RoleLinkButtonCellRenderer extends React.Component<ICellRendererParams<linkRow>> {
    @resolve("apiClient") private readonly client!: IApiClient;
    rowData: linkRow | undefined;
    onChange: () => Promise<void>;
    roleName: string;

    constructor(props: ICellRendererParams<linkRow>) {
        super(props);
        this.rowData = props.data;
        this.onChange = (props as any).onChange;
        this.roleName = (props as any).role;
    }
    handleClick() {
        if (this.rowData?.id) {
            if (this.rowData?.roles[this.roleName]) {
                this.client.admin.deleteAdminDeleteLinkRole(this.rowData?.id || -1, this.roleName).then(() => this.onChange());
            } else {
                this.client.admin.postAdminAddLinkRole(this.rowData?.id || -1, this.roleName).then(() => this.onChange());
            }
            
        }
    }

    render(): React.ReactNode {
        return (
            <Form>
                <Form.Check className='mt-2' type="switch" id="custom-switch" label="" checked={this.rowData?.roles[this.roleName]} onChange={() => this.handleClick()} />
            </Form>
        );
    }
}
export default RoleLinkButtonCellRenderer;