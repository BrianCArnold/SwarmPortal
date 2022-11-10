/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */

import type { Link } from './Link';

export type IGroup = {
    id?: number;
    name?: string | null;
    readonly enabled?: boolean;
    links?: Array<Link> | null;
};

