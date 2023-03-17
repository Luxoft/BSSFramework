import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import { MatBadgeModule } from '@angular/material/badge';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatListModule } from '@angular/material/list';
import { MatTabsModule } from '@angular/material/tabs';

import { IOperation } from '../../../operations/operations.component';
import { IRole } from '../../roles.component';

export interface IRoleDetails {
  Operations: IOperation[];
  Principals: string[];
}

@Component({
  selector: 'app-view-role-dialog',
  standalone: true,
  imports: [CommonModule, MatTabsModule, MatDialogModule, MatButtonModule, MatListModule, MatBadgeModule],
  templateUrl: './view-role-dialog.component.html',
  styleUrls: ['./view-role-dialog.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ViewRoleDialogComponent implements OnInit {
  public details: IRoleDetails = { Operations: [], Principals: [] };

  constructor(@Inject(MAT_DIALOG_DATA) public data: IRole, private readonly http: HttpClient, private readonly cdr: ChangeDetectorRef) {}

  ngOnInit(): void {
    this.http.get<IRoleDetails>(`api/role/${this.data.Id}`).subscribe((x) => {
      this.details = x;
      this.cdr.markForCheck();
    });
  }
}
