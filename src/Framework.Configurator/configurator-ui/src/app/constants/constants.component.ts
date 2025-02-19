import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableModule } from '@angular/material/table';
import { BehaviorSubject, Observable } from 'rxjs';

import { ConstantEditDialogComponent } from './components/constant-edit-dialog/constant-edit-dialog.component';

export interface ISystemConstant {
  Name: string;
  Description: string;
  Value: string;
}

@Component({
  selector: 'app-constants',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatButtonModule, MatDialogModule],
  templateUrl: './constants.component.html',
  styleUrls: ['./constants.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ConstantsComponent implements OnInit {
  public displayedColumns: string[] = ['Name', 'Value', 'action'];
  private systemConstant$ = new BehaviorSubject<ISystemConstant[]>([]);

  constructor(private readonly dialog: MatDialog, private readonly snackBar: MatSnackBar, private readonly http: HttpClient) {}

  ngOnInit(): void {
    this.refresh();
  }

  public openDialog(selectedItem: ISystemConstant): void {
    this.dialog
      .open(ConstantEditDialogComponent, { data: selectedItem, height: '300px', width: '500px' })
      .beforeClosed()
      .subscribe((newValue) => {
        if (newValue === undefined) {
          return;
        }

        this.http.post(`api/constant/${selectedItem.Name}`, JSON.stringify(newValue)).subscribe(() => {
          this.snackBar.open('System constant has been changed');
          this.refresh();
        });
      });
  }

  public get dataSource(): Observable<ISystemConstant[]> {
    return this.systemConstant$;
  }

  private refresh(): void {
    this.http.get<ISystemConstant[]>('api/constants').subscribe((x) => this.systemConstant$.next(x));
  }
}
