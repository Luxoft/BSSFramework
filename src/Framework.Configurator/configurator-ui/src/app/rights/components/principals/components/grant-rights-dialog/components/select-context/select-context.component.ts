import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';
import { IEntity } from '../../../view-principal-dialog/view-principal-dialog.component';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-select-context[entitiesList]',
  standalone: true,
  imports: [CommonModule, MatIconModule],
  templateUrl: './select-context.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SelectContextComponent {
  @Input() entitiesList!: IEntity[];

  open = false;
}
