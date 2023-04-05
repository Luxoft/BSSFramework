import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, Inject, OnInit, Self } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatTableModule } from '@angular/material/table';
import { IPrincipal } from '../../principals.component';
import { SelectContextComponent } from './components/select-context/select-context.component';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { SearchFieldComponent } from './components/search-header/search-header.component';
import { ContextFilterPipe } from './context-filter.pipe';
import { ContextsApiService, PrincipalApiService } from 'src/app/shared/api.services';
import { RightsFilterPipe } from './rights-filter.pipe';
import { DestroyService } from 'src/app/shared/destroy.service';
import { MatMenuModule } from '@angular/material/menu';
import { GrantRightsDialogService } from './grant-rights-dialog.service';

@Component({
  selector: 'app-grant-rights-dialog',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    SelectContextComponent,
    MatTableModule,
    SearchFieldComponent,
    MatIconModule,
    MatButtonModule,
    ContextFilterPipe,
    RightsFilterPipe,
    MatMenuModule,
  ],
  providers: [PrincipalApiService, ContextsApiService, DestroyService, GrantRightsDialogService],
  templateUrl: './grant-rights-dialog.component.html',
  styleUrls: ['./grant-rights-dialog.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class GrantRightsDialogComponent implements OnInit {
  rights$ = this.grantRightsDialogService.rightsSubject.asObservable();
  allContexts$ = this.grantRightsDialogService.allContextsSubject.asObservable();
  contextFilter = this.grantRightsDialogService.contextFilter.asObservable();
  roletFilter = this.grantRightsDialogService.roletFilter.asObservable();

  public displayedColumns = ['actions', 'Role', 'Comment', 'Contexts'];

  constructor(@Inject(MAT_DIALOG_DATA) public data: IPrincipal, public readonly grantRightsDialogService: GrantRightsDialogService) {}

  ngOnInit(): void {
    if (this.data.Id) {
      this.grantRightsDialogService.init(this.data.Id);
    }
  }
}
