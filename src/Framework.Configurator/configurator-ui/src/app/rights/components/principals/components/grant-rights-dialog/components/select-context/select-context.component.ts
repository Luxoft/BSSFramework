import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output, TemplateRef, ViewChild } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { map, Observable, of, startWith, switchMap } from 'rxjs';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatCardModule } from '@angular/material/card';
import { IEntity } from '../../../view-principal-dialog/view-principal-dialog.component';
import { OverlayModule } from '@angular/cdk/overlay';
import { MatRippleModule } from '@angular/material/core';
import { ContextCheckPipe } from './select-context-check.pipe';

@Component({
  selector: 'app-select-context',
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
    ContextCheckPipe,
  ],
  providers: [ContextCheckPipe],
  templateUrl: './select-context.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SelectContextComponent implements OnInit {
  @Input() contextId: string | undefined;
  @Input() entitiesList!: IEntity[];
  @Output() selected = new EventEmitter<IEntity>();
  @Output() removeContext = new EventEmitter<IEntity>();

  @ViewChild('overlay') overlay!: TemplateRef<HTMLElement>;

  public searchcontrol = new FormControl('');
  public control = new FormControl<IEntity[]>([]);
  public entities: Observable<IEntity[]> | undefined;
  isOpen = false;

  constructor(private readonly http: HttpClient, public readonly contextCheck: ContextCheckPipe) {}

  public ngOnInit(): void {
    this.entities = this.searchcontrol.valueChanges.pipe(
      switchMap((search) =>
        search
          ? this.http.get<IEntity[]>(`api/context/${this.contextId}/entities?searchToken=${search}`).pipe(
              map((list) => [...this.entitiesList, ...list.filter((item) => !this.entitiesList.find((x) => x.Name === item.Name))]),
              map((list) => list.filter((i) => i.Name.toLocaleLowerCase().includes(search.toLocaleLowerCase() || '')))
            )
          : of(this.entitiesList)
      ),
      startWith(this.entitiesList || [])
    );
  }

  public select(entyty: IEntity, entities: IEntity[]): void {
    if (this.contextCheck.transform(entyty, entities)) {
      this.removeContext.emit(entyty);
    } else {
      this.selected.emit(entyty);
    }
  }

  public entitiesListCrop(entyty: IEntity[]): IEntity[] {
    return [...entyty.sort(this.sortByLength)].splice(0, 3);
  }

  public focus(): void {
    const input = this.overlay?.elementRef?.nativeElement?.ownerDocument?.querySelector('.search-header-input');
    if (input?.focus) {
      input.focus();
    }
  }

  private sortByLength(a: IEntity, b: IEntity): number {
    return a.Name.length - b.Name.length;
  }
}
