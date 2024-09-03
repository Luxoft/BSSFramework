export interface IPrincipalDetails {
  Permissions: IPermissionDto[];
}

// TODO: refactor interfaces
export interface IPermissionDto {
  Id: string;
  Role: string;
  RoleId?: string;
  Comment: string | null;
  Contexts: IContextDto[];
  StartDate?: string;
  EndDate?: string | null;
}

export interface IPermission extends IPermissionDto {
  Contexts: IContextWithRoleRestrictions[];
}

export interface IContextWithRoleRestrictions extends IContextDto {
  available: boolean;
  required: boolean;
}

export interface IContextDto {
  Id: string;
  Name: string;
  Entities: IEntity[];
}

export interface IEntity {
  Id: string;
  Name: string;
  recentlySavedValue?: boolean;
}
