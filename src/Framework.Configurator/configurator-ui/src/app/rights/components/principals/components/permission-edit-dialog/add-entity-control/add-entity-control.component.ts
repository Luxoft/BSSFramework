import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatIconModule } from '@angular/material/icon';
import { Observable, of, filter, switchMap, tap, startWith, debounceTime, Subject, BehaviorSubject } from 'rxjs';
import { IEntity } from '../../view-principal-dialog/view-principal-dialog.component';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ContextsApiService } from 'src/app/shared/api.services';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-add-entity-control[unitId]',
  standalone: true,
  imports: [
    CommonModule,
    MatAutocompleteModule,
    ReactiveFormsModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatFormFieldModule,
    MatInputModule,
  ],
  providers: [ContextsApiService],
  templateUrl: './add-entity-control.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AddEntityControlComponent implements OnInit {
  @Input() unitId!: string;
  @Output() selected = new EventEmitter<IEntity>();
  control = new FormControl<IEntity | null>(null);

  loadedSubject = new BehaviorSubject<boolean>(true);
  constructor(private readonly contextsApiService: ContextsApiService) {}

  filteredOptions!: Observable<IEntity[]>;

  ngOnInit(): void {
    this.filteredOptions = this.control.valueChanges.pipe(
      startWith(this.control.value),
      debounceTime(300),
      filter((value) => typeof value === 'string' && value),
      tap(() => this.loadedSubject.next(false)),
      switchMap((search) => (typeof search === 'string' ? this.contextsApiService.getEntities(this.unitId, search) : of([]))),
      tap(() => this.loadedSubject.next(true))
    );
  }

  displayFn(value: IEntity | string): string {
    return typeof value === 'string' ? value : value?.Name ? value.Name : '';
  }

  onSetected() {
    const value = this.control.value;
    if (!value) {
      return;
    }
    this.selected.emit(value);
    this.control.reset();
  }
}
