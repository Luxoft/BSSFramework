import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, EventEmitter, OnDestroy } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { NavigationEnd, Router, RouterModule } from '@angular/router';
import { takeUntil } from 'rxjs';
import { filter, map } from 'rxjs/operators';

interface IMenuItem {
  route: string;
  title: string;
  active: boolean;
}

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, MatToolbarModule, RouterModule],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class HeaderComponent implements OnDestroy {
  private readonly destroy$ = new EventEmitter();

  public readonly menuItems: IMenuItem[] = [
    { route: '/', title: 'Rights', active: true },
    { route: '/events', title: 'Events', active: false },
    { route: '/constants', title: 'Constants', active: false },
  ];

  constructor(router: Router, cdr: ChangeDetectorRef) {
    router.events
      .pipe(
        takeUntil(this.destroy$),
        filter((e): e is NavigationEnd => e instanceof NavigationEnd),
        map((e) => e.urlAfterRedirects)
      )
      .subscribe((x) => {
        this.menuItems.forEach((i) => (i.active = i.route === x.split('?')[0]));
        cdr.markForCheck();
      });
  }

  ngOnDestroy(): void {
    this.destroy$.emit();
    this.destroy$.complete();
  }
}
