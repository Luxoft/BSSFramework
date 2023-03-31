import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnDestroy, OnInit, Output, TemplateRef, ViewChild } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { takeUntil, tap } from 'rxjs';
import { ConnectionPositionPair, OverlayModule } from '@angular/cdk/overlay';

@Component({
  selector: 'app-search-header[label]',
  standalone: true,
  imports: [CommonModule, MatInputModule, ReactiveFormsModule, MatIconModule, OverlayModule],
  templateUrl: './search-header.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SearchFieldComponent implements OnInit, OnDestroy {
  @Input() label = '';
  @Output() search = new EventEmitter<string>();
  public searchcontrol = new FormControl('');
  private readonly destroy$ = new EventEmitter();
  isOpen = false;
  @ViewChild('overlay') overlay!: TemplateRef<HTMLElement>;
  positions = [
    new ConnectionPositionPair({ originX: 'start', originY: 'top' }, { overlayX: 'start', overlayY: 'top' }),
    new ConnectionPositionPair({ originX: 'start', originY: 'top' }, { overlayX: 'start', overlayY: 'bottom' }),
  ];

  ngOnInit(): void {
    this.searchcontrol.valueChanges
      .pipe(
        tap((x) => this.search.emit(x || '')),
        takeUntil(this.destroy$)
      )
      .subscribe();
  }

  ngOnDestroy(): void {
    this.destroy$.emit();
    this.destroy$.complete();
  }

  focus() {
    const input = this.overlay?.elementRef?.nativeElement?.ownerDocument?.querySelector('.search-header-input');
    if (input?.focus) {
      input.focus();
    }
  }
}
