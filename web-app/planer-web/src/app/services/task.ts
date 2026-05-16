import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Task } from '../models/models';

@Injectable({ providedIn: 'root' })
export class TaskService {
  private api = 'http://localhost:5000/api/tasks';

  constructor(private http: HttpClient) {}

  getAll(userId: number, status?: string, category?: string): Observable<Task[]> {
    let params = new HttpParams().set('userId', userId);
    if (status)   params = params.set('status', status);
    if (category) params = params.set('category', category);
    return this.http.get<Task[]>(this.api, { params });
  }

  getOne(id: number): Observable<Task> {
    return this.http.get<Task>(`${this.api}/${id}`);
  }

  create(task: Partial<Task>): Observable<Task> {
    return this.http.post<Task>(this.api, task);
  }

  update(id: number, task: Partial<Task>): Observable<Task> {
    return this.http.put<Task>(`${this.api}/${id}`, task);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.api}/${id}`);
  }
}