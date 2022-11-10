import { Container } from 'inversify';
import { IApiClient } from './Interfaces/IApiClient';
import { ApiClient } from './Implementations/ApiClient';
import { OpenAPIConfig } from './openapi';

export const container = new Container();
const client: IApiClient = new ApiClient();
const config: Partial<OpenAPIConfig> = {};
container.bind<IApiClient>("apiClient").toConstantValue(client);
container.bind<Partial<OpenAPIConfig>>("apiConfig").toConstantValue(config);