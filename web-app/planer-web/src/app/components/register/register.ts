import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class Register {
  username = '';
  email = '';
  password = '';
  error = '';
  success = '';

  constructor(private auth: AuthService, private router: Router) {}

  register() {
    if (!this.username || !this.email || !this.password) {
      this.error = 'Wypełnij wszystkie pola';
      return;
    }
    this.auth.register(this.username, this.email, this.password).subscribe({
      next: () => {
        this.success = 'Konto utworzone! Możesz się zalogować.';
        setTimeout(() => this.router.navigate(['/login']), 1500);
      },
      error: (err) => this.error = err.error || 'Błąd rejestracji'
    });
  }
}