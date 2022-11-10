/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */

export type Stream = {
    readonly canRead?: boolean;
    readonly canWrite?: boolean;
    readonly canSeek?: boolean;
    readonly canTimeout?: boolean;
    readonly length?: number;
    position?: number;
    readTimeout?: number;
    writeTimeout?: number;
};

