import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatTableModule } from '@angular/material/table';
import { BehaviorSubject, Observable } from 'rxjs';
import { ViewOperationDialogComponent } from './components/view-operation-dialog/view-operation-dialog.component';

export interface IOperation {
  Id: string;
  Name: string;
  Description: string;
  Selected: boolean;
}

@Component({
  selector: 'app-operations',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatButtonModule, MatDialogModule],
  templateUrl: './operations.component.html',
  styleUrls: ['./operations.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class OperationsComponent implements OnInit {
  public displayedColumns: string[] = ['Name', 'action'];
  private operations$ = new BehaviorSubject<IOperation[]>([]);

  constructor(private readonly http: HttpClient, private readonly dialog: MatDialog) {}

  ngOnInit(): void {
    this.http.get<IOperation[]>('api/operations').subscribe((x) => this.operations$.next(x));
  }

  public get dataSource(): Observable<IOperation[]> {
    return this.operations$;
  }

  public viewDetails(operation: IOperation): void {
    this.dialog.open(ViewOperationDialogComponent, { data: operation, height: '600px', width: '600px' });
  }
}
