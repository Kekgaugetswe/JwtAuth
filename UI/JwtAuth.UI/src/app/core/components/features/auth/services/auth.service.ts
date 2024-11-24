import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginRequest } from '../models/login-request';
import { LoginResponse } from '../models/login-response';
import { environment } from '../../../../../../environments/environment.development';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private readonly http: HttpClient) {

   }

  login(request:LoginRequest): Observable<LoginResponse> {
    return this. http.post<LoginResponse>(`${environment.apiBaseUrl}/api/auth/login`,{email:request.email, password:request.password})
  }
}
