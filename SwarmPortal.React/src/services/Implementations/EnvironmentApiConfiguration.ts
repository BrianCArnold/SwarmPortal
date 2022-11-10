import { IApiConfiguration } from "../Interfaces/IApiConfiguration";
import { injectable } from 'inversify';

@injectable()
export class EnvironmentApiConfiguration implements IApiConfiguration {
    BASE = process.env.REACT_APP_BASE_URI || "";
}