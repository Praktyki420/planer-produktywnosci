import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../models/models';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private api = 'http://localhost:5000/api/users';

  constructor(private http: HttpClient) {}

  register(username: string, email: string, password: string): Observable<User> {
    return this.http.post<User>(`${this.api}/register`, { username, email, password });
  }

  login(email: string, password: string): Observable<User> {
    return this.http.post<User>(`${this.api}/login`, { email, password });
  }

  saveUser(user: User): void {
    localStorage.setItem('user', JSON.stringify(user));
  }

  getUser(): User | null {
    const u = localStorage.getItem('user');
    return u ? JSON.parse(u) : null;
  }

  logout(): void {
    localStorage.removeItem('user');
  }

  isLoggedIn(): boolean {
    return this.getUser() !== null;
  }
}