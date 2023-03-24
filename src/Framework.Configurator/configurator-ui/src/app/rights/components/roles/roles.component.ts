import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableModule } from '@angular/material/table';
import { BehaviorSubject, Observable } from 'rxjs';
import { ConfirmDialogComponent } from 'src/app/shared/confirm-dialog/confirm-dialog.component';

import { EditRoleDialogComponent, IEditRoleResult } from './components/edit-role-dialog/edit-role-dialog.component';
import { ViewRoleDialogComponent } from './components/view-role-dialog/view-role-dialog.component';

export interface IRole {
  Id: string | undefined;
  Name: string | undefined;
}

@Component({
  selector: 'app-roles',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatButtonModule, MatDialogModule],
  templateUrl: './roles.component.html',
  styleUrls: ['./roles.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RolesComponent implements OnInit {
  public displayedColumns: string[] = ['Name', 'action'];
  private roles$ = new BehaviorSubject<IRole[]>([]);

  constructor(private readonly http: HttpClient, private readonly dialog: MatDialog, private readonly snackBar: MatSnackBar) {}

  ngOnInit(): void {
    this.refresh();
  }

  public get dataSource(): Observable<IRole[]> {
    return this.roles$;
  }

  public remove(role: IRole): void {
    this.dialog
      .open(ConfirmDialogComponent, {
        data: { title: 'Are you sure you want to delete this role?', button: 'Yes, delete' },
        height: '170px',
        width: '400px',
      })
      .beforeClosed()
      .subscribe((isConfirm) => {
        if (!isConfirm) {
          return;
        }

        this.http.delete(`api/role/${role.Id}`).subscribe(() => {
          this.snackBar.open('Role has been deleted');
          this.refresh();
        });
      });
  }

  public add(): void {
    this.openEditDialog().subscribe((newRole) => {
      if (!newRole) {
        return;
      }

      const dto = { OperationIds: newRole.operationIds, Name: newRole.name };
      this.http.post('api/roles', dto).subscribe(() => {
        this.snackBar.open('Role has been added');
        this.refresh();
      });
    });
  }

  public edit(role: IRole): void {
    this.openEditDialog(role).subscribe((changedRole) => {
      if (!changedRole) {
        return;
      }

      const dto = { OperationIds: changedRole.operationIds };
      this.http.post(`api/role/${role.Id}`, dto).subscribe(() => {
        this.snackBar.open('Role has been changed');
        this.refresh();
      });
    });
  }

  public viewDetails(role: IRole): void {
    this.dialog.open(ViewRoleDialogComponent, { data: role, height: '600px', width: '600px' });
  }

  private refresh(): void {
    this.http.get<IRole[]>('api/roles').subscribe((x) => this.roles$.next(x));
  }

  private openEditDialog(role: IRole | undefined = undefined): Observable<IEditRoleResult> {
    return this.dialog.open(EditRoleDialogComponent, { data: role, height: '600px', width: '600px' }).beforeClosed();
  }
}
