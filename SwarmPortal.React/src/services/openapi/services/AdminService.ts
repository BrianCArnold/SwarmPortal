/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { CommonLinkItem } from '../models/CommonLinkItem';
import type { IGroup } from '../models/IGroup';
import type { ILink } from '../models/ILink';
import type { ILinkItem } from '../models/ILinkItem';
import type { IRole } from '../models/IRole';

import type { CancelablePromise } from '../core/CancelablePromise';
import type { BaseHttpRequest } from '../core/BaseHttpRequest';

export class AdminService {

    constructor(public readonly httpRequest: BaseHttpRequest) {}

    /**
     * @returns ILink Success
     * @throws ApiError
     */
    public getAdminAllLinks(): CancelablePromise<Array<ILink>> {
        return this.httpRequest.request({
            method: 'GET',
            url: '/Admin/All/Links',
        });
    }

    /**
     * @returns ILink Success
     * @throws ApiError
     */
    public getAdminAllEnabledLinks(): CancelablePromise<Array<ILink>> {
        return this.httpRequest.request({
            method: 'GET',
            url: '/Admin/All/EnabledLinks',
        });
    }

    /**
     * @returns ILink Success
     * @throws ApiError
     */
    public getAdminAllDisabledLinks(): CancelablePromise<Array<ILink>> {
        return this.httpRequest.request({
            method: 'GET',
            url: '/Admin/All/DisabledLinks',
        });
    }

    /**
     * @param group
     * @returns ILink Success
     * @throws ApiError
     */
    public getAdminLinks(
        group: string,
    ): CancelablePromise<Array<ILink>> {
        return this.httpRequest.request({
            method: 'GET',
            url: '/Admin/{group}/Links',
            path: {
                'group': group,
            },
        });
    }

    /**
     * @param role
     * @returns ILink Success
     * @throws ApiError
     */
    public getAdminAllLinksFor(
        role: string,
    ): CancelablePromise<Array<ILink>> {
        return this.httpRequest.request({
            method: 'GET',
            url: '/Admin/All/LinksFor/{role}',
            path: {
                'role': role,
            },
        });
    }

    /**
     * @param role
     * @param group
     * @returns ILink Success
     * @throws ApiError
     */
    public getAdminLinksFor(
        role: string,
        group: string,
    ): CancelablePromise<Array<ILink>> {
        return this.httpRequest.request({
            method: 'GET',
            url: '/Admin/{group}/LinksFor/{role}',
            path: {
                'role': role,
                'group': group,
            },
        });
    }

    /**
     * @returns IRole Success
     * @throws ApiError
     */
    public getAdminRoles(): CancelablePromise<Array<IRole>> {
        return this.httpRequest.request({
            method: 'GET',
            url: '/Admin/Roles',
        });
    }

    /**
     * @returns IRole Success
     * @throws ApiError
     */
    public getAdminEnabledRolesWithNoLinks(): CancelablePromise<Array<IRole>> {
        return this.httpRequest.request({
            method: 'GET',
            url: '/Admin/EnabledRolesWithNoLinks',
        });
    }

    /**
     * @returns IRole Success
     * @throws ApiError
     */
    public getAdminDisabledRoles(): CancelablePromise<Array<IRole>> {
        return this.httpRequest.request({
            method: 'GET',
            url: '/Admin/DisabledRoles',
        });
    }

    /**
     * @param role
     * @returns string Success
     * @throws ApiError
     */
    public postAdminAddRole(
        role: string,
    ): CancelablePromise<string> {
        return this.httpRequest.request({
            method: 'POST',
            url: '/Admin/AddRole/{role}',
            path: {
                'role': role,
            },
        });
    }

    /**
     * @param roleId
     * @returns any Success
     * @throws ApiError
     */
    public deleteAdminDisableRole(
        roleId: number,
    ): CancelablePromise<any> {
        return this.httpRequest.request({
            method: 'DELETE',
            url: '/Admin/DisableRole/{roleId}',
            path: {
                'roleId': roleId,
            },
        });
    }

