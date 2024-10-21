import { Injectable, Injector, Self } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { BehaviorSubject, combineLatest, finalize, firstValueFrom, forkJoin, map, Observable, takeUntil, tap } from 'rxjs';
import { IRole } from '../../../roles/roles.component';
import { IPermission, IPermissionDto, IPermissionUiDto } from '../principal.models';
import { AddRoleDialogComponent } from './components/add-role-dialog/add-role-dialog.component';
import { ConfirmDialogComponent } from 'src/app/shared/confirm-dialog/confirm-dialog.component';
import { PermissionEditDialogComponent } from '../permission-edit-dialog/permission-edit-dialog.component';
import { PrincipalApiService } from 'src/app/shared/api-services/principal.api.service';
import { ContextsApiService } from 'src/app/shared/api-services/context.api.serivce';
import { DestroyService } from 'src/app/shared/destroy.service';
import { IGrantedRight, IRoleContext } from './grant-rights-dialog.models';
import { IRoleInfo, RolesApiService } from 'src/app/shared/api-services/role.api.service';
import { getRandomGuid } from 'src/app/shared/utils/getRandomGuid';

@Injectable()
export class GrantRightsDialogService {
  public rights$: Observable<IPermission[]>;
  public loadedSubject = new BehaviorSubject<boolean>(false);
  public allContextsSubject = new BehaviorSubject<IRoleContext[]>([]);
  public rolesSubject = new BehaviorSubject<IRoleInfo[]>([]);

  private readonly permissionsSubject = new BehaviorSubject<IPermissionUiDto[]>([]);

  public filter = new BehaviorSubject<{
    contexts?: { contextId: string; search: string }[];
    role?: string;
    comment?: string;
  }>({});

  constructor(
    @Self() private destroy$: DestroyService,
    private readonly dialog: MatDialog,
    private readonly principalApiService: PrincipalApiService,
    private readonly contextsApiService: ContextsApiService,

    private readonly rolesApiService: RolesApiService,
    private readonly injector: Injector
  ) {
    this.rights$ = this.buildPermissionsWithContextRestrictions();
  }

  public init(principalId: string): void {
    this.loadedSubject.next(false);
    forkJoin([this.principalApiService.getPrincipal(principalId), this.contextsApiService.getContexts(), this.rolesApiService.getRoles()])
      .pipe(takeUntil(this.destroy$))
      .subscribe(([rights, contexts, roles]) => {
        this.loadedSubject.next(true);
        this.permissionsSubject.next(this.mapPermissionsToUiDto(rights.Permissions));
        this.rolesSubject.next(roles);
        this.allContextsSubject.next(contexts);
      });
  }

  public remove(permission: IPermissionUiDto): void {
    this.dialog
      .open(ConfirmDialogComponent, {
        data: { title: 'Are you sure you want to delete this role?', button: 'Yes, delete' },
        height: '170px',
        width: '400px',
      })
      .afterClosed()
      .pipe(
        tap((dialogResult: string) => {
          if (!dialogResult) {
            return;
          }
          this.removePermission(permission);
        }),
        takeUntil(this.destroy$)
      )
      .subscribe();
  }

  public edit(permissionDto: IPermissionUiDto, units: IRoleContext[]): void {
    const permission = this.mapPermission(permissionDto, this.rolesSubject.value, this.allContextsSubject.value);

    this.dialog
      .open(PermissionEditDialogComponent, {
        maxHeight: '90vh',
        maxWidth: '90vw',
        data: { permission, units },
      })
      .afterClosed()
      .pipe(
        tap((result) => {
          if (result) {
            this.updatePermission(result);
          }
          // TODO: fix next three lines
          const filterValue = this.filter.value;
          this.filter.next({ ...filterValue, comment: '' });
          this.filter.next({ ...filterValue });
        }),
        takeUntil(this.destroy$)
      )
      .subscribe();
  }

  public addPermission(): void {
    this.dialog
      .open(AddRoleDialogComponent, { injector: this.injector, height: '250px', width: '400px' })
      .afterClosed()
      .pipe(
        tap((role: IRole | string) => {
          if (!role || typeof role === 'string') {
            return;
          }

          const permission = this.createPermissionForRole(role);
          this.edit(permission, this.allContextsSubject.value);
        }),
        takeUntil(this.destroy$)
      )
      .subscribe();
  }

