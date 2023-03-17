import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule, MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { MatInputModule } from '@angular/material/input';
import { debounceTime, distinctUntilChanged, iif, Observable, of, switchMap, takeUntil } from 'rxjs';

import { IEntity } from '../../../view-principal-dialog/view-principal-dialog.component';

const DEBOUNCE = 300;

@Component({
  selector: 'app-select-context',
  standalone: true,
  imports: [CommonModule, MatInputModule, MatAutocompleteModule, ReactiveFormsModule],
  templateUrl: './select-context.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SelectContextComponent implements OnInit, OnDestroy {
  @Input() contextId: string | undefined;
  @Output() selected = new EventEmitter<IEntity>();
  public control = new FormControl('');
  public entities: Observable<IEntity[]> | undefined;
  private readonly destroy$ = new EventEmitter();

  constructor(private readonly http: HttpClient) {}

  ngOnInit(): void {
    this.entities = this.control.valueChanges.pipe(
      takeUntil(this.destroy$),
      distinctUntilChanged(),
      debounceTime(DEBOUNCE),
      switchMap((x) => iif(() => Boolean(x), this.http.get<IEntity[]>(`api/context/${this.contextId}/entities?searchToken=${x}`), of([])))
    );
  }

  ngOnDestroy(): void {
    this.destroy$.emit();
    this.destroy$.complete();
  }

  public select(event: MatAutocompleteSelectedEvent): void {
    this.selected.emit(event.option.value);
    this.control.patchValue('');
  }
}
