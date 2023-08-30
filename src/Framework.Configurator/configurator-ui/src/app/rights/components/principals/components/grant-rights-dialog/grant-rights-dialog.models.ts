export interface IRoleContext {
  Id: string;
  Name: string;
}

// TODO: refactor interfaces
export interface IGrantedRight {
  PermissionId: string;
  RoleId: string;
  StartDate: string | null;
  EndDate: string | null;
  Comment: string;
  Contexts: IGrantedContext[];
}

interface IGrantedContext {
  Id: string;
  Entities: string[];
}
