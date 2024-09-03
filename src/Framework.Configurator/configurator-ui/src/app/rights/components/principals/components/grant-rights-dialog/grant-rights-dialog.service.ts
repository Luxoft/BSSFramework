import { Injectable, Self } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { BehaviorSubject, finalize, firstValueFrom, forkJoin, map, takeUntil, tap } from 'rxjs';
import { IRole } from '../../../roles/roles.component';
import { IPermission, IPrincipalDetails } from '../view-principal-dialog/view-principal-dialog.component';
import { AddRoleDialogComponent } from './components/add-role-dialog/add-role-dialog.component';
import { ConfirmDialogComponent } from 'src/app/shared/confirm-dialog/confirm-dialog.component';
import { PermissionEditDialogComponent } from '../permission-edit-dialog/permission-edit-dialog.component';
import { PrincipalApiService } from 'src/app/shared/api-services/principal.api.service';
import { ContextsApiService } from 'src/app/shared/api-services/context.api.serivce';
import { DestroyService } from 'src/app/shared/destroy.service';
import { IGrantedRight, IRoleContext } from './grant-rights-dialog.models';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class GrantRightsDialogService {
  public rightsSubject = new BehaviorSubject<IPrincipalDetails>({ Permissions: [] });
  public loadedSubject = new BehaviorSubject<boolean>(true);
  public allContextsSubject = new BehaviorSubject<IRoleContext[]>([]);
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

    private readonly http: HttpClient
  ) {}

  public init(principalId: string): void {
    this.loadedSubject.next(false);
    forkJoin([this.principalApiService.getPrincipal(principalId), this.contextsApiService.getContexts()])
      .pipe(takeUntil(this.destroy$))
      .subscribe(([rights, contexts]) => {
        this.loadedSubject.next(true);
        this.rightsSubject.next(rights);
        this.allContextsSubject.next(contexts);
      });
  }

  public remove(permission: IPermission): void {
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
          const rights = this.rightsSubject.value;
          rights.Permissions = rights.Permissions.filter((x) => x.Id != permission.Id);
          this.rightsSubject.next(rights);
        }),
        takeUntil(this.destroy$)
      )
      .subscribe();
  }

  public edit(permission: IPermission, units: IRoleContext[]): void {
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
            const rights = this.rightsSubject.value;
            const findIndex = rights.Permissions.findIndex((x) => x.Id === result.Id);
            if (findIndex > -1) {
              rights.Permissions[findIndex] = result;
              this.rightsSubject.next(rights);
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
      .open(AddRoleDialogComponent, { height: '250px', width: '400px' })
      .afterClosed()
      .pipe(
        tap((x: IRole | string) => {
          if (!x || typeof x === 'string') {
            return;
          }

          const permission: IPermission = { Id: '', RoleId: x.Id ?? '', Role: x.Name ?? '', Comment: '', Contexts: [] };
          const rights = this.rightsSubject.value;
          rights.Permissions.unshift(permission);
          this.edit(permission, this.allContextsSubject.value);
          this.rightsSubject.next(rights);
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
    const rights = this.rightsSubject.value;
    return rights.Permissions.map((x) => ({
      PermissionId: x.Id,
      RoleId: x.RoleId ?? '',
      Comment: x.Comment ?? '',
      Contexts: x.Contexts.map((c) => ({ Id: c.Id, Entities: c.Entities.map((e) => e.Id) })),
      StartDate: x.StartDate ? x.StartDate : null,
      EndDate: x.EndDate ? x.EndDate : null,
    }));
  }
}
