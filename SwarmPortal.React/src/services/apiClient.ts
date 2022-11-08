import { BaseHttpRequest } from "./openapi/core/BaseHttpRequest";
import type { OpenAPIConfig } from "./openapi/core/OpenAPI";
import jwt_decode, { JwtPayload } from "jwt-decode";
import type { ApiRequestOptions } from "./openapi/core/ApiRequestOptions";
import { CancelablePromise } from "./openapi/core/CancelablePromise";
import { internalClient } from "./openapi";
import { request as __request } from "./openapi/core/request";

export interface JwtToken extends JwtPayload {
  name: string;
}
const tokenKey = "swarmportalAuth";
export class ApiClient extends internalClient {

  headers: Record<string, string>;
  constructor(config?: Partial<OpenAPIConfig>) {
    const token = localStorage.getItem(tokenKey);
    const headers: Record<string, string> = {};
    if (token) {
      headers["Authorization"] = `Bearer ${token}`;
    }
    const url = "http://localhost:5109";
    //change conf to commented line for beta.encora.it building
    const conf = { ...{ BASE: url, HEADERS: headers }, ...config };
    super(conf, ObjectGraphReconstructingRequest);
    this.headers = headers;
  }
  get isLoggedIn(): boolean {
    return !!localStorage.getItem(tokenKey);
  }
  logIn(token: string) {
    localStorage.setItem(tokenKey, token);
    this.headers["Authorization"] = token;
  }
  logOut() {
    localStorage.removeItem(tokenKey)
  }

}
export class ObjectGraphReconstructingRequest extends BaseHttpRequest {
  constructor(config: OpenAPIConfig) {
    super(config);
  }

  /**
   * Request method
   * @param options The request options from the service
   * @returns CancelablePromise<T>
   * @throws ApiError
   */
  public override request<T>(options: ApiRequestOptions): CancelablePromise<T> {
    return new CancelablePromise(async (resolve, reject, onCancel) => {
      try {
        const cancellableRequest = __request<T>(this.config, options);
        onCancel(() => cancellableRequest.cancel());
        const result = await cancellableRequest;
        resolve(this.restoreJsonNetReferences(result));
      } catch (error) {
        reject(error);
      }
    });
  }

  cloneObject<T>(obj: T): T {
    const clone = this.restoreJsonNetReferences(
      JSON.parse(JSON.stringify(this.prepareObjectReferences(obj)))
    );
    this.restoreJsonNetReferences(obj);
    return clone;
  }

  prepareObjectReferences<T>(obj: T): T {
    const models: any[] = [];
    const handleProperty = (pObj: any): any => {
      // console.log(typeof (pObj));
      if (pObj === null || typeof pObj !== "object" || pObj instanceof String) {
        if (typeof pObj === "string") {
          return pObj;
        } else if (pObj instanceof String) {
          return pObj.toString();
        }
        //it's fine.

        return pObj;
      } else if (pObj.constructor.name === "Date") {
        let retDate = pObj as Date;
        if (
          retDate.getHours() + retDate.getMinutes() + retDate.getSeconds() ===
          0
        ) {
          retDate = new Date(
            Date.UTC(
              retDate.getFullYear(),
              retDate.getMonth(),
              retDate.getDate()
            )
          );
        }
        return retDate.toISOString();
      } else {
        if (Array.isArray(pObj)) {
          const res = [];
          for (let i = 0; i < pObj.length; i++) {
            res.push(handleProperty(pObj[i]));
          }
          return res;
        } else {
          const mIndex = models.indexOf(pObj);
          if (mIndex >= 0) {
            const newModel = { $ref: mIndex.toString() };
            return newModel;
          } else {
            models.push(pObj);
            return pObj;
          }
        }
      }
    };
    const addIdentity = (model: any): any => {
      const mIndex = models.findIndex((m) => m === model);
      if (Array.isArray(model)) {
        const res = [];
        for (let i = 0; i < model.length; i++) {
          res.push(addIdentity(model[i]));
        }
        return res;
      }
      if (mIndex >= 0) {
        const newModel: { [index: string]: any } = {
          $id: mIndex.toString(),
          ...model,
        };

        // tslint:disable-next-line:forin
        for (const p in model) {
          newModel[p] = addIdentity(model[p]);
        }
        return newModel;
      }
      return model;
    };
    if (obj != null && typeof obj === "object") {
      models.push(obj);
      for (let i = 0; i < models.length; i++) {
        // tslint:disable-next-line:forin
        for (const p in models[i]) {
          models[i][p] = handleProperty(models[i][p]);
        }
      }

      return addIdentity(obj);
    }
    return obj;
  }

  restoreJsonNetReferences<TModel>(m: TModel): TModel {
    const ids: { [index: string]: any } = {};

    const getIds = (model: any): any => {
      // we care naught about primitives
      if (
        model === null ||
        typeof model !== "object" ||
        model instanceof String
      ) {
        if (model instanceof String) {
          model = model.toString();
        }
        if (typeof model === "string") {
          if (
            model.match(
              /^[0-9]{4}-[0-9]{2}-[0-9]{2}T[0-9]{2}:[0-9]{2}:[0-9]{2}/
            )
          ) {
            return new Date(Date.parse(model));
          } else {
            return model;
          }
        }
        return model;
      }
      if (Array.isArray(model)) {
        const nArr = [];
        for (let i = 0; i < model.length; i++) {
          nArr.push(getIds(model[i]));
        }
        return nArr;
      }
      const id = model["$id"];
      if (id != null) {
        delete model["$id"];

        // either return previously known object, or
        // remember this object linking for later
        if (ids[id]) {
          console.error("Duplicate ID " + id + "found.");
          console.error(model);
        }
        ids[id] = model;
      }

      // then, recursively for each key/index, relink the sub-graph
      for (const p in model) {
        if (model.hasOwnProperty(p)) {
          model[p] = getIds(model[p]);
        }
      }
      return model;
    };

    const relink = (model: any) => {
      // we care naught about primitives
      if (model === null || typeof model !== "object") {
        return model;
      }
      const id = model["$ref"];

      // either return previously known object, or
      // remember this object linking for later
      if (id != null) {
        // delete model['$ref'];
        if (ids[id] === null) {
          //console.log(ids);
          //console.log(id);
        }
        return ids[id];
      }

      // then, recursively for each key/index, relink the sub-graph
      if (model.hasOwnProperty("length")) {
        // array or array-like; a different guard may be more appropriate
        for (let i = 0; i < model.length; i++) {
          model[i] = relink(model[i]);
        }
      } else {
        // other objects
        for (const p in model) {
          if (model.hasOwnProperty(p)) {
            model[p] = relink(model[p]);
          }
        }
      }

      return model;
    };

    m = getIds(m);
    const result = relink(m);
    return result;
  }
}
