import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable, BehaviorSubject, map } from 'rxjs';
import { AuthUser } from '../../../core/models/auth.models';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private http = inject(HttpClient);

  constructor(private router: Router) { }

  // =========================
  // API URL
  // =========================
  private apiUrl = 'https://localhost:5000';

  // =========================
  // STATE
  // =========================
  private userSubject = new BehaviorSubject<AuthUser | null>(null);

  user$ = this.userSubject.asObservable();

  isLoggedIn$ = this.user$.pipe(
    map(user => !!user)
  );

  // =========================
  // LOGIN API
  // =========================
  login(payload: any): Observable<any> {
    const url = `${this.apiUrl}/auth/login`;
    return this.http.post(url, payload);
  }

  // =========================
  // SET USER
  // =========================
  setUser(user: AuthUser): void {
    this.userSubject.next(user);
    localStorage.setItem('auth_user', JSON.stringify(user));
  }

  // =========================
  // LOAD FROM STORAGE
  // =========================
  loadUserFromStorage(): void {
    const data = localStorage.getItem('auth_user');

    if (data) {
      try {
        const user: AuthUser = JSON.parse(data);
        this.userSubject.next(user);
      } catch {
        this.logout();
      }
    }
  }

  // =========================
  // LOGOUT
  // =========================
  logout(): void {
    this.userSubject.next(null);
    localStorage.removeItem('auth_user');

    this.router.navigate(['/']); // or '/' if landing page is root
  }

  // =========================
  // GETTERS
  // =========================
  getUser(): AuthUser | null {
    return this.userSubject.value;
  }

  getToken(): string | null {
    return this.userSubject.value?.token ?? null;
  }

  getEmail(): string | null {
    return this.userSubject.value?.email ?? null;
  }

  // =========================
  // SAFE HELPERS
  // =========================
  getUserFullName(): string {
    const user = this.userSubject.value;
    if (!user) return '';

    return `${user.firstName ?? ''} ${user.lastName ?? ''}`.trim();
  }

  getFirstName(): string {
    return this.userSubject.value?.firstName ?? '';
  }

  getLastName(): string {
    return this.userSubject.value?.lastName ?? '';
  }

  // =========================
  // AUTH STATUS
  // =========================
  isLoggedIn(): boolean {
    return !!this.userSubject.value;
  }

  hasToken(): boolean {
    return !!this.userSubject.value?.token;
  }
}
