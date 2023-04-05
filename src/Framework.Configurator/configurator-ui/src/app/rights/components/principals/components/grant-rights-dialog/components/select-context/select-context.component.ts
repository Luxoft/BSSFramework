import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { IEntity } from '../../../view-principal-dialog/view-principal-dialog.component';

@Component({
  selector: 'app-select-context[entitiesList]',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './select-context.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SelectContextComponent {
  @Input() entitiesList!: IEntity[];
}
