/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { Stream } from '../models/Stream';

import type { CancelablePromise } from '../core/CancelablePromise';
import type { BaseHttpRequest } from '../core/BaseHttpRequest';

export class IconService {

    constructor(public readonly httpRequest: BaseHttpRequest) {}

    /**
     * @param uri
     * @returns Stream Success
     * @throws ApiError
     */
    public getIcon(
        uri: string,
    ): CancelablePromise<Stream> {
        return this.httpRequest.request({
            method: 'GET',
            url: '/Icon/{uri}',
            path: {
                'uri': uri,
            },
        });
    }

    /**
     * @returns Stream Success
     * @throws ApiError
     */
    public getIconFailure(): CancelablePromise<Stream> {
        return this.httpRequest.request({
            method: 'GET',
            url: '/Icon/failure',
        });
    }

}
