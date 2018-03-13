import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

@Injectable()
export class AuthService {
  constructor(private http: HttpClient) { }

  login(email: string, password: string): Observable<any>{
    return this.http.post<any>('/api/Auth', { email: email, password: password })
      .map(res => {
        if (res) {
          // Guarda as infos do usuário e o jwt token no localStorage para mantê-lo logado
          localStorage.setItem('auth_token', res.auth_token);
        }
        return res;
      });
  }

  logout() {
    localStorage.removeItem('auth_token');
  }
}
