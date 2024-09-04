import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, shareReplay } from 'rxjs';

@Injectable()
export class RolesApiService {
  private roles$ = this.http.get<IRoleInfo[]>('api/roles').pipe(shareReplay(1));
  constructor(private readonly http: HttpClient) {}

  public getRoles(): Observable<IRoleInfo[]> {
    return this.roles$;
  }
}

export interface IRoleInfo {
  Name: string;
  Id: string;
  Contexts: IContextRestriction[];
}

export interface IContextRestriction {
  Name: string;
  Required: boolean;
}
