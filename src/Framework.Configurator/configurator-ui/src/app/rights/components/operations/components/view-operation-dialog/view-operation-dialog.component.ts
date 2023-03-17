import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import { MatBadgeModule } from '@angular/material/badge';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatListModule } from '@angular/material/list';
import { MatTabsModule } from '@angular/material/tabs';

import { IOperation } from '../../operations.component';

export interface IOperationDetails {
  BusinessRoles: string[];
  Principals: string[];
}

@Component({
  selector: 'app-view-role-dialog',
  standalone: true,
  imports: [CommonModule, MatTabsModule, MatDialogModule, MatButtonModule, MatListModule, MatBadgeModule],
  templateUrl: './view-operation-dialog.component.html',
  styleUrls: ['./view-operation-dialog.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ViewOperationDialogComponent implements OnInit {
  public details: IOperationDetails = { BusinessRoles: [], Principals: [] };

  constructor(@Inject(MAT_DIALOG_DATA) public data: IOperation, private readonly http: HttpClient, private readonly cdr: ChangeDetectorRef) {}

  ngOnInit(): void {
    this.http.get<IOperationDetails>(`api/operation/${this.data.Id}`).subscribe((x) => {
      this.details = x;
      this.cdr.markForCheck();
    });
  }
}
