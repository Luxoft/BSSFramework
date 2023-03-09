import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

import { IPrincipal } from '../../principals.component';

@Component({
  selector: 'app-edit-principal-dialog',
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatButtonModule, MatFormFieldModule, MatInputModule, FormsModule],
  templateUrl: './edit-principal-dialog.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class EditPrincipalDialogComponent {
  public isEditMode: boolean;
  public name: string | undefined;

  constructor(@Inject(MAT_DIALOG_DATA) public data: IPrincipal | undefined) {
    this.isEditMode = Boolean(data?.Id);
    this.name = data?.Name;
  }
}
