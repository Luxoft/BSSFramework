import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { IEntity } from '../../../view-principal-dialog/view-principal-dialog.component';
import { MatIconModule } from '@angular/material/icon';
import { HighlightDirective } from 'src/app/shared/highlight.derective';

@Component({
  selector: 'app-select-context[entitiesList]',
  standalone: true,
  imports: [CommonModule, MatIconModule, HighlightDirective],
  templateUrl: './select-context.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SelectContextComponent {
  @Input() entitiesList!: IEntity[];
  @Input() highlight!: string | null;
  open = false;
}
