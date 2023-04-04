import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output, TemplateRef, ViewChild } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { map, Observable, of, filter, switchMap, tap, startWith } from 'rxjs';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatCardModule } from '@angular/material/card';
import { OverlayModule } from '@angular/cdk/overlay';
import { MatRippleModule } from '@angular/material/core';
import { IEntity } from '../../view-principal-dialog/view-principal-dialog.component';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-select-entity[control][unitId]',
  standalone: true,
  imports: [
    CommonModule,
    MatInputModule,
    MatAutocompleteModule,
    ReactiveFormsModule,
    MatIconModule,
    OverlayModule,
    MatCheckboxModule,
    MatCardModule,
    MatRippleModule,
    MatProgressSpinnerModule,
  ],
  templateUrl: './select-entity.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SelectEntityComponent implements OnInit {
  @Input() control!: FormControl<IEntity | null>;
  @Input() unitId!: string;

  visible = true;
  loading = false;
  constructor(private readonly http: HttpClient) {}

  filteredOptions!: Observable<IEntity[]>;

  ngOnInit(): void {
    this.filteredOptions = this.control.valueChanges.pipe(
      startWith(this.control.value),
      filter((value) => typeof value === 'string' && value),
      tap(() => (this.loading = true)),
      switchMap((search) =>
        typeof search === 'string' ? this.http.get<IEntity[]>(`api/context/${this.unitId}/entities?searchToken=${search}`) : of([])
      ),
      tap(() => (this.loading = false))
    );
  }

  displayFn(value: IEntity | string): string {
    return typeof value === 'string' ? value : value?.Name ? value.Name : '';
  }
}
