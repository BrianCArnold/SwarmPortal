/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */

import type { Status } from './Status';

export type IStatusItem = {
    status?: Status;
    readonly name?: string | null;
    readonly group?: string | null;
    readonly roles?: Array<string> | null;
};

