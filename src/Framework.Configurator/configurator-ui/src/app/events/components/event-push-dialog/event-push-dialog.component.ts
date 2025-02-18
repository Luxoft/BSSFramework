import { DialogRef } from '@angular/cdk/dialog';
import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, EventEmitter, Inject, Output, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';

import { IDomainType } from '../../events.component';
import { MatIconModule } from '@angular/material/icon';

export interface IPushedOperation {
  operationName: string | undefined;
  domainTypesIds: string | undefined;
  revision: number | undefined;
}
const SAVE_OPERATION = 'Save';

@Component({
  selector: 'app-event-push-dialog',
  standalone: true,
  imports: [
    CommonModule,
    MatButtonModule,
    MatDialogModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    FormsModule,
    MatIconModule,
  ],
  templateUrl: './event-push-dialog.component.html',
  styleUrls: ['./event-push-dialog.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class EventPushDialogComponent implements OnInit {
  public result: IPushedOperation = { operationName: undefined, domainTypesIds: undefined, revision: undefined };
  @Output() pushEvent = new EventEmitter<IPushedOperation>();
  constructor(@Inject(MAT_DIALOG_DATA) public data: IDomainType, private dialogRef: DialogRef) {}

  ngOnInit(): void {
    const saveOperation = this.data.Operations.find((operations) => operations.Name === SAVE_OPERATION);
    if (saveOperation) {
      this.result.operationName = saveOperation.Name;
    }
  }

  public close(result: IPushedOperation, closeAfter: boolean) {
    this.pushEvent.emit(result);
    if (closeAfter) {
      this.dialogRef.close();
    }
  }
}
