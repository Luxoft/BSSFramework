import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import { MatBadgeModule } from '@angular/material/badge';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatTabsModule } from '@angular/material/tabs';

import { IPrincipal } from '../../principals.component';

export interface IPrincipalDetails {
  Permissions: IPermission[];
}

export interface IPermission {
  Id: string;
  Role: string;
  RoleId?: string;
  Comment: string | null;
  Contexts: IContext[];
}

interface IContext {
  Id: string;
  Name: string;
  Entities: IEntity[];
}

export interface IEntity {
  Id: string;
  Name: string;
}

@Component({
  selector: 'app-view-principal-dialog',
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatButtonModule, MatBadgeModule, MatTabsModule, MatCardModule, MatChipsModule],
  templateUrl: './view-principal-dialog.component.html',
  styleUrls: ['./view-principal-dialog.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ViewPrincipalDialogComponent implements OnInit {
  public details: IPrincipalDetails | undefined;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: IPrincipal,
    private readonly http: HttpClient,
    private readonly cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.http.get<IPrincipalDetails>(`api/principal/${this.data.Id}`).subscribe((x) => {
      this.details = x;
      this.cdr.markForCheck();
    });
  }
}
