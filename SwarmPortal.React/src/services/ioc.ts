import { Container } from 'inversify';
import { IApiClient } from './Interfaces/IApiClient';
import { ApiClient } from './Implementations/ApiClient';
import { OpenAPIConfig } from './openapi';
import { IApiConfiguration } from './Interfaces/IApiConfiguration';
import { EnvironmentApiConfiguration } from './Implementations/EnvironmentApiConfiguration';

export const container = new Container();
const config: Partial<OpenAPIConfig> = new EnvironmentApiConfiguration();
const client = new ApiClient(config);
container.bind<IApiConfiguration>("apiConfig").toConstantValue(config);
container.bind<IApiClient>("apiClient").toConstantValue(client);