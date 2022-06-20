
export interface GridLinkItem {
  id: number;
  url: string;
  name: string;
  roles: { [key: string]: boolean; };
  group: string;
}
