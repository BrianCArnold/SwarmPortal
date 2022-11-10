/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { IStatusItem } from '../models/IStatusItem';

import type { CancelablePromise } from '../core/CancelablePromise';
import type { BaseHttpRequest } from '../core/BaseHttpRequest';

export class StatusesService {

    constructor(public readonly httpRequest: BaseHttpRequest) {}

    /**
     * @returns IStatusItem Success
     * @throws ApiError
     */
    public getStatusesPublic(): CancelablePromise<Record<string, Array<IStatusItem>>> {
        return this.httpRequest.request({
            method: 'GET',
            url: '/Statuses/Public',
        });
    }

    /**
     * @returns IStatusItem Success
     * @throws ApiError
     */
    public getStatusesAll(): CancelablePromise<Record<string, Array<IStatusItem>>> {
        return this.httpRequest.request({
            method: 'GET',
            url: '/Statuses/All',
        });
    }

}
