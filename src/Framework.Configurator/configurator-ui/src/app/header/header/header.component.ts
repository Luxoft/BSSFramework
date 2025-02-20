import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterModule } from '@angular/router';
import { Observable, shareReplay, map } from 'rxjs';

interface IMenuItem {
  route: string;
  title: string;
}

const allMenuItems: (IMenuItem & { key: string })[] = [
  { route: '/', title: 'Rights', key: 'Main' },
  { route: '/events', title: 'Events', key: 'Events' },
  { route: '/constants', title: 'Constants', key: 'ApplicationVariables' },
];

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, MatToolbarModule, RouterModule],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class HeaderComponent {
  public readonly menuItems$: Observable<IMenuItem[]>;

  constructor(http: HttpClient) {
    this.menuItems$ = http.get<string[]>('api/modules').pipe(
      map((values) =>
        values
          .map((key) => allMenuItems.find((x) => x.key.toLowerCase() === key.toLowerCase()))
          .filter((x) => x !== undefined)
          .map((x) => x!)
      ),
      shareReplay(1)
    );
  }
}
