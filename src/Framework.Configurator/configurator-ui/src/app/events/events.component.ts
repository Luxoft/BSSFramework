import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, EventEmitter, OnDestroy, OnInit } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableModule } from '@angular/material/table';
import { BehaviorSubject, debounceTime, distinctUntilChanged, Observable, takeUntil } from 'rxjs';

import { EventPushDialogComponent, IPushedOperation } from './components/event-push-dialog/event-push-dialog.component';

export interface IDomainType {
  Id: string;
  Name: string;
  Namespace: string;
  Operations: IDomainTypeOperation[];
}

interface IDomainTypeOperation {
  Id: string;
  Name: string;
}

const DEBOUNCE = 300;

@Component({
  selector: 'app-events',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatButtonModule, MatDialogModule, MatFormFieldModule, MatInputModule, ReactiveFormsModule],
  templateUrl: './events.component.html',
  styleUrls: ['./events.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class EventsComponent implements OnInit, OnDestroy {
  public displayedColumns: string[] = ['Name', 'action'];
  public control = new FormControl('');
  private domainTypes$ = new BehaviorSubject<IDomainType[]>([]);
  private readonly destroy$ = new EventEmitter();
  private allItems: IDomainType[] = [];

  constructor(private readonly dialog: MatDialog, private readonly snackBar: MatSnackBar, private readonly http: HttpClient) {}

  ngOnInit(): void {
    this.refresh();

    this.control.valueChanges
      .pipe(takeUntil(this.destroy$), distinctUntilChanged(), debounceTime(DEBOUNCE))
      .subscribe((x) => this.domainTypes$.next(this.filter(x)));
  }

  ngOnDestroy(): void {
    this.destroy$.emit();
    this.destroy$.complete();
  }

  public openDialog(selectedItem: IDomainType): void {
    this.dialog
      .open(EventPushDialogComponent, { data: selectedItem, height: '560px', width: '500px' })
      .beforeClosed()
      .subscribe((result: IPushedOperation) => {
        if (!result) {
          return;
        }

        const dto = { Revision: result.revision, Ids: result.domainTypesIds };
        this.http.post(`api/domainType/${selectedItem.Id}/operation/${result.operationId}`, dto).subscribe(() => {
          this.snackBar.open(`Event '${selectedItem.Name}' has been pushed`);
        });
      });
  }

  public get dataSource(): Observable<IDomainType[]> {
    return this.domainTypes$;
  }

  private filter(searchToken: string | null): IDomainType[] {
    return searchToken ? this.allItems.filter((x) => x.Name.toLowerCase().includes(searchToken.toLowerCase())) : this.allItems;
  }

  private refresh(): void {
    this.http.get<IDomainType[]>('api/domainTypes').subscribe((x) => {
      this.allItems = x;
      this.domainTypes$.next(this.allItems);
    });
  }
}
