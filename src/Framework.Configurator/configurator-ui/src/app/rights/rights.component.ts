import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { MatTabsModule } from '@angular/material/tabs';
import { OperationsComponent } from './components/operations/operations.component';
import { PrincipalsComponent } from './components/principals/principals.component';
import { RolesComponent } from './components/roles/roles.component';
import { RolesApiService } from '../shared/api-services/role.api.service';
import { ContextsApiService } from '../shared/api-services/context.api.serivce';

@Component({
  selector: 'app-rights',
  standalone: true,
  imports: [CommonModule, MatTabsModule, PrincipalsComponent, RolesComponent, OperationsComponent],
  providers: [RolesApiService, ContextsApiService],
  templateUrl: './rights.component.html',
  styleUrls: ['./rights.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RightsComponent {}
