import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, Inject, Self } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { BehaviorSubject, forkJoin, map, takeUntil, tap } from 'rxjs';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { DestroyService } from 'src/app/shared/destroy.service';
import { ContextsApiService } from 'src/app/shared/api-services/context.api.serivce';
import { IRoleContext } from '../../grant-rights-dialog/grant-rights-dialog.models';
import { MatCheckboxModule } from '@angular/material/checkbox';
@Component({
  selector: 'app-mass-insert-dialog[control][unitId]',
  standalone: true,
  imports: [CommonModule, MatInputModule, FormsModule, MatButtonModule, MatProgressSpinnerModule, MatCheckboxModule],
  templateUrl: './mass-insert-dialog.component.html',
  providers: [DestroyService, ContextsApiService],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MassInsertDialogComponent {
  loading = false;
  value = '';

  totalSubject = new BehaviorSubject<number>(0);
  currentSubject = new BehaviorSubject<number>(0);

  excludeComma = false;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: { unit: IRoleContext },
    public dialogRef: MatDialogRef<MassInsertDialogComponent>,
    @Self() private destroy$: DestroyService,
    private readonly contextsApiService: ContextsApiService
  ) {}

  save() {
    if (!this.value.trim()) {
      this.cancel();
    }

    const breakpoint = this.excludeComma ? new RegExp('[^\\n\\t;,]+', 'g') : new RegExp('[\\n,\\t;]+', 'g');
    const matches = this.value.split(breakpoint).map((x) => x.trim());

    this.loading = true;
    const matchesfiltred = [...new Set(matches)];
    if (!matchesfiltred[matchesfiltred.length - 1]) {
      matchesfiltred.length = matchesfiltred.length - 1;
    }
    this.totalSubject.next(matchesfiltred.length);
    forkJoin(
      matchesfiltred.map((s) =>
        this.contextsApiService
          .getEntities(this.data.unit.Id, s)
          .pipe(map((res) => res.find((g) => g.Name.toLocaleLowerCase() === s.toLocaleLowerCase()) || s))
          .pipe(tap(() => this.currentSubject.next(this.currentSubject.value + 1)))
      )
    )
      .pipe(takeUntil(this.destroy$))
      .subscribe((x) => {
        this.dialogRef.close(x);
      });
  }

  cancel() {
    this.dialogRef.close();
  }
}
