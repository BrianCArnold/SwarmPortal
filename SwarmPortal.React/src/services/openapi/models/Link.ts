/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */

import type { Group } from './Group';
import type { Role } from './Role';

export type Link = {
    id?: number;
    name?: string | null;
    url?: string | null;
    group?: Group;
    enabled?: boolean;
    roles?: Array<Role> | null;
};

