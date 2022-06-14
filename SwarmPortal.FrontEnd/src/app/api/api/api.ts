export * from './admin.service';
import { AdminService } from './admin.service';
export * from './auth.service';
import { AuthService } from './auth.service';
export * from './links.service';
import { LinksService } from './links.service';
export * from './statuses.service';
import { StatusesService } from './statuses.service';
export const APIS = [AdminService, AuthService, LinksService, StatusesService];
