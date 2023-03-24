import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { from, Observable, throwError } from 'rxjs';
import { catchError, switchMap, tap } from 'rxjs/operators';

@Injectable()
export class HttpRequestsInterceptor implements HttpInterceptor {
  constructor(private readonly snackBar: MatSnackBar) {}

  public intercept(req: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const newReq = req.clone({ url: `${document.getElementsByTagName('base')[0].href}${req.url}` });

    return next.handle(newReq).pipe(
      catchError((z: Error) => {
        if (z instanceof HttpErrorResponse) {
          return from(this.getErrorMessage(z)).pipe(
            tap((x) => this.snackBar.open(x, undefined, { panelClass: 'error-message' })),
            switchMap(() => throwError(() => z))
          );
        }

        return throwError(() => z);
      })
    );
  }

  private getErrorMessage(response: HttpErrorResponse): Promise<string> {
    if (response.error instanceof Blob) {
      return response.error.text();
    }

    if (response.status !== 0) {
      return Promise.resolve(response.error as string);
    }

    if (!window.navigator.onLine) {
      return Promise.resolve('Connection to the server is lost! Your last changes might be not saved.');
    }

    return Promise.resolve('Something went wrong. Your last changes might be not saved. Please refresh the page.');
  }
}
