import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

import { ISystemConstant } from '../../constants.component';

@Component({
  selector: 'app-constant-edit-dialog',
  standalone: true,
  imports: [CommonModule, MatButtonModule, MatDialogModule, MatFormFieldModule, MatInputModule, FormsModule],
  templateUrl: './constant-edit-dialog.component.html',
  styleUrls: ['./constant-edit-dialog.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ConstantEditDialogComponent {
  public value: string;

  constructor(@Inject(MAT_DIALOG_DATA) public data: ISystemConstant) {
    this.value = data.Value;
  }
}
