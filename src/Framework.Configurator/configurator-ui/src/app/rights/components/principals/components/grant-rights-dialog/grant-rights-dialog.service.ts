import { Injectable, Injector, Self } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { BehaviorSubject, combineLatest, finalize, firstValueFrom, forkJoin, map, Observable, takeUntil, tap } from 'rxjs';
import { IRole } from '../../../roles/roles.component';
import { IPermission, IPermissionDto, IPrincipalDetails } from '../principal.models';
import { AddRoleDialogComponent } from './components/add-role-dialog/add-role-dialog.component';
import { ConfirmDialogComponent } from 'src/app/shared/confirm-dialog/confirm-dialog.component';
import { PermissionEditDialogComponent } from '../permission-edit-dialog/permission-edit-dialog.component';
import { PrincipalApiService } from 'src/app/shared/api-services/principal.api.service';
import { ContextsApiService } from 'src/app/shared/api-services/context.api.serivce';
import { DestroyService } from 'src/app/shared/destroy.service';
import { IGrantedRight, IRoleContext } from './grant-rights-dialog.models';
import { IRoleInfo, RolesApiService } from 'src/app/shared/api-services/role.api.service';

@Injectable()
export class GrantRightsDialogService {
  public rights$: Observable<IPermission[]>;
  public loadedSubject = new BehaviorSubject<boolean>(false);
  public allContextsSubject = new BehaviorSubject<IRoleContext[]>([]);
  public rolesSubject = new BehaviorSubject<IRoleInfo[]>([]);

  public rightsInternalSubject = new BehaviorSubject<IPrincipalDetails>({ Permissions: [] });

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
        this.rightsInternalSubject.next(rights);
        this.rolesSubject.next(roles);
        this.allContextsSubject.next(contexts);
      });
  }

  public remove(permission: IPermissionDto): void {
    this.dialog
      .open(ConfirmDialogComponent, {
        data: { title: 'Are you sure you want to delete this role?', button: 'Yes, delete' },
        height: '170px',
        width: '400px',
      })
      .afterClosed()
      .pipe(
        tap((x: IRole | string) => {
          if (!x) {
            return;
          }
          const rights = this.rightsInternalSubject.value;
          rights.Permissions = rights.Permissions.filter((x) => x.Id != permission.Id);
          this.rightsInternalSubject.next(rights);
        }),
        takeUntil(this.destroy$)
      )
      .subscribe();
  }

  public edit(permission: IPermissionDto, units: IRoleContext[]): void {
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
            const rights = this.rightsInternalSubject.value;
            const findIndex = rights.Permissions.findIndex((x) => x.Id === result.Id);
            if (findIndex > -1) {
              rights.Permissions[findIndex] = result;
              this.rightsInternalSubject.next(rights);
            }
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
        tap((x: IRole | string) => {
          if (!x || typeof x === 'string') {
            return;
          }

          const permission: IPermissionDto = { Id: '', RoleId: x.Id ?? '', Role: x.Name ?? '', Comment: '', Contexts: [] };
          const rights = this.rightsInternalSubject.value;
          rights.Permissions.unshift(permission);
          this.edit(permission, this.allContextsSubject.value);
          this.rightsInternalSubject.next(rights);
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
    const permissions = this.getResult();
    this.loadedSubject.next(false);
    return firstValueFrom(
      this.principalApiService.savePermissions(principalId, permissions).pipe(
        map(() => true),
        finalize(() => this.loadedSubject.next(true))
      )
    );
  }

  private getResult(): IGrantedRight[] {
    const rights = this.rightsInternalSubject.value;
    return rights.Permissions.map((x) => ({
      PermissionId: x.Id,
      RoleId: x.RoleId ?? '',
      Comment: x.Comment ?? '',
      Contexts: x.Contexts.map((c) => ({ Id: c.Id, Entities: c.Entities.map((e) => e.Id) })),
      StartDate: x.StartDate ? x.StartDate : null,
      EndDate: x.EndDate ? x.EndDate : null,
    }));
  }

  private buildPermissionsWithContextRestrictions(): Observable<IPermission[]> {
    const permissions$ = this.rightsInternalSubject.pipe(map((x) => x.Permissions));

    return combineLatest([permissions$, this.rolesSubject, this.allContextsSubject]).pipe(
      map(([permissions, roles, contexts]) => permissions.map((permission) => this.mapPermission(permission, roles, contexts)))
    );
  }

  private mapPermission(permission: IPermissionDto, roles: IRoleInfo[], contexts: IRoleContext[]): IPermission {
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
