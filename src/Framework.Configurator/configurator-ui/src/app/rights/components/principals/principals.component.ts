import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, EventEmitter, OnDestroy, OnInit } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableModule } from '@angular/material/table';
import { BehaviorSubject, debounceTime, distinctUntilChanged, Observable, takeUntil } from 'rxjs';
import { ConfirmDialogComponent } from 'src/app/shared/confirm-dialog/confirm-dialog.component';

import { EditPrincipalDialogComponent } from './components/edit-principal-dialog/edit-principal-dialog.component';
import { GrantRightsDialogComponent, IGrantedRight } from './components/grant-rights-dialog/grant-rights-dialog.component';
import { ViewPrincipalDialogComponent } from './components/view-principal-dialog/view-principal-dialog.component';

export interface IPrincipal {
  Id: string | undefined;
  Name: string | undefined;
}

const DEBOUNCE = 300;

@Component({
  selector: 'app-principals',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatButtonModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    ReactiveFormsModule,
    MatCardModule,
  ],
  templateUrl: './principals.component.html',
  styleUrls: ['./principals.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PrincipalsComponent implements OnInit, OnDestroy {
  public displayedColumns: string[] = ['Name', 'action'];
  public control = new FormControl('');
  private principals$ = new BehaviorSubject<IPrincipal[]>([]);
  private runAs$ = new BehaviorSubject<string>('');
  private readonly destroy$ = new EventEmitter();

  constructor(private readonly http: HttpClient, private readonly dialog: MatDialog, private readonly snackBar: MatSnackBar) {}

  ngOnInit(): void {
    this.refresh();
    this.refreshRunAs();

    this.control.valueChanges
      .pipe(takeUntil(this.destroy$), distinctUntilChanged(), debounceTime(DEBOUNCE))
      .subscribe((x) => this.refresh(x ?? ''));
  }

  ngOnDestroy(): void {
    this.destroy$.emit();
    this.destroy$.complete();
  }

  public get dataSource(): Observable<IPrincipal[]> {
    return this.principals$;
  }

  public get runAsSource(): Observable<string> {
    return this.runAs$;
  }

  public viewDetails(principal: IPrincipal): void {
    this.dialog.open(ViewPrincipalDialogComponent, { data: principal, height: '800px', width: '1000px' });
  }

  public add(): void {
    this.openEditDialog().subscribe((name) => {
      if (!name) {
        return;
      }

      this.http.post('api/principals', JSON.stringify(name)).subscribe(() => {
        this.snackBar.open('Principal has been added');
        this.refresh();
      });
    });
  }

  public edit(principal: IPrincipal): void {
    this.openEditDialog(principal).subscribe((newName) => {
      if (!newName) {
        return;
      }

      this.http.post(`api/principal/${principal.Id}`, JSON.stringify(newName)).subscribe(() => {
        this.snackBar.open('Principal has been changed');
        this.refresh();
      });
    });
  }

  public remove(principal: IPrincipal): void {
    this.dialog
      .open(ConfirmDialogComponent, {
        data: { title: 'Are you sure you want to delete this principal?', button: 'Yes, delete' },
        height: '170px',
        width: '400px',
      })
      .beforeClosed()
      .subscribe((isConfirm) => {
        if (!isConfirm) {
          return;
        }

        this.http.delete(`api/principal/${principal.Id}`).subscribe(() => {
          this.snackBar.open('Principal has been deleted');
          this.refresh();
        });
      });
  }

  public grant(principal: IPrincipal): void {
    this.dialog
      .open(GrantRightsDialogComponent, { data: principal, height: '800px', width: '1000px' })
      .beforeClosed()
      .subscribe((x: IGrantedRight) => {
        if (!x) {
          return;
        }

        this.http.post(`api/principal/${principal.Id}/permissions`, x).subscribe(() => {
          this.snackBar.open('Rights has been granted');
          this.refresh();
        });
      });
  }

  public stopRunAs(): void {
    this.http.delete('api/principal/current/runAs').subscribe(() => this.refreshRunAs());
  }

  public runAs(principal: IPrincipal): void {
    this.dialog
      .open(ConfirmDialogComponent, {
        data: { title: `Are you sure you want to log in as ${principal.Name}?`, button: 'Yes, log in' },
        height: '170px',
        width: '400px',
      })
      .beforeClosed()
      .subscribe((isConfirm) => {
        if (!isConfirm) {
          return;
        }

        this.http.post('api/principal/current/runAs', JSON.stringify(principal.Name)).subscribe(() => this.refreshRunAs());
      });
  }

  private refresh(searchToken = ''): void {
    this.http.get<IPrincipal[]>(`api/principals?searchToken=${searchToken}`).subscribe((x) => this.principals$.next(x));
  }

  private refreshRunAs(): void {
    this.http.get<string>('api/principal/current/runAs').subscribe((x) => this.runAs$.next(x));
  }

  private openEditDialog(principal: IPrincipal | undefined = undefined): Observable<string> {
    return this.dialog.open(EditPrincipalDialogComponent, { data: principal, height: '250px', width: '400px' }).beforeClosed();
  }
}
