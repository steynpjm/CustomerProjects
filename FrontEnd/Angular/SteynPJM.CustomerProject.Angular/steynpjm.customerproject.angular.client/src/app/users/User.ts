export interface User {
  id: number;
  deletedIndicator: boolean;
  version: string;
  companyHid: number;
  username: string;
  password: string;
  title: string;
  firstname: string;
  lastname: string;
  email: string;
  designation: string;
  companyH: any;
  project: any[];
  [key: string]: any;
}
