import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, Inject, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTableModule } from '@angular/material/table';
import { PrincipalApiService } from 'src/app/shared/api-services/principal.api.service';
import { DestroyService } from 'src/app/shared/destroy.service';
import { HighlightDirective } from 'src/app/shared/highlight.derective';
import { IPrincipal } from '../../principals.component';
import { SearchFieldComponent } from './components/search-header/search-header.component';
import { SelectContextComponent } from './components/select-context/select-context.component';
import { ContextFilterPipe } from './context-filter.pipe';
import { ContextStringFilterPipe } from './context-string-filter.pipe';
import { GrantRightsDialogService } from './grant-rights-dialog.service';
import { RightsFilterPipe } from './rights-filter.pipe';

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
    HighlightDirective,
    ContextStringFilterPipe,
    MatProgressSpinnerModule,
  ],
  providers: [PrincipalApiService, DestroyService, GrantRightsDialogService],
  templateUrl: './grant-rights-dialog.component.html',
  styleUrls: ['./grant-rights-dialog.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class GrantRightsDialogComponent implements OnInit {
  rights$ = this.grantRightsDialogService.rights$;
  allContexts$ = this.grantRightsDialogService.allContextsSubject.asObservable();
  filter$ = this.grantRightsDialogService.filter.asObservable();
  loaded$ = this.grantRightsDialogService.loadedSubject.asObservable();

  public displayedColumns = ['actions', 'Role', 'Comment', 'Contexts'];

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: IPrincipal,
    public readonly grantRightsDialogService: GrantRightsDialogService,
    private readonly dialog: MatDialogRef<GrantRightsDialogComponent>
  ) {}

  public ngOnInit(): void {
    if (this.data.Id) {
      this.grantRightsDialogService.init(this.data.Id);
    }
  }

  public save(): void {
    if (this.data.Id === undefined) {
      throw new Error("Can't save permissions for empty principal");
    }

    this.grantRightsDialogService.savePermissions(this.data.Id).then(() => this.dialog.close(true));
  }
}
