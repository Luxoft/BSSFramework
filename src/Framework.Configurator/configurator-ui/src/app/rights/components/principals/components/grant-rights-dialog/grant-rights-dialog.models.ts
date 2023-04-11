export interface IRoleContext {
  Id: string;
  Name: string;
}

export interface IGrantedRight {
  PermissionId: string;
  RoleId: string;
  Comment: string;
  Contexts: IGrantedContext[];
}

interface IGrantedContext {
  Id: string;
  Entities: string[];
}
