import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Inject, OnInit, Self } from '@angular/core';
import { MatBadgeModule } from '@angular/material/badge';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatTabsModule } from '@angular/material/tabs';

import { IPrincipal } from '../../principals.component';
import { PrincipalApiService } from 'src/app/shared/api-services/principal.api.service';
import { DestroyService } from 'src/app/shared/destroy.service';
import { takeUntil } from 'rxjs';

export interface IPrincipalDetails {
  Permissions: IPermission[];
}

// TODO: refactor interfaces
export interface IPermission {
  Id: string;
  Role: string;
  RoleId?: string;
  Comment: string | null;
  Contexts: IContext[];
  StartDate?: string;
  EndDate?: string | null;
}

export interface IContext {
  Id: string;
  Name: string;
  Entities: IEntity[];
}

export interface IEntity {
  Id: string;
  Name: string;
  recentlySavedValue?: boolean;
}

@Component({
  selector: 'app-view-principal-dialog',
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatButtonModule, MatBadgeModule, MatTabsModule, MatCardModule, MatChipsModule],
  templateUrl: './view-principal-dialog.component.html',
  styleUrls: ['./view-principal-dialog.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [PrincipalApiService, DestroyService],
})
export class ViewPrincipalDialogComponent implements OnInit {
  public details: IPrincipalDetails | undefined;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: IPrincipal,
    private readonly principalApiService: PrincipalApiService,
    private readonly cdr: ChangeDetectorRef,
    @Self() private destroy$: DestroyService
  ) {}

  ngOnInit(): void {
    this.principalApiService
      .getPrincipal(this.data.Id || '')
      .pipe(takeUntil(this.destroy$))
      .subscribe((x) => {
        this.details = x;
        this.cdr.markForCheck();
      });
  }
}
