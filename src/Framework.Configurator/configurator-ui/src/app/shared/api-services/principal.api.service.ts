import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IPrincipalDetails } from '../../rights/components/principals/components/view-principal-dialog/view-principal-dialog.component';
import { Observable } from 'rxjs';
import { IGrantedRight } from '../../rights/components/principals/components/grant-rights-dialog/grant-rights-dialog.models';

@Injectable()
export class PrincipalApiService {
  constructor(private readonly http: HttpClient) {}

  public getPrincipal(principalId: string): Observable<IPrincipalDetails> {
    return this.http.get<IPrincipalDetails>(`api/principal/${principalId}`);
  }

  public savePermissions(principalId: string, permissions: IGrantedRight[]): Observable<object> {
    return this.http.post(`api/principal/${principalId}/permissions`, permissions);
  }
}
