import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, ActivatedRoute, RouterLink } from '@angular/router';
import { TaskService } from '../../services/task';
import { AuthService } from '../../services/auth';
import { Task } from '../../models/models';

@Component({
  selector: 'app-task-form',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './task-form.html',
  styleUrl: './task-form.css'
})
export class TaskForm implements OnInit {
  isEdit = false;
  taskId: number | null = null;
  error = '';

  task: Partial<Task> = {
    title: '',
    description: '',
    status: 'Nowe',
    priority: 'Średni',
    category: '',
    dueDate: undefined
  };

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
      this.isEdit = true;
      this.taskId = +id;
      this.taskService.getOne(this.taskId).subscribe(task => this.task = task);
    }
  }

  save() {
    if (!this.task.title) {
      this.error = 'Tytuł jest wymagany';
      return;
    }

    const user = this.auth.getUser()!;
    this.task.userId = user.id;

    if (this.isEdit && this.taskId) {
      this.taskService.update(this.taskId, this.task).subscribe({
        next: () => this.router.navigate(['/tasks']),
        error: () => this.error = 'Błąd podczas zapisywania'
      });
    } else {
      this.taskService.create(this.task).subscribe({
        next: () => this.router.navigate(['/tasks']),
        error: () => this.error = 'Błąd podczas tworzenia zadania'
      });
    }
  }
}