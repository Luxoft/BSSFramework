import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MAT_DIALOG_DATA, MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { forkJoin } from 'rxjs';

import { IRole } from '../../../roles/roles.component';
import { IPrincipal } from '../../principals.component';
import { IEntity, IPermission, IPrincipalDetails } from '../view-principal-dialog/view-principal-dialog.component';
import { AddRoleDialogComponent } from './components/add-role-dialog/add-role-dialog.component';
import { SelectContextComponent } from './components/select-context/select-context.component';

interface IRoleContext {
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
    MatButtonModule,
    MatCardModule,
    MatChipsModule,
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
    SelectContextComponent,
  ],
  templateUrl: './grant-rights-dialog.component.html',
  styleUrls: ['./grant-rights-dialog.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class GrantRightsDialogComponent implements OnInit {
  public rights: IPrincipalDetails = { Permissions: [] };
  public allContexts: IRoleContext[] | undefined;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: IPrincipal,
    private readonly dialog: MatDialog,
    private readonly http: HttpClient,
    private readonly cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    forkJoin([this.http.get<IPrincipalDetails>(`api/principal/${this.data.Id}`), this.http.get<IRoleContext[]>('api/contexts')]).subscribe(
      ([rights, contexts]) => {
        this.rights = rights;
        this.allContexts = contexts;
        this.cdr.markForCheck();
      }
    );
  }

  public getEntities(permission: IPermission, context: IRoleContext): IEntity[] {
    return permission.Contexts.find((x) => x.Id === context.Id)?.Entities ?? [];
  }

  public remove(permission: IPermission): void {
    this.rights.Permissions = this.rights.Permissions.filter((x) => x.Id != permission.Id);
  }

  public removeContext(permission: IPermission, context: IRoleContext, entity: IEntity): void {
    const permissionContext = permission.Contexts.find((x) => x.Id === context.Id);
    if (!permissionContext) {
      return;
    }

    permissionContext.Entities = permissionContext.Entities.filter((x) => x.Id != entity.Id);
  }

  public addContext(permission: IPermission, context: IRoleContext, entity: IEntity): void {
    let permissionContext = permission.Contexts.find((x) => x.Id === context.Id);
    if (!permissionContext) {
      permissionContext = { ...context, Entities: [] };
      permission.Contexts.push(permissionContext);
    }

    permissionContext.Entities.push(entity);
  }

  public add(): void {
    this.dialog
      .open(AddRoleDialogComponent, { height: '250px', width: '400px' })
      .afterClosed()
      .subscribe((x: IRole | string) => {
        if (!x || typeof x === 'string') {
          return;
        }

        this.rights.Permissions.unshift({ Id: '', RoleId: x.Id ?? '', Role: x.Name ?? '', Comment: '', Contexts: [] });
        this.cdr.markForCheck();
      });
  }

  public getResult(): IGrantedRight[] {
    return this.rights.Permissions.map((x) => ({
      PermissionId: x.Id,
      RoleId: x.RoleId ?? '',
      Comment: x.Comment ?? '',
      Contexts: x.Contexts.map((c) => ({ Id: c.Id, Entities: c.Entities.map((e) => e.Id) })),
    }));
  }
}
