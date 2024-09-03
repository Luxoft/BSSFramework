import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatIconModule } from '@angular/material/icon';
import { Observable, of, filter, switchMap, tap, startWith, debounceTime } from 'rxjs';
import { IEntity } from '../../principal.models';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ContextsApiService } from 'src/app/shared/api-services/context.api.serivce';

@Component({
  selector: 'app-select-entity[control][unitId]',
  standalone: true,
  imports: [CommonModule, MatAutocompleteModule, ReactiveFormsModule, MatIconModule, MatProgressSpinnerModule],
  providers: [ContextsApiService],
  templateUrl: './select-entity.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SelectEntityComponent implements OnInit {
  @Input() control!: FormControl<IEntity | null>;
  @Input() unitId!: string;

  visible = true;
  loading = false;
  constructor(private readonly contextsApiService: ContextsApiService) {}

  filteredOptions!: Observable<IEntity[]>;

  ngOnInit(): void {
    this.filteredOptions = this.control.valueChanges.pipe(
      startWith(this.control.value),
      debounceTime(300),
      filter((value) => typeof value === 'string' && value),
      tap(() => (this.loading = true)),
      switchMap((search) => (typeof search === 'string' ? this.contextsApiService.getEntities(this.unitId, search) : of([]))),
      tap(() => (this.loading = false))
    );
  }

  displayFn(value: IEntity | string): string {
    return typeof value === 'string' ? value : value?.Name ? value.Name : '';
  }
}
