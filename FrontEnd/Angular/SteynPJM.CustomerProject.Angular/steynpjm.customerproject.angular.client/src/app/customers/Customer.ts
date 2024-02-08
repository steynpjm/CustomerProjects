export interface Customer {
  id: number;
  deletedIndicator: boolean;
  version: string;
  name: string;
  address1: string;
  address2: string;
  town: string;
  postalcode: string;
  country: string;
  project: any[];
  [key: string]: any;
}
