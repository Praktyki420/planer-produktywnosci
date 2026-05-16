import { Component, OnInit } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { Router, ActivatedRoute, RouterLink } from '@angular/router';
import { TaskService } from '../../services/task';
import { AuthService } from '../../services/auth';
import { Task } from '../../models/models';

@Component({
  selector: 'app-task-detail',
  standalone: true,
  imports: [CommonModule, RouterLink, DatePipe],
  templateUrl: './task-detail.html',
  styleUrl: './task-detail.css'
})
export class TaskDetail implements OnInit {
  task: Task | null = null;
  error = '';

  constructor(
    private taskService: TaskService,
    private auth: AuthService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    if (!this.auth.isLoggedIn()) {
      this.router.navigate(['/login']);
      return;
    }

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.taskService.getOne(+id).subscribe({
        next: (task) => this.task = task,
        error: () => this.error = 'Nie znaleziono zadania'
      });
    }
  }

  delete() {
    if (!this.task) return;
    if (confirm('Czy na pewno usunąć to zadanie?')) {
      this.taskService.delete(this.task.id).subscribe(() => {
        this.router.navigate(['/tasks']);
      });
    }
  }
}