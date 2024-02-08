export interface Project {
  id: number;
  deletedIndicator: boolean;
  version: string;
  name: string;
  code: string;
  description: string;
  companyHid: number;
  managerHid: number;
  [key: string]: any;
}
