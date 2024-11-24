import { Component } from '@angular/core';
import { LoginRequest } from '../models/login-request';
import { CardModule } from 'primeng/card';
import { PRIME_NG_IMPORTS } from '../../../../shared/shared.module';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [PRIME_NG_IMPORTS],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  model: LoginRequest;

  constructor() {
  this.model = {
    email: '',
    password: ''
  };

  }

  onFormSubmit(): void {}
}