    /**
     * @param roleId
     * @returns string Success
     * @throws ApiError
     */
    public putAdminEnableRole(
        roleId: number,
    ): CancelablePromise<string> {
        return this.httpRequest.request({
            method: 'PUT',
            url: '/Admin/EnableRole/{roleId}',
            path: {
                'roleId': roleId,
            },
        });
    }

    /**
     * @returns IGroup Success
     * @throws ApiError
     */
    public getAdminGroups(): CancelablePromise<Array<IGroup>> {
        return this.httpRequest.request({
            method: 'GET',
            url: '/Admin/Groups',
        });
    }

    /**
     * @returns IGroup Success
     * @throws ApiError
     */
    public getAdminDisabledGroups(): CancelablePromise<Array<IGroup>> {
        return this.httpRequest.request({
            method: 'GET',
            url: '/Admin/DisabledGroups',
        });
    }

    /**
     * @returns IGroup Success
     * @throws ApiError
     */
    public getAdminEnabledGroupsWithNoLinks(): CancelablePromise<Array<IGroup>> {
        return this.httpRequest.request({
            method: 'GET',
            url: '/Admin/EnabledGroupsWithNoLinks',
        });
    }

    /**
     * @param group
     * @returns string Success
     * @throws ApiError
     */
    public postAdminAddGroup(
        group: string,
    ): CancelablePromise<string> {
        return this.httpRequest.request({
            method: 'POST',
            url: '/Admin/AddGroup/{group}',
            path: {
                'group': group,
            },
        });
    }

    /**
     * @param groupId
     * @returns any Success
     * @throws ApiError
     */
    public deleteAdminDisableGroup(
        groupId: number,
    ): CancelablePromise<any> {
        return this.httpRequest.request({
            method: 'DELETE',
            url: '/Admin/DisableGroup/{groupId}',
            path: {
                'groupId': groupId,
            },
        });
    }

    /**
     * @param groupId
     * @returns any Success
     * @throws ApiError
     */
    public putAdminEnableGroup(
        groupId: number,
    ): CancelablePromise<any> {
        return this.httpRequest.request({
            method: 'PUT',
            url: '/Admin/EnableGroup/{groupId}',
            path: {
                'groupId': groupId,
            },
        });
    }

    /**
     * @param requestBody
     * @returns ILinkItem Success
     * @throws ApiError
     */
    public postAdminAddLink(
        requestBody?: CommonLinkItem,
    ): CancelablePromise<ILinkItem> {
        return this.httpRequest.request({
            method: 'POST',
            url: '/Admin/AddLink',
            body: requestBody,
            mediaType: 'application/json-patch+json',
        });
    }

    /**
     * @param linkId
     * @param role
     * @returns string Success
     * @throws ApiError
     */
    public postAdminAddLinkRole(
        linkId: number,
        role: string,
    ): CancelablePromise<string> {
        return this.httpRequest.request({
            method: 'POST',
            url: '/Admin/AddLinkRole/{linkId}/{role}',
            path: {
                'linkId': linkId,
                'role': role,
            },
        });
    }

    /**
     * @param linkId
     * @returns any Success
     * @throws ApiError
     */
    public deleteAdminDisableLink(
        linkId: number,
    ): CancelablePromise<any> {
        return this.httpRequest.request({
            method: 'DELETE',
            url: '/Admin/DisableLink/{linkId}',
            path: {
                'linkId': linkId,
            },
        });
    }

    /**
     * @param linkId
     * @returns any Success
     * @throws ApiError
     */
    public putAdminEnableLink(
        linkId: number,
    ): CancelablePromise<any> {
        return this.httpRequest.request({
            method: 'PUT',
            url: '/Admin/EnableLink/{linkId}',
            path: {
                'linkId': linkId,
            },
        });
    }

    /**
     * @param linkId
     * @param role
     * @returns any Success
     * @throws ApiError
     */
    public deleteAdminDeleteLinkRole(
        linkId: number,
        role: string,
    ): CancelablePromise<any> {
        return this.httpRequest.request({
            method: 'DELETE',
            url: '/Admin/DeleteLinkRole/{linkId}/{role}',
            path: {
                'linkId': linkId,
                'role': role,
            },
        });
    }

}
