/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */

import type { Link } from './Link';

export type Role = {
    id?: number;
    name?: string | null;
    enabled?: boolean;
    links?: Array<Link> | null;
};

