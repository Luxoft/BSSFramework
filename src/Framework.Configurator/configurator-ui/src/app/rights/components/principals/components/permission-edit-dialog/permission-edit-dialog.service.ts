import { ChangeDetectorRef, Inject, Injectable, Self } from '@angular/core';
import { AbstractControl, FormArray, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { IContextWithRoleRestrictions, IEntity, IPermission, IPermissionDto } from '../principal.models';
import { filter, takeUntil, tap } from 'rxjs';
import { MassInsertDialogComponent } from './mass-insert-dialog/mass-insert-dialog.component';
import { DestroyService } from 'src/app/shared/destroy.service';
import { IRoleContext } from '../grant-rights-dialog/grant-rights-dialog.models';

export function forbiddenContextValidator(): ValidatorFn {
  return (control: AbstractControl<IEntity | null | string>): ValidationErrors | null => {
    const value = control.value;
    const forbidden = typeof value === 'string' || !value ? true : false;
    return forbidden ? { forbiddenContext: { value: control.value } } : null;
  };
}

@Injectable()
export class PermissionEditDialogService {
  public forms: FormGroup<{
    unit: FormControl<IContextWithRoleRestrictions>;
    entities: FormArray<FormControl<IEntity | null>>;
  }>[] = [];
  validators = [Validators.required, forbiddenContextValidator()];
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: { permission: IPermission; units: IRoleContext[] },
    private readonly dialog: MatDialog,
    @Self() private destroy$: DestroyService,
    private readonly cdr: ChangeDetectorRef
  ) {
    this.data.permission.Contexts.filter((x) => x.available).forEach((unit) =>
      this.forms.push(
        new FormGroup({
          unit: new FormControl<IContextWithRoleRestrictions>(unit, { nonNullable: true }),
          entities: new FormArray<FormControl<IEntity | null>>(
            this.getEntities(data.permission, unit).map(
              (entity) => new FormControl<IEntity>({ ...entity, recentlySavedValue: true }, this.validators)
            )
          ),
        })
      )
    );
  }

  public getEntities(permission: IPermissionDto, context: IRoleContext): IEntity[] {
    return permission.Contexts.find((x) => x.Id === context.Id)?.Entities ?? [];
  }

  public add(entities: FormArray<FormControl<IEntity | null>>, value: IEntity | null) {
    entities.push(new FormControl<IEntity | null>(value, this.validators));
    this.cdr.markForCheck();
  }

  public remove(entities: FormArray<FormControl<IEntity | null>>, context: FormControl<IEntity | null>) {
    const index = entities.value.findIndex((value) => value?.Id === context.value?.Id);
    if (index > -1) {
      entities.removeAt(index);
      this.cdr.markForCheck();
    }
  }

  public massInsert(unit: IRoleContext, entities: FormArray<FormControl<IEntity | null>>) {
    const dialogRef = this.dialog.open(MassInsertDialogComponent, {
      width: '500px',
      data: { unit },
    });
    dialogRef
      .afterClosed()
      .pipe(
        filter((x) => Boolean(x)),
        tap((mass: (IEntity | null)[]) =>
          mass
            .filter((value) => !(value?.Id && entities.controls.find((control) => control.value?.Id === value?.Id)))
            .forEach((x) => this.add(entities, x))
        ),
        takeUntil(this.destroy$)
      )
      .subscribe();
  }
}
