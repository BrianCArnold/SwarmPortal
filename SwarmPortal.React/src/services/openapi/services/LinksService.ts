/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { ILinkItem } from '../models/ILinkItem';

import type { CancelablePromise } from '../core/CancelablePromise';
import type { BaseHttpRequest } from '../core/BaseHttpRequest';

export class LinksService {

    constructor(public readonly httpRequest: BaseHttpRequest) {}

    /**
     * @returns ILinkItem Success
     * @throws ApiError
     */
    public getLinksPublic(): CancelablePromise<Record<string, Array<ILinkItem>>> {
        return this.httpRequest.request({
            method: 'GET',
            url: '/Links/Public',
        });
    }

    /**
     * @returns ILinkItem Success
     * @throws ApiError
     */
    public getLinksAll(): CancelablePromise<Record<string, Array<ILinkItem>>> {
        return this.httpRequest.request({
            method: 'GET',
            url: '/Links/All',
        });
    }

}
