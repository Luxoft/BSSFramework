import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  EventEmitter,
  Inject,
  OnDestroy,
  OnInit,
} from '@angular/core';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatBadgeModule } from '@angular/material/badge';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { debounceTime, distinctUntilChanged, takeUntil } from 'rxjs';

import { IOperation } from '../../../operations/operations.component';
import { IRole } from '../../roles.component';
import { IRoleDetails } from '../view-role-dialog/view-role-dialog.component';

const DEBOUNCE = 300;

export interface IEditRoleResult {
  name: string;
  operationIds: string[];
}

@Component({
  selector: 'app-edit-role-dialog',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
    MatListModule,
    MatBadgeModule,
    ReactiveFormsModule,
  ],
  templateUrl: './edit-role-dialog.component.html',
  styleUrls: ['./edit-role-dialog.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class EditRoleDialogComponent implements OnInit, OnDestroy {
  public control = new FormControl('');
  public isEditMode: boolean;
  public operations: IOperation[] = [];
  public name: string | undefined;
  private allOperations: IOperation[] = [];
  private readonly destroy$ = new EventEmitter();

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: IRole | undefined,
    private readonly http: HttpClient,
    private readonly cdr: ChangeDetectorRef
  ) {
    this.isEditMode = Boolean(data?.Id);
    this.name = data?.Name;
  }

  ngOnInit(): void {
    this.http.get<IOperation[]>('api/operations').subscribe((x) => {
      this.allOperations = x;
      this.operations = x;

      if (!this.isEditMode) {
        this.cdr.markForCheck();
        return;
      }

      this.http.get<IRoleDetails>(`api/role/${this.data?.Id}`).subscribe((roleDetails) => {
        roleDetails.Operations.forEach((x) => {
          const selectedOperation = this.allOperations.find((z) => z.Id.toLowerCase() === x.Id.toLowerCase());
          if (selectedOperation) {
            selectedOperation.Selected = true;
          }
        });
        this.cdr.markForCheck();
      });
    });

    this.control.valueChanges.pipe(takeUntil(this.destroy$), distinctUntilChanged(), debounceTime(DEBOUNCE)).subscribe((x) => {
      this.operations = this.filter(x);
      this.cdr.markForCheck();
    });
  }

  ngOnDestroy(): void {
    this.destroy$.emit();
    this.destroy$.complete();
  }

  public getResult(): IEditRoleResult {
    return {
      name: this.name ?? '',
      operationIds: this.allOperations.filter((x) => x.Selected).map((z) => z.Id),
    };
  }

  private filter(searchToken: string | null): IOperation[] {
    return searchToken ? this.allOperations.filter((x) => x.Name.toLowerCase().includes(searchToken.toLowerCase())) : this.allOperations;
  }
}
