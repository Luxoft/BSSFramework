import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { IContext, IEntity, IPermission } from '../view-principal-dialog/view-principal-dialog.component';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FilterContextsPipe } from 'src/app/shared/filter-contexts.pipe';
import { SelectEntityComponent } from './select-entity/select-entity.component';
import { DestroyService } from 'src/app/shared/destroy.service';
import { MatInputModule } from '@angular/material/input';
import { IRoleContext } from '../grant-rights-dialog/grant-rights-dialog.models';
import { PermissionEditDialogService } from './permission-edit-dialog.service';

@Component({
  selector: 'app-grant-rights-dialog',
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
  ],
  templateUrl: './permission-edit-dialog.component.html',
  styleUrls: ['./permission-edit-dialog.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [DestroyService, PermissionEditDialogService],
})
export class PermissionEditDialogComponent {
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: { permission: IPermission; units: IRoleContext[] },
    public dialogRef: MatDialogRef<PermissionEditDialogComponent>,
    public permissionEditDialogService: PermissionEditDialogService,
    public cdr: ChangeDetectorRef
  ) {}

  save() {
    if (this.permissionEditDialogService.forms.find((f) => f.invalid)) {
      // TODO: fix this
      this.permissionEditDialogService.forms.forEach((f) => f.markAllAsTouched());
      this.permissionEditDialogService.forms.forEach((f) => f.controls.entities.controls.forEach((g) => g.markAsTouched()));
      this.cdr.markForCheck();
      return;
    }
    const Contexts: IContext[] = this.permissionEditDialogService.forms
      .map((f) => f.value)
      .map((v) => ({
        Id: v.unit?.Id || '',
        Name: v.unit?.Name || '',
        Entities: ((v.entities as IEntity[]) || []).filter((entity, index, arr) => arr.findIndex((x) => x.Id === entity.Id) === index),
      }));
    const permission: IPermission = { ...this.data.permission, Contexts };
    this.dialogRef.close(permission);
  }

  cancel() {
    this.dialogRef.close();
  }
}