  public searchContext(contextId: string, search: string) {
    const contextFilter = this.filter.value.contexts || [];
    const index = contextFilter.findIndex((conext) => conext.contextId === contextId);
    if (index > -1) {
      contextFilter.splice(index, 1);
    }
    if (search) {
      contextFilter.push({ contextId, search });
    }
    this.filter.next({ ...this.filter.value, contexts: contextFilter });
  }

  searchRole(search: string) {
    this.filter.next({ ...this.filter.value, role: search });
  }

  public searchComment(search: string) {
    this.filter.next({ ...this.filter.value, comment: search });
  }

  public savePermissions(principalId: string): Promise<boolean> {
    const grantedRights = this.getPermissionsAsGrantedRights();
    this.loadedSubject.next(false);
    return firstValueFrom(
      this.principalApiService.savePermissions(principalId, grantedRights).pipe(
        map(() => true),
        finalize(() => this.loadedSubject.next(true))
      )
    );
  }

  private mapPermissionsToUiDto(permissions: IPermissionDto[]): IPermissionUiDto[] {
    return permissions.map((permission) => ({
      ...permission,
      uiPermissionId: permission.Id,
    }));
  }

  private createPermissionForRole(role: IRole): IPermissionUiDto {
    return {
      uiPermissionId: getRandomGuid(),
      Id: '',
      RoleId: role.Id ?? '',
      IsVirtual: false,
      Role: role.Name ?? '',
      Comment: '',
      Contexts: [],
    };
  }

  private removePermission(permission: IPermissionUiDto): void {
    const currentPermissions = this.permissionsSubject.value;
    const newPermissions = currentPermissions.filter((x) => x.uiPermissionId != permission.uiPermissionId);
    this.permissionsSubject.next(newPermissions);
  }

  private updatePermission(updatedPermission: IPermissionUiDto): void {
    const updatedPermissions = [...this.permissionsSubject.value];
    const currentPermissionIndex = updatedPermissions.findIndex((x) => x.uiPermissionId === updatedPermission.uiPermissionId);
    if (currentPermissionIndex > -1) {
      updatedPermissions[currentPermissionIndex] = updatedPermission;
    } else {
      updatedPermissions.unshift(updatedPermission);
    }

    this.permissionsSubject.next(updatedPermissions);
  }

  private getPermissionsAsGrantedRights(): IGrantedRight[] {
    const permissions = this.permissionsSubject.value;
    return permissions
      .map((x) => ({
        PermissionId: x.Id,
        RoleId: x.RoleId ?? '',
        IsVirtual: x.IsVirtual,
        Comment: x.Comment ?? '',
        Contexts: x.Contexts.map((c) => ({ Id: c.Id, Entities: c.Entities.map((e) => e.Id) })),
        StartDate: x.StartDate ? x.StartDate : null,
        EndDate: x.EndDate ? x.EndDate : null,
      }))
      .filter((x) => !x.IsVirtual);
  }

  private buildPermissionsWithContextRestrictions(): Observable<IPermission[]> {
    return combineLatest([this.permissionsSubject, this.rolesSubject, this.allContextsSubject]).pipe(
      map(([permissions, roles, contexts]) => permissions.map((permission) => this.mapPermission(permission, roles, contexts)))
    );
  }

  private mapPermission(permission: IPermissionUiDto, roles: IRoleInfo[], contexts: IRoleContext[]): IPermission {
    const currentRoleContexts = roles.find((r) => r.Name === permission.Role)?.Contexts ?? [];

    const Contexts = contexts.map((context) => {
      const permissionContext = permission.Contexts.find((c) => c.Id === context.Id);
      const roleContext = currentRoleContexts.find((c) => c.Name === context.Name);
      return {
        ...context,
        Entities: permissionContext?.Entities ?? [],
        required: roleContext?.Required ?? false,
        available: roleContext !== undefined,
      };
    });

    return { ...permission, Contexts };
  }
}
