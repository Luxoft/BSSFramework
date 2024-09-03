import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable()
export class RolesApiService {
  constructor(private readonly http: HttpClient) {}

  public getRoles(): Observable<object[]> {
    return this.http.get<object[]>('api/roles');
  }
}
