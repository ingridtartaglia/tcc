import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import 'rxjs/add/operator/map';

@Injectable()
export class AuthService {
  private loggedIn = new BehaviorSubject<boolean>(false);

  get isLoggedIn() {
    return this.loggedIn.asObservable();
  }

  constructor(private http: HttpClient) { }

  login(email: string, password: string): Observable<any> {
    return this.http.post<any>('/api/Auth', { email: email, password: password })
      .map(res => {
        if (res) {
          const response = JSON.parse(res);
          const encryptedData = response.auth_token.split('.')[1];
          const decryptedToken = JSON.parse(window.atob(encryptedData));

          // Guarda as infos do usuário e o jwt token no localStorage para mantê-lo logado
          localStorage.setItem('auth_token', response.auth_token);

          // Guarda qual é o perfil do usuário logado no localStorage
          localStorage.setItem('user_role', decryptedToken.rol);

          this.loggedIn.next(true);
        }
        return res;
      });
  }

  logout() {
    localStorage.removeItem('auth_token');
    localStorage.removeItem('user_role');
    this.loggedIn.next(false);
  }
}
