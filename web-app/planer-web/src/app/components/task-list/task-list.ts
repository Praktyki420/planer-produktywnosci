import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { TaskService } from '../../services/task';
import { AuthService } from '../../services/auth';
import { Task } from '../../models/models';

@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './task-list.html'
})
export class TaskList implements OnInit {
  tasks: Task[] = [];
  filterStatus = '';
  filterCategory = '';

  constructor(
    private taskService: TaskService,
    private auth: AuthService,
    private router: Router
  ) {}

  ngOnInit() {
    if (!this.auth.isLoggedIn()) {
      this.router.navigate(['/login']);
      return;
    }
    this.loadTasks();
  }

  loadTasks() {
    const user = this.auth.getUser()!;
    this.taskService.getAll(user.id, this.filterStatus, this.filterCategory)
      .subscribe(tasks => this.tasks = tasks);
  }

  delete(id: number) {
    if (confirm('Czy na pewno usunąć zadanie?')) {
      this.taskService.delete(id).subscribe(() => this.loadTasks());
    }
  }

  logout() {
    this.auth.logout();
    this.router.navigate(['/login']);
  }
}