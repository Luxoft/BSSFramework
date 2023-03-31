import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatTableModule } from '@angular/material/table';
import { BehaviorSubject, combineLatest, forkJoin, map, withLatestFrom } from 'rxjs';

import { IRole } from '../../../roles/roles.component';
import { IPrincipal } from '../../principals.component';
import { IEntity, IPermission, IPrincipalDetails } from '../view-principal-dialog/view-principal-dialog.component';
import { AddRoleDialogComponent } from './components/add-role-dialog/add-role-dialog.component';
import { SelectContextComponent } from './components/select-context/select-context.component';
import { MatIconModule } from '@angular/material/icon';
import { ConfirmDialogComponent } from 'src/app/shared/confirm-dialog/confirm-dialog.component';
import { MatButtonModule } from '@angular/material/button';
import { SearchFieldComponent } from './components/search-header/search-header.component';

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

@Component({
  selector: 'app-grant-rights-dialog',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    FormsModule,
    SelectContextComponent,
    MatTableModule,
    SearchFieldComponent,
    MatIconModule,
    MatButtonModule,
  ],
  templateUrl: './grant-rights-dialog.component.html',
  styleUrls: ['./grant-rights-dialog.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class GrantRightsDialogComponent implements OnInit {
  public rightsSubject = new BehaviorSubject<IPrincipalDetails>({ Permissions: [] });

  public allContexts: IRoleContext[] | undefined;
  public displayedColumns = ['delete', 'Role', 'Comment', 'Contexts'];

  public contextFilter = new BehaviorSubject<{ contextId: string; search: string }[]>([]);
  public roletFilter = new BehaviorSubject<string>('');
  public rightsObj = combineLatest([this.contextFilter.asObservable(), this.roletFilter.asObservable()]).pipe(
    withLatestFrom(this.rightsSubject.asObservable()),
    map(([[filter, roleFilter], list]) => {
      if (filter.length > 0) {
        list = {
          ...list,
          Permissions: list.Permissions.filter(
            (permissions) =>
              permissions.Contexts.filter(
                (x) =>
                  x.Entities.filter((entity) => {
                    const search = filter.find((f) => f.contextId === x.Id);
                    return search ? entity.Name.toLocaleLowerCase().includes(search.search.toLocaleLowerCase()) : true;
                  }).length && filter.find((c) => c.contextId === x.Id)
              ).length
          ),
        };
      }
      return { ...list, Permissions: list.Permissions.filter((c) => c.Role.toLocaleLowerCase().includes(roleFilter.toLocaleLowerCase())) };
    })
  );

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: IPrincipal,
    private readonly dialog: MatDialog,
    private readonly http: HttpClient,
    private readonly cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    forkJoin([this.http.get<IPrincipalDetails>(`api/principal/${this.data.Id}`), this.http.get<IRoleContext[]>('api/contexts')]).subscribe(
      ([rights, contexts]) => {
        this.rightsSubject.next(rights);
        this.allContexts = contexts;
        this.roletFilter.next('');
      }
    );
  }

  public getEntities(permission: IPermission, context: IRoleContext): IEntity[] {
    return permission.Contexts.find((x) => x.Id === context.Id)?.Entities ?? [];
  }

  public remove(permission: IPermission): void {
    this.dialog
      .open(ConfirmDialogComponent, {
        data: { title: 'Are you sure you want to delete this role?', button: 'Yes, delete' },
        height: '170px',
        width: '400px',
      })
      .afterClosed()
      .subscribe((x: IRole | string) => {
        if (!x) {
          return;
        }
        const rights = this.rightsSubject.value;
        rights.Permissions = rights.Permissions.filter((x) => x.Id != permission.Id);
        this.rightsSubject.next(rights);
        this.roletFilter.next('');
      });
  }

  public removeContext(permission: IPermission, context: IRoleContext, entity: IEntity): void {
    const permissionContext = permission.Contexts.find((x) => x.Id === context.Id);
    if (!permissionContext) {
      return;
    }
    permissionContext.Entities = permissionContext.Entities.filter((x) => x.Id != entity.Id);

    const rights = this.rightsSubject.value;
    const findIndex = rights.Permissions.findIndex((x) => x.Id === permission.Id);
    if (findIndex > -1) {
      const permission = rights.Permissions[findIndex];

      const permissionContext = permission.Contexts.find((x) => x.Id === context.Id);
      if (!permissionContext) {
        return;
      }
      permissionContext.Entities = permissionContext.Entities.filter((x) => x.Id != entity.Id);
      rights.Permissions[findIndex] = permission;
      this.rightsSubject.next(rights);
    }
  }

  public addContext(permission: IPermission, context: IRoleContext, entity: IEntity): void {
    let permissionContext = permission.Contexts.find((x) => x.Id === context.Id);
    if (!permissionContext) {
      permissionContext = { ...context, Entities: [] };
      permission.Contexts.push(permissionContext);
    }

    permissionContext.Entities.push(entity);
    const rights = this.rightsSubject.value;
    const findIndex = rights.Permissions.findIndex((x) => x.Id === permission.Id);
    if (findIndex > -1) {
      rights.Permissions[findIndex] = permission;
      this.rightsSubject.next(rights);
    }
  }

  public add(): void {
    this.dialog
      .open(AddRoleDialogComponent, { height: '250px', width: '400px' })
      .afterClosed()
      .subscribe((x: IRole | string) => {
        if (!x || typeof x === 'string') {
          return;
        }
        const rights = this.rightsSubject.value;
        rights.Permissions.unshift({ Id: '', RoleId: x.Id ?? '', Role: x.Name ?? '', Comment: '', Contexts: [] });
        this.rightsSubject.next(rights);
        this.roletFilter.next('');
      });
  }

  public getResult(): IGrantedRight[] {
    const rights = this.rightsSubject.value;
    return rights.Permissions.map((x) => ({
      PermissionId: x.Id,
      RoleId: x.RoleId ?? '',
      Comment: x.Comment ?? '',
      Contexts: x.Contexts.map((c) => ({ Id: c.Id, Entities: c.Entities.map((e) => e.Id) })),
    }));
  }

  searchContext(contextId: string, search: string) {
    const contextFilter = this.contextFilter.value;
    const index = contextFilter.findIndex((conext) => conext.contextId === contextId);
    if (index > -1) {
      contextFilter.splice(index, 1);
    }
    if (search) {
      contextFilter.push({ contextId, search });
    }
    this.contextFilter.next(contextFilter);
  }

  searchRole(search: string) {
    this.roletFilter.next(search);
  }
}
