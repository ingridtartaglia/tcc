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
          let response = JSON.parse(res);
          // Guarda as infos do usuário e o jwt token no localStorage para mantê-lo logado
          localStorage.setItem('auth_token', response.auth_token);

          var encryptedData = response.auth_token.split('.')[1];
          let decryptedToken = JSON.parse(window.atob(encryptedData));
          localStorage.setItem('user_role', decryptedToken.rol);
        }
        return res;
      });
  }

  logout() {
    localStorage.removeItem('auth_token');
    localStorage.removeItem('user_role');
  }
}
