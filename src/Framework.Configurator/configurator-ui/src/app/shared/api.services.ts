import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IPrincipalDetails } from '../rights/components/principals/components/view-principal-dialog/view-principal-dialog.component';
import { Observable } from 'rxjs';

@Injectable()
export class PrincipalApiService {
  constructor(private readonly http: HttpClient) {}

  public getPrincipal(principalId: string): Observable<IPrincipalDetails> {
    return this.http.get<IPrincipalDetails>(`api/principal/${principalId}`);
  }
}
