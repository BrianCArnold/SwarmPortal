import { Injectable } from '@angular/core';
import { OAuthStorage } from 'angular-oauth2-oidc';

export class SwarmStorage extends OAuthStorage {
  private prefix: string;
  constructor(prefix: string) {
    super();
    this.prefix = prefix;
  }
  getItem(key: string): string | null {
    console.info(`Getting Item ${key} from Local Storage for OAuth`);
    const value = localStorage.getItem(this.prefix + "_" + key);
    console.info(`Got Item ${key} from Local Storage for OAuth with value of ${value}`);
    return value;
  }
  setItem(key: string, data: string): void {
    console.info(`Setting Item ${key} to Local Storage for OAuth with value of ${data}`);
    localStorage.setItem(this.prefix + "_" + key, data);
    console.info(`Set Item ${key} to Local Storage for OAuth with value of ${data}`);
  }
  removeItem(key: string): void {
    console.info(`Removing Item ${key} from Local Storage for OAuth`);
    localStorage.removeItem(this.prefix + "_" + key);
    console.info(`Removed Item ${key} from Local Storage for OAuth`);
  }
}
