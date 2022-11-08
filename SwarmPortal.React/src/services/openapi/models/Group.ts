/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */

import type { Link } from './Link';

export type Group = {
    id?: number;
    name?: string | null;
    enabled?: boolean;
    links?: Array<Link> | null;
};

