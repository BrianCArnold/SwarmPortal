import { AdminService, AuthService, BaseHttpRequest, IconService, LinksService, StatusesService } from "../openapi";
import { JwtToken } from "../../Models/JwtToken";


export interface IApiClient {
  admin: AdminService;
  auth: AuthService;
  icon: IconService;
  links: LinksService;
  statuses: StatusesService;
  request: BaseHttpRequest;
  get token(): JwtToken;
  get roles(): string[];
  get isLoggedIn(): boolean;
  login(): Promise<void>;
  processLogIn(): Promise<void>;
  logOut(): Promise<void>;
  processLogOut(): Promise<void>;
}
