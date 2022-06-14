export * from './auth.service';
import { AuthService } from './auth.service';
export * from './links.service';
import { LinksService } from './links.service';
export * from './statuses.service';
import { StatusesService } from './statuses.service';
export const APIS = [AuthService, LinksService, StatusesService];
