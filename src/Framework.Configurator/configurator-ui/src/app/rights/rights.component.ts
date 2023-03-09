import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { MatTabsModule } from '@angular/material/tabs';

import { OperationsComponent } from './components/operations/operations.component';
import { PrincipalsComponent } from './components/principals/principals.component';
import { RolesComponent } from './components/roles/roles.component';

@Component({
  selector: 'app-rights',
  standalone: true,
  imports: [CommonModule, MatTabsModule, PrincipalsComponent, RolesComponent, OperationsComponent],
  templateUrl: './rights.component.html',
  styleUrls: ['./rights.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RightsComponent {}
