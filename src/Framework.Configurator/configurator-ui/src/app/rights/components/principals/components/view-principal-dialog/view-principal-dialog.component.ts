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
import { PrincipalApiService } from 'src/app/shared/api.services';

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

export interface IContext {
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
  providers: [PrincipalApiService],
})
export class ViewPrincipalDialogComponent implements OnInit {
  public details: IPrincipalDetails | undefined;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: IPrincipal,
    private readonly principalApiService: PrincipalApiService,
    private readonly cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.principalApiService.getPrincipal(this.data.Id || '').subscribe((x) => {
      this.details = x;
      this.cdr.markForCheck();
    });
  }
}
