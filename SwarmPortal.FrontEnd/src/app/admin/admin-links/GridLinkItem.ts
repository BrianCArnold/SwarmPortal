
export interface GridLinkItem {
  id: number;
  url: string;
  name: string;
  enabled: boolean;
  roles: { [key: string]: boolean; };
  group: string;
}
