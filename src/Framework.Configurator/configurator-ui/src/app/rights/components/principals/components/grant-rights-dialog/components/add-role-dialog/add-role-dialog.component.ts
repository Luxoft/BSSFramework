import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { IRole } from 'src/app/rights/components/roles/roles.component';

@Component({
  selector: 'app-add-role-dialog',
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatButtonModule, MatFormFieldModule, MatInputModule, ReactiveFormsModule, MatAutocompleteModule],
  templateUrl: './add-role-dialog.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AddRoleDialogComponent implements OnInit {
  public roles: IRole[] = [];
  public control = new FormControl('');

  constructor(private readonly http: HttpClient, private readonly cdr: ChangeDetectorRef) {}

  ngOnInit(): void {
    this.http.get<IRole[]>('api/roles').subscribe((x) => {
      this.roles = x;
      this.cdr.markForCheck();
    });
  }

  public displayFn(role: IRole): string {
    return role?.Name ?? '';
  }
}
