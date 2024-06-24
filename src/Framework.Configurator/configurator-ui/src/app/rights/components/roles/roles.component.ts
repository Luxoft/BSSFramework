import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatTableModule } from '@angular/material/table';
import { BehaviorSubject, Observable } from 'rxjs';

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

  constructor(private readonly http: HttpClient, private readonly dialog: MatDialog) {}

  ngOnInit(): void {
    this.http.get<IRole[]>('api/roles').subscribe((x) => this.roles$.next(x));
  }

  public get dataSource(): Observable<IRole[]> {
    return this.roles$;
  }

  public viewDetails(role: IRole): void {
    this.dialog.open(ViewRoleDialogComponent, { data: role, height: '600px', width: '600px' });
  }
}
