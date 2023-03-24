import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';

import { IDomainType } from '../../events.component';

export interface IPushedOperation {
  operationId: string | undefined;
  domainTypesIds: string | undefined;
  revision: number | undefined;
}

@Component({
  selector: 'app-event-push-dialog',
  standalone: true,
  imports: [CommonModule, MatButtonModule, MatDialogModule, MatFormFieldModule, MatSelectModule, MatInputModule, FormsModule],
  templateUrl: './event-push-dialog.component.html',
  styleUrls: ['./event-push-dialog.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class EventPushDialogComponent {
  public result: IPushedOperation = { operationId: undefined, domainTypesIds: undefined, revision: undefined };

  constructor(@Inject(MAT_DIALOG_DATA) public data: IDomainType) {}
}
