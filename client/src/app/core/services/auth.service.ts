import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

import { environment } from '../../../environments/environment';
import { LoginRequest, LoginResponse } from '../models/auth.model';

const TOKEN_STORAGE_KEY = 'trackmanagement.token';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);

  readonly isAuthenticated = signal(!!localStorage.getItem(TOKEN_STORAGE_KEY));

  login(request: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${environment.apiUrl}/auth/login`, request).pipe(
      tap((response) => {
        localStorage.setItem(TOKEN_STORAGE_KEY, response.token);
        this.isAuthenticated.set(true);
      }),
    );
  }

  logout(): void {
    localStorage.removeItem(TOKEN_STORAGE_KEY);
    this.isAuthenticated.set(false);
  }

  getToken(): string | null {
    return localStorage.getItem(TOKEN_STORAGE_KEY);
  }
}
