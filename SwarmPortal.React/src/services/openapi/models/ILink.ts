/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */

import type { Group } from './Group';
import type { IRole } from './IRole';

export type ILink = {
    readonly id?: number;
    readonly name?: string | null;
    readonly url?: string | null;
    group?: Group;
    readonly enabled?: boolean;
    readonly roles?: Array<IRole> | null;
};

