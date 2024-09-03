import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { IContextDto, IEntity, IPermission, IPermissionDto } from '../principal.models';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FilterContextsPipe } from 'src/app/shared/filter-contexts.pipe';
import { SelectEntityComponent } from './select-entity/select-entity.component';
import { DestroyService } from 'src/app/shared/destroy.service';
import { MatInputModule } from '@angular/material/input';
import { IRoleContext } from '../grant-rights-dialog/grant-rights-dialog.models';
import { PermissionEditDialogService } from './permission-edit-dialog.service';
import { SearchFieldComponent } from '../grant-rights-dialog/components/search-header/search-header.component';
import { AddEntityControlComponent } from './add-entity-control/add-entity-control.component';
import { MatDatepicker, MatDatepickerModule } from '@angular/material/datepicker';
@Component({
  selector: 'app-permission-edit-dialog',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    FormsModule,
    MatIconModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    FilterContextsPipe,
    SelectEntityComponent,
    SearchFieldComponent,
    AddEntityControlComponent,
    MatDatepickerModule,
    ReactiveFormsModule,
    FormsModule,
  ],
  templateUrl: './permission-edit-dialog.component.html',
  styleUrls: ['./permission-edit-dialog.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [DestroyService, PermissionEditDialogService],
})
export class PermissionEditDialogComponent implements OnInit {
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: { permission: IPermission; units: IRoleContext[] },
    public dialogRef: MatDialogRef<PermissionEditDialogComponent>,
    public permissionEditDialogService: PermissionEditDialogService,
    public cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.dates.setValue({
      start: new Date(this.data.permission.StartDate || new Date()),
      end: this.data.permission.EndDate ? new Date(this.data.permission.EndDate) : null,
    });
  }

  dates = new FormGroup({
    start: new FormControl<Date>(new Date(), { nonNullable: true, validators: [Validators.required] }),
    end: new FormControl<Date | null>(null),
  });

  save() {
    if (!this.dates.valid) {
      this.dates.markAllAsTouched();
      return;
    }
    const dates = this.dates.getRawValue();
    if (this.permissionEditDialogService.forms.find((f) => f.invalid)) {
      // TODO: fix this
      this.permissionEditDialogService.forms.forEach((f) => f.markAllAsTouched());
      this.permissionEditDialogService.forms.forEach((f) => f.controls.entities.controls.forEach((g) => g.markAsTouched()));
      this.cdr.markForCheck();
      return;
    }
    const Contexts: IContextDto[] = this.permissionEditDialogService.forms
      .map((f) => f.value)
      .map((v) => ({
        Id: v.unit?.Id || '',
        Name: v.unit?.Name || '',
        Entities: ((v.entities as IEntity[]) || []).filter((entity, index, arr) => arr.findIndex((x) => x.Id === entity.Id) === index),
      }));

    const permission: IPermissionDto = {
      ...this.data.permission,
      StartDate: this.dateToLocalString(dates.start),
      EndDate: dates.end ? this.dateToLocalString(dates.end) : null,
      Contexts,
    };
    this.dialogRef.close(permission);
  }

  cancel() {
    this.dialogRef.close();
  }

  setToday(datepicker: MatDatepicker<Date>) {
    datepicker.select(new Date());
    datepicker.close();
  }

  dateToLocalString(date: Date) {
    date.setHours(0);
    date.setMinutes(0);
    date.setSeconds(0);
    date.setMilliseconds(0);

    const tzoffset = date.getTimezoneOffset() * 60000;
    return new Date(+date - tzoffset).toISOString().slice(0, -5);
  }
}
