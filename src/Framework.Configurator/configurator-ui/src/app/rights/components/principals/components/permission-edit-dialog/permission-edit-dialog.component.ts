import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormArray,
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialog, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatTableModule } from '@angular/material/table';
import { IContext, IEntity, IPermission, IPrincipalDetails } from '../view-principal-dialog/view-principal-dialog.component';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { IRoleContext } from '../grant-rights-dialog/grant-rights-dialog.component';
import { FilterContextsPipe } from 'src/app/shared/filter-contexts.pipe';
import { MatAutocompleteModule, MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, combineLatest, filter, forkJoin, shareReplay, switchMap, tap } from 'rxjs';
import { SelectEntityComponent } from './select-entity/select-entity.component';
import { MassInsertDialogComponent } from './mass-insert-dialog/mass-insert-dialog.component';
import { MatMenuModule } from '@angular/material/menu';

export function forbiddenContextValidator(): ValidatorFn {
  return (control: AbstractControl<IEntity | null | string>): ValidationErrors | null => {
    const value = control.value;

    const forbidden = typeof value === 'string' || !value ? true : false;
    return forbidden ? { forbiddenContext: { value: control.value } } : null;
  };
}

@Component({
  selector: 'app-grant-rights-dialog',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    FormsModule,
    MatTableModule,
    MatIconModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    FilterContextsPipe,
    MatAutocompleteModule,
    ReactiveFormsModule,
    SelectEntityComponent,
    MatMenuModule,
  ],
  templateUrl: './permission-edit-dialog.component.html',
  styleUrls: ['./permission-edit-dialog.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PermissionEditDialogComponent {
  public forms: FormGroup<{
    unit: FormControl<IRoleContext>;
    entities: FormArray<FormControl<IEntity | null>>;
  }>[] = [];
  validators = [Validators.required, forbiddenContextValidator()];
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: { permission: IPermission; units: IRoleContext[] },
    private readonly dialog: MatDialog,
    public dialogRef: MatDialogRef<PermissionEditDialogComponent>,
    private readonly cdr: ChangeDetectorRef
  ) {
    this.data.units.forEach((unit) =>
      this.forms.push(
        new FormGroup({
          unit: new FormControl<IRoleContext>(unit, { nonNullable: true }),
          entities: new FormArray<FormControl<IEntity | null>>(
            this.getEntities(data.permission, unit).map((entity) => new FormControl<IEntity>(entity, this.validators))
          ),
        })
      )
    );
  }

  public getEntities(permission: IPermission, context: IRoleContext): IEntity[] {
    return permission.Contexts.find((x) => x.Id === context.Id)?.Entities ?? [];
  }

  add(entities: FormArray<FormControl<IEntity | null>>, value: IEntity | null) {
    entities.push(new FormControl<IEntity | null>(value, this.validators));
    this.cdr.markForCheck();
  }

  remove(entities: FormArray<FormControl<IEntity | null>>, context: FormControl<IEntity | null>) {
    const index = entities.value.findIndex((value) => value?.Id === context.value?.Id);
    if (index > -1) {
      entities.removeAt(index);
      this.cdr.markForCheck();
    }
  }

  massInsert(unit: IRoleContext, entities: FormArray<FormControl<IEntity | null>>) {
    const dialogRef = this.dialog.open(MassInsertDialogComponent, {
      width: '500px',
      data: { unit },
    });
    dialogRef
      .afterClosed()
      .pipe(
        filter((x) => Boolean(x)),
        tap((mass: (IEntity | null)[]) => mass.forEach((x) => this.add(entities, x)))
      )
      .subscribe();
  }

  save() {
    if (this.forms.find((f) => f.invalid)) {
      this.forms.forEach((f) => f.markAllAsTouched());
      this.forms.forEach((f) => f.controls.entities.controls.forEach((g) => g.setValue(g.value)));
      this.cdr.markForCheck();
      return;
    }
    const Contexts: IContext[] = this.forms
      .map((f) => f.value)
      .map((v) => ({
        Id: v.unit?.Id || '',
        Name: v.unit?.Name || '',
        Entities: (v.entities as IEntity[]) || [],
      }));
    const permission: IPermission = { ...this.data.permission, Contexts };
    this.dialogRef.close(permission);
  }

  cancel() {
    this.dialogRef.close();
  }
}
