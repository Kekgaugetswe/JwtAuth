import { Component } from '@angular/core';
import { LoginRequest } from '../models/login-request';
import { CardModule } from 'primeng/card';
import { PRIME_NG_IMPORTS } from '../../../../shared/shared.module';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [PRIME_NG_IMPORTS],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  model: LoginRequest;

  constructor( private readonly authService: AuthService) {
  this.model = {
    email: '',
    password: ''
  };

  }

  onFormSubmit(): void {
    this.authService.login(this.model)
    .subscribe({
      next: (response)=> {
        console.log(response);
      },
      error: (err) => {
        console.error('Login error:', err);
      }
    })
  }
}
