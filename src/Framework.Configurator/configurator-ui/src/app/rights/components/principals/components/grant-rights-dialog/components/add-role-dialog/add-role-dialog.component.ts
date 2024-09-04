import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { IRole } from 'src/app/rights/components/roles/roles.component';
import { RolesApiService } from 'src/app/shared/api-services/role.api.service';
import { FilterOptionsPipe } from 'src/app/shared/filter-options.pipe';

@Component({
  selector: 'app-add-role-dialog',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    ReactiveFormsModule,
    MatAutocompleteModule,
    FilterOptionsPipe,
  ],
  templateUrl: './add-role-dialog.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AddRoleDialogComponent {
  public roles$ = inject(RolesApiService).getRoles();
  public control = new FormControl('');

  public displayFn(role: IRole): string {
    return role?.Name ?? '';
  }
}
