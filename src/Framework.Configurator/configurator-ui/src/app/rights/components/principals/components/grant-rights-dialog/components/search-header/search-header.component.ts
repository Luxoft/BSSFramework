import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnDestroy, OnInit, Output, TemplateRef, ViewChild } from '@angular/core';
import { ControlValueAccessor, FormControl, FormsModule, NG_VALUE_ACCESSOR, ReactiveFormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { takeUntil, tap } from 'rxjs';
import { ConnectionPositionPair, OverlayModule } from '@angular/cdk/overlay';

@Component({
  selector: 'app-search-header[label]',
  standalone: true,
  imports: [CommonModule, MatInputModule, FormsModule, MatIconModule, OverlayModule],
  styleUrls: ['search-header.component.scss'],
  templateUrl: './search-header.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: SearchFieldComponent,
    },
  ],
})
export class SearchFieldComponent implements ControlValueAccessor {
  @Input() label = '';
  @Input() width = '300px';
  @Output() search = new EventEmitter<string>();

  searchText!: string;

  isOpen = false;
  @ViewChild('overlay', { static: true }) private overlay!: TemplateRef<HTMLElement>;
  positions = [
    new ConnectionPositionPair({ originX: 'start', originY: 'top' }, { overlayX: 'start', overlayY: 'top' }),
    new ConnectionPositionPair({ originX: 'start', originY: 'top' }, { overlayX: 'start', overlayY: 'bottom' }),
  ];

  onChange!: (v: number) => void;
  onTouch!: (v: number) => void;

  writeValue(value: string) {
    this.searchText = value;
  }

  registerOnChange(fn: any) {
    this.onChange = fn;
  }

  registerOnTouched(onTouched: any) {
    this.onTouch = onTouched;
  }

  onSearch(): void {
    this.search.emit(this.searchText || '');
  }

  focus() {
    const input = this.overlay?.elementRef?.nativeElement?.ownerDocument?.querySelector('.search-header-input');
    const wrapper = this.overlay?.elementRef?.nativeElement?.ownerDocument?.querySelector('.wrapper');
    if (input?.focus) {
      input.focus();
    }
    if (wrapper) {
      wrapper.style.width = this.width;
    }
  }
}
