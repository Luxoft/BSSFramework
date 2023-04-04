import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatCardModule } from '@angular/material/card';
import { OverlayModule } from '@angular/cdk/overlay';
import { MatRippleModule } from '@angular/material/core';
import { IEntity } from '../../view-principal-dialog/view-principal-dialog.component';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { IRoleContext } from '../../grant-rights-dialog/grant-rights-dialog.component';
import { forkJoin, map } from 'rxjs';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinner, MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-mass-insert-dialog[control][unitId]',
  standalone: true,
  imports: [
    CommonModule,
    MatInputModule,
    MatAutocompleteModule,
    ReactiveFormsModule,
    MatIconModule,
    OverlayModule,
    MatCheckboxModule,
    MatCardModule,
    MatRippleModule,
    FormsModule,
    MatButtonModule,
    MatProgressSpinnerModule,
  ],
  templateUrl: './mass-insert-dialog.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MassInsertDialogComponent {
  loading = false;
  value = '';

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: { unit: IRoleContext },
    public dialogRef: MatDialogRef<MassInsertDialogComponent>,

    private readonly http: HttpClient
  ) {}

  save() {
    if (!this.value.trim()) {
      this.cancel();
    }
    const breakpoint = /[\n,\t;]+(?=[^\n,\t;])/; // comma maybe incorrect!
    const matches = this.value.split(breakpoint).map((x) => x.trim());

    this.loading = true;
    forkJoin(
      [...new Set(matches)].map((s) =>
        this.http
          .get<IEntity[]>(`api/context/${this.data.unit.Id}/entities?searchToken=${s}`)
          .pipe(map((res) => res.find((g) => g.Name === s) || s))
      )
    ).subscribe((x) => {
      this.dialogRef.close(x);
    });
  }

  cancel() {
    this.dialogRef.close();
  }
}
