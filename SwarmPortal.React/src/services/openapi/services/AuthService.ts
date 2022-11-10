/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { IAuthConfig } from '../models/IAuthConfig';

import type { CancelablePromise } from '../core/CancelablePromise';
import type { BaseHttpRequest } from '../core/BaseHttpRequest';

export class AuthService {

    constructor(public readonly httpRequest: BaseHttpRequest) {}

    /**
     * @returns IAuthConfig Success
     * @throws ApiError
     */
    public getAuthConfig(): CancelablePromise<IAuthConfig> {
        return this.httpRequest.request({
            method: 'GET',
            url: '/Auth/Config',
        });
    }

    /**
     * @returns any Success
     * @throws ApiError
     */
    public getAuthProcessLogin(): CancelablePromise<any> {
        return this.httpRequest.request({
            method: 'GET',
            url: '/Auth/ProcessLogin',
        });
    }

}
